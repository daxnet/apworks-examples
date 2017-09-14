using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeText.Services.Shared.Events;

namespace WeText.Services.Accounts.Events
{
    /// <summary>
    /// Represents the event that occurs when an account has been authenticated.
    /// </summary>
    public sealed class AccountAuthenticatedEvent : IntegrationEvent
    {
        public AccountAuthenticatedEvent() { }

        public AccountAuthenticatedEvent(string accountName, bool isSuccess, string reason = "")
        {
            this.AccountName = accountName;
            this.IsSuccess = isSuccess;
            this.Reason = reason;
        }

        /// <summary>
        /// Gets or sets the name of the account that has been authenticated.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value which indicates if the authentication
        /// has succeeded.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="string"/> value, which describes the reason when the authentication has
        /// failed.
        /// </summary>
        public string Reason { get; set; }
    }
}
