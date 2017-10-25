using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Apworks.Repositories;
using Apworks.Integration.AspNetCore.DataServices;
using WeText.Services.Auditing.Models;

namespace WeText.Services.Auditing.Controllers
{
    [Produces("application/json")]
    [Route("api/authentications")]
    public class EventsController : DataServiceController<Guid, Authentication>
    {
        public EventsController(IRepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
