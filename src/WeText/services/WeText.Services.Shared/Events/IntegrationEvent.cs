using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeText.Services.Shared.Events
{
    public abstract class IntegrationEvent : Event
    {
        public const string IntegrationEventMetadataKey = "IntegrationEvent";

        protected IntegrationEvent()
        {
            this.Metadata.Add(IntegrationEventMetadataKey, true);
        }
    }
}
