using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeText.Services.Shared.Events
{
    public sealed class TextContentChangedEvent : DomainEvent
    {
        public TextContentChangedEvent(string content)
        {
            this.Content = content;
        }

        public string Content { get; }
    }
}
