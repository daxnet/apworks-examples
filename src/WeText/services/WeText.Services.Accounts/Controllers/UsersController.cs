using Apworks.Events;
using Apworks.Integration.AspNetCore.DataServices;
using Apworks.Messaging;
using Apworks.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeText.Services.Accounts.Models;
using WeText.Services.Shared.Events;

namespace WeText.Services.Accounts.Controllers
{
    /// <summary>
    /// Represents the users API controller.
    /// </summary>
    public sealed class UsersController : DataServiceController<Guid, User>
    {
        private readonly IEventPublisher eventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context instance used by the current API controller to manage
        /// the lifecyle of the aggregates.</param>
        /// <param name="eventPublisher">The event publisher.</param>
        public UsersController(IRepositoryContext repositoryContext,
            IEventPublisher eventPublisher)
            : base(repositoryContext)
        {
            this.eventPublisher = eventPublisher;
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
            await this.eventPublisher.PublishAsync(msg, "events.accounts");
        }
    }
}
