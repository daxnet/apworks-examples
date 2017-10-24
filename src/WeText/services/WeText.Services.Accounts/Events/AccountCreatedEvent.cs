using Apworks.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeText.Services.Accounts.Events
{
    public class AccountCreatedEvent : Event
    {
        public AccountCreatedEvent() { }

        public AccountCreatedEvent(string accountName, string displayName, string email)
        {
            this.AccountName = accountName;
            this.DisplayName = displayName;
            this.Email = email;
        }

        public string AccountName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }
    }
}
