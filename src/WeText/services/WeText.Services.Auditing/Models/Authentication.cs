using Apworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeText.Services.Auditing.Models
{
    public sealed class Authentication : IAggregateRoot<Guid>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the time of authentication.
        /// </summary>
        /// <value>
        /// The time of authentication.
        /// </value>
        public DateTime TimeOfAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>
        /// The name of the account.
        /// </value>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the authentication was succeeded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if succeeded; otherwise, <c>false</c>.
        /// </value>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> value indicating the fail reason.
        /// </summary>
        /// <value>
        /// The fail reason.
        /// </value>
        public string FailReason { get; set; }
    }
}
