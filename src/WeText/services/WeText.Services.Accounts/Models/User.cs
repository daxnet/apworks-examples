using Apworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeText.Services.Accounts.Models
{
    /// <summary>
    /// Represents a user in the WeText application.
    /// </summary>
    public class User : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime DateRegistered { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string AvatarBackgroundColor { get; set; }

        public override string ToString() => this.UserName;
    }
}
