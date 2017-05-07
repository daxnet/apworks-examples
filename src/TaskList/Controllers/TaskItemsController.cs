// ==================================================================================================================                                                                                          
//        ,::i                                                           BBB                
//       BBBBBi                                                         EBBB                
//      MBBNBBU                                                         BBB,                
//     BBB. BBB     BBB,BBBBM   BBB   UBBB   MBB,  LBBBBBO,   :BBG,BBB :BBB  .BBBU  kBBBBBF 
//    BBB,  BBB    7BBBBS2BBBO  BBB  iBBBB  YBBJ :BBBMYNBBB:  FBBBBBB: OBB: 5BBB,  BBBi ,M, 
//   MBBY   BBB.   8BBB   :BBB  BBB .BBUBB  BB1  BBBi   kBBB  BBBM     BBBjBBBr    BBB1     
//  BBBBBBBBBBBu   BBB    FBBP  MBM BB. BB BBM  7BBB    MBBY .BBB     7BBGkBB1      JBBBBi  
// PBBBFE0GkBBBB  7BBX   uBBB   MBBMBu .BBOBB   rBBB   kBBB  ZBBq     BBB: BBBJ   .   iBBB  
//BBBB      iBBB  BBBBBBBBBE    EBBBB  ,BBBB     MBBBBBBBM   BBB,    iBBB  .BBB2 :BBBBBBB7  
//vr7        777  BBBu8O5:      .77r    Lr7       .7EZk;     L77     .Y7r   irLY  JNMMF:    
//               LBBj
//
// Apworks Application Development Framework
// Copyright (C) 2009-2017 by daxnet.
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ==================================================================================================================

using Apworks.Examples.TaskList.Models;
using Apworks.Integration.AspNetCore.DataServices;
using Apworks.Repositories;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Apworks.Examples.TaskList.Controllers
{
    /// <summary>
    /// Represents the API controller which handles the task item services.
    /// </summary>
    /// <seealso cref="Apworks.Integration.AspNetCore.DataServices.DataServiceController{System.Guid, Apworks.Examples.TaskList.Models.TaskItem}" />
    public sealed class TaskItemsController : DataServiceController<Guid, TaskItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItemsController"/> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context that is used by the current
        /// <see cref="T:Apworks.Integration.AspNetCore.DataServices.DataServiceController`2" /> for managing the object lifecycle.</param>
        public TaskItemsController(IRepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

        /// <summary>
        /// Handles an HTTP POST request, which will create the task item in the repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be created.</param>
        /// <returns>Controller action execution result.</returns>
        public override Task<IActionResult> Post([FromBody] TaskItem aggregateRoot)
        {
            // Before creating the task item, assign the date of the creation property.
            aggregateRoot.CreationTime = DateTime.UtcNow;

            // Invokes the base class to create the task item.
            return base.Post(aggregateRoot);
        }

        /// <summary>
        /// Updates the status for all the task items.
        /// </summary>
        /// <remarks>From this example you can see that in Apworks Data Service, it is very
        /// easy to extend the DataServiceController to implement your own RESTful API.</remarks>
        /// <param name="done">The status to be updated to all of the task items.</param>
        /// <returns>Controller action execution result.</returns>
        [HttpPost]
        [Route("all")]
        public async Task<IActionResult> UpdateAllStatus([FromBody] bool done)
        {
            var taskItems = await this.Repository.FindAllAsync(x => x.Done != done);
            foreach(var taskItem in taskItems)
            {
                taskItem.Done = done;
                await this.Repository.UpdateAsync(taskItem);
            }

            await this.RepositoryContext.CommitAsync();

            return NoContent();
        }
    }
}
