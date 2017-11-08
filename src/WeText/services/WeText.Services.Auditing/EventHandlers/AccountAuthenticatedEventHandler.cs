using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Apworks.Repositories;
using WeText.Services.Auditing.Models;
using WeText.Services.Shared.Events;

namespace WeText.Services.Auditing.EventHandlers
{
    /// <summary>
    /// Represents the event handler that handles the AccountAuthenticatedEvent.
    /// </summary>
    public class AccountAuthenticatedEventHandler : Apworks.Events.EventHandler<AccountAuthenticatedEvent>
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
        /// Handles the asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<bool> HandleAsync(AccountAuthenticatedEvent message, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var accountName = message.AccountName;
                var timestamp = message.Timestamp;
                var reason = message.Reason;
                var succeeded = message.IsSuccess;

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
