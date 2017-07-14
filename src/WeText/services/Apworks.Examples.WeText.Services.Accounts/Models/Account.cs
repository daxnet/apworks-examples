using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apworks.Examples.WeText.Services.Accounts.Models
{
    public class Account : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }
    }
}
