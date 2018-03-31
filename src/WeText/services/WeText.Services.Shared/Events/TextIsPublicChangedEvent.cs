using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeText.Services.Shared.Events
{
    public sealed class TextIsPublicChangedEvent : DomainEvent
    {
        public TextIsPublicChangedEvent(bool isPublic)
        {
            this.IsPublic = isPublic;
        }

        public bool IsPublic { get; }
    }
}
