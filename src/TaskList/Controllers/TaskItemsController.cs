using Apworks.Examples.TaskList.Models;
using Apworks.Integration.AspNetCore.DataServices;
using Apworks.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace Apworks.Examples.TaskList.Controllers
{
    public sealed class TaskItemsController : DataServiceController<Guid, TaskItem>
    {
        public TaskItemsController(IRepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

        public override Task<IActionResult> Post([FromBody] TaskItem aggregateRoot)
        {
            aggregateRoot.CreationTime = DateTime.UtcNow;
            return base.Post(aggregateRoot);
        }
    }
}
