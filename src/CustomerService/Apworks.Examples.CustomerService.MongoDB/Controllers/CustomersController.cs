using Apworks.Examples.CustomerService.Model;
using Apworks.Integration.AspNetCore.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apworks.Repositories;

namespace Apworks.Examples.CustomerService.MongoDB.Controllers
{
    /// <summary>
    /// Represents the controller that provides the APIs for Customer entity.
    /// </summary>
    /// <seealso cref="Integration.AspNetCore.DataServices.DataServiceController{System.Guid, Model.Customer}" />
    public class CustomersController : DataServiceController<Guid, Customer>
    {
        public CustomersController(IRepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
