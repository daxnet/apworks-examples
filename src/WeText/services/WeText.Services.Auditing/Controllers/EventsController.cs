using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Apworks.Repositories;
using WeText.Services.Auditing.Models;
using Apworks.Integration.AspNetCore.DataServices;

namespace WeText.Services.Auditing.Controllers
{
    [Produces("application/json")]
    [Route("api/events")]
    public class EventsController : DataServiceController<Guid, EventItem>
    {
        public EventsController(IRepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
