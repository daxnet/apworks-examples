using Apworks;
using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeText.Services.Shared.Events;

namespace WeText.Services.Texting.EventHandlers
{
    public class TextContentChangedEventHandler : Apworks.Events.EventHandler<TextContentChangedEvent>
    {
        public override Task<bool> HandleAsync(TextContentChangedEvent message, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }
    }
}
