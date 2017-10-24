using WeText.Services.Accounts.Models;
using WeText.Services.Accounts.Events;
using Apworks.Integration.AspNetCore.DataServices;
using System;
using Apworks.Messaging;
using Apworks.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace WeText.Services.Accounts.Controllers
{
    /// <summary>
    /// Represents the controller that manages users.
    /// </summary>
    public sealed class UsersController : DataServiceController<Guid, User>
    {
        private readonly IMessageBus integrationMessageBus;

        public UsersController(IRepositoryContext repositoryContext,
            IMessageBus integrationMessageBus)
            : base(repositoryContext)
        {
            this.integrationMessageBus = integrationMessageBus;
        }

        public override async Task<IActionResult> Post([FromBody] User aggregateRoot)
        {
            var userNameExists = await this.Repository.ExistsAsync(x => x.UserName == aggregateRoot.UserName);
            if (userNameExists)
            {
                throw new EntityAlreadyExistsException($"User with the user name '{aggregateRoot.UserName}' already exists.");
            }

            var emailExists = await this.Repository.ExistsAsync(x => x.Email == aggregateRoot.Email);
            if (emailExists)
            {
                throw new EntityAlreadyExistsException($"User with the email of '{aggregateRoot.Email}' already exists.");
            }

            aggregateRoot.DateRegistered = DateTime.UtcNow;
            
            var actionResult = await base.Post(aggregateRoot);
            await this.PublishMessageAsync<AccountCreatedEvent>(new AccountCreatedEvent(aggregateRoot.UserName, aggregateRoot.DisplayName, aggregateRoot.Email));

            return actionResult;
        }

        /// <summary>
        /// Authenticates the user by using the given user name and password.
        /// </summary>
        /// <param name="model">The model which contains the user name and password data.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] dynamic model)
        {
            var userName = (string)model.userName;
            var password = (string)model.password;
            if (string.IsNullOrEmpty(userName))
            {
                throw new InvalidArgumentException("The user name has not been specified.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidArgumentException("The password has not been specified.");
            }

            var users = (await this.Repository.FindAllAsync(x => x.UserName == userName)).ToList();
            if (users == null || users.Count == 0)
            {
                var errorMessage = $"The user with the user name '{userName}' does not exist.";
                await this.PublishMessageAsync(new AccountAuthenticatedEvent(userName, false, errorMessage));
                throw new EntityNotFoundException(errorMessage);
            }

            if (users.First().Password == password)
            {
                var user = users.First();
                await this.PublishMessageAsync(new AccountAuthenticatedEvent(userName, true));
                return Ok();
            }

            await this.PublishMessageAsync(new AccountAuthenticatedEvent(userName, false, "Incorrect password."));
            return Unauthorized();
        }

        private async Task PublishMessageAsync<TMessage>(TMessage msg)
            where TMessage : IMessage
        {
            await this.integrationMessageBus.PublishAsync(msg, "wetext.integration");
        }
    }
}
