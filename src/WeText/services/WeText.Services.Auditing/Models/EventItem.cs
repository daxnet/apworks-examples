using Apworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeText.Services.Auditing.Models
{
    public sealed class EventItem : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public string Intent { get; set; }

        public bool IsIntegration { get; set; }

        public DateTime Timestamp { get; set; }

        public string Payload { get; set; }
    }
}
