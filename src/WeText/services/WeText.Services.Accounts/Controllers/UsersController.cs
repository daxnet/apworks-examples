using Apworks.Integration.AspNetCore.DataServices;
using Apworks.Repositories;
using System;
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
    }
}
