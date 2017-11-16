using Apworks.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeText.Services.Texting.Commands;
using System.Threading;

namespace WeText.Services.Texting.CommandHandlers
{
    public class PostTextCommandHandler : CommandHandler<PostTextCommand>
    {
        public override Task<bool> HandleAsync(PostTextCommand message, CancellationToken cancellationToken = default(CancellationToken))
        {
            // throw new NotImplementedException();
            return Task.FromResult(true);
        }
    }
}
