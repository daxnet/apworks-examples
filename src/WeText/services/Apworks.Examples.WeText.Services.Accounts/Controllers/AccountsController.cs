using Apworks.Examples.WeText.Services.Accounts.Models;
using Apworks.Integration.AspNetCore.DataServices;
using Apworks.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apworks.Examples.WeText.Services.Accounts.Controllers
{
    public class AccountsController : DataServiceController<Guid, Account>
    {
        public AccountsController(IRepositoryContext repositoryContext)
            : base(repositoryContext)
        { }
    }
}
