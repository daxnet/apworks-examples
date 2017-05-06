using Apworks.Examples.CustomerService.Model;
using Apworks.Integration.AspNetCore.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apworks.Repositories;

namespace Apworks.Examples.CustomerService.MongoDB.Controllers
{
    public class CustomersController : DataServiceController<Guid, Customer>
    {
        public CustomersController(IRepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
