using Apworks.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeText.Services.Texting.Commands;

namespace WeText.Services.Texting.Controllers
{
    [Route("api/[controller]")]
    public class TextsController : Controller
    {
        private readonly ICommandSender commandSender;

        public TextsController(ICommandSender commandSender)
        {
            this.commandSender = commandSender;
        }

        [HttpPost]
        public async Task<IActionResult> PostText([FromBody] dynamic text)
        {
            var accountName = (string)text.accountName;
            var isPublic = (bool)text.isPublic;
            var content = (string)text.content;

            await this.commandSender.PublishAsync(new PostTextCommand(accountName, isPublic, content));

            return Ok();
        }
    }
}
