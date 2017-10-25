using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Apworks.Repositories;
using WeText.Services.Shared;
using WeText.Services.Auditing.Models;

namespace WeText.Services.Auditing.EventHandlers
{
    /// <summary>
    /// Represents the event handler that handles the AccountAuthenticatedEvent.
    /// </summary>
    /// <seealso cref="WeText.Services.Shared.IntentionalEventHandler" />
    public class AccountAuthenticatedEventHandler : IntentionalEventHandler
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, Authentication> authenticationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAuthenticatedEventHandler"/> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        public AccountAuthenticatedEventHandler(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
            this.authenticationRepository = this.repositoryContext.GetRepository<Guid, Authentication>();
        }

        /// <summary>
        /// Gets a <see cref="T:System.String" /> value which indicates the intention
        /// of the event that will be handled by the current event handler.
        /// </summary>
        /// <value>
        /// The handling intention.
        /// </value>
        public override string HandlingIntention => "AccountAuthenticatedEvent";

        /// <summary>
        /// Handles the message asynchronously.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task<bool> HandleMessageAsync(IDictionary<string, object> eventData, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var accountName = (string)eventData["AccountName"];
                var timestamp = (DateTime)eventData["Timestamp"];
                var reason = (string)eventData["Reason"];
                var succeeded = (bool)eventData["IsSuccess"];

                var authentication = new Authentication
                {
                    AccountName = accountName,
                    Succeeded = succeeded,
                    TimeOfAuthentication = timestamp,
                    FailReason = reason
                };

                await this.authenticationRepository.AddAsync(authentication, cancellationToken);
                await this.repositoryContext.CommitAsync(cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
