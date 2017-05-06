using Apworks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apworks.Examples.CustomerService.Model
{
    public class Customer : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Address ContactAddress { get; set; }
    }
}
