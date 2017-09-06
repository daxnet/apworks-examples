using Apworks.Integration.AspNetCore.DataServices;
using Apworks.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeText.Services.Accounts.Models;
using Microsoft.AspNetCore.Mvc;
using Apworks.Messaging;
using WeText.Services.Accounts.Events;

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

        public override Task<IActionResult> Post([FromBody] User aggregateRoot)
        {
            aggregateRoot.DateRegistered = DateTime.UtcNow;
            return base.Post(aggregateRoot);
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
                this.integrationMessageBus.Publish(new AccountAuthenticatedEvent(userName, false, errorMessage));
                throw new EntityNotFoundException(errorMessage);
            }

            if (users.First().Password == password)
            {
                var user = users.First();
                this.integrationMessageBus.Publish(new AccountAuthenticatedEvent(userName, true));
                return Ok();
            }

            this.integrationMessageBus.Publish(new AccountAuthenticatedEvent(userName, false, "Incorrect password."));
            return Unauthorized();
        }
    }
}
