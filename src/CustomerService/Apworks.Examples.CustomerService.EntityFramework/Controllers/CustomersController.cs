
using Apworks.Examples.CustomerService.EntityFramework.Model;
using Apworks.Integration.AspNetCore.DataServices;
using Apworks.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apworks.Examples.CustomerService.EntityFramework.Controllers
{
    public class CustomersController : DataServiceController<Guid, Customer>
    {
        public CustomersController(IRepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
