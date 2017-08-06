using Apworks.Integration.AspNetCore.DataServices;
using Apworks.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeText.Services.Accounts.Models;
using Microsoft.AspNetCore.Mvc;

namespace WeText.Services.Accounts.Controllers
{
    public sealed class UsersController : DataServiceController<Guid, User>
    {
        public UsersController(IRepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

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
                throw new EntityNotFoundException($"The user with the user name '{userName}' does not exist.");
            }

            if (users.First().Password == password)
            {
                var user = users.First();
                return Ok();
            }

            return Unauthorized();
        }
    }
}
