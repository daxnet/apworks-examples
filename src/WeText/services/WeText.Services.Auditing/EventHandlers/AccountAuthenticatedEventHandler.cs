using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Apworks.Repositories;
using WeText.Services.Shared;

namespace WeText.Services.Auditing.EventHandlers
{
    public class AccountAuthenticatedEventHandler : IIntentionalEventHandler
    {
        private readonly IRepositoryContext repositoryContext;

        public AccountAuthenticatedEventHandler(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public string HandlingIntention => "AccountAuthenticatedEvent";

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
