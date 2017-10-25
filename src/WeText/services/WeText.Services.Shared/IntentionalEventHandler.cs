using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WeText.Services.Shared
{
    /// <summary>
    /// Represents the base class for the event handlers which handles a specified
    /// type of intent.
    /// </summary>
    /// <seealso cref="WeText.Services.Shared.IIntentionalEventHandler" />
    public abstract class IntentionalEventHandler : IIntentionalEventHandler
    {
        /// <summary>
        /// Gets a <see cref="string" /> value which indicates the intention
        /// of the event that will be handled by the current event handler.
        /// </summary>
        /// <value>
        /// The handling intention.
        /// </value>
        public abstract string HandlingIntention { get; }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public bool Handle(object message)
        {
            return HandleAsync(message).Result;
        }

        /// <summary>
        /// Handles the specified message asynchronously.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<bool> HandleAsync(object message, CancellationToken cancellationToken = default(CancellationToken))
        {
            var eventData = (IDictionary<string, object>)message;
            return await this.HandleMessageAsync(eventData, cancellationToken);
        }

        protected abstract Task<bool> HandleMessageAsync(IDictionary<string, object> eventData, CancellationToken cancellationToken = default(CancellationToken));
    }
}
