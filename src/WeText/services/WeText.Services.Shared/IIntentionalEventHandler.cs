using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeText.Services.Shared
{
    public interface IIntentionalEventHandler : IEventHandler
    {
        /// <summary>
        /// Gets a <see cref="string"/> value which indicates the intention
        /// of the event that will be handled by the current event handler.
        /// </summary>
        /// <value>
        /// The handling intention.
        /// </value>
        string HandlingIntention { get; }
    }
}
