using Apworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeText.Services.Texting.Model
{
    public class Text : AggregateRootWithEventSourcing<Guid>
    {
        public Text(Guid id) : base(id)
        {
        }
    }
}
