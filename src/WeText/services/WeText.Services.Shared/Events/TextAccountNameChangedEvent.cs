using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeText.Services.Shared.Events
{
    public sealed class TextAccountNameChangedEvent : DomainEvent
    {

        public TextAccountNameChangedEvent(string accountName)
        {
            this.AccountName = accountName;
        }

        public string AccountName { get; }
    }
}
