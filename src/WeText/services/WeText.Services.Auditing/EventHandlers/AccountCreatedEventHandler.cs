using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using WeText.Services.Shared;

namespace WeText.Services.Auditing.EventHandlers
{
    public class AccountCreatedEventHandler : IIntentionalEventHandler
    {
        public string HandlingIntention => "AccountCreatedEvent";

        public bool Handle(object message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HandleAsync(object message, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
