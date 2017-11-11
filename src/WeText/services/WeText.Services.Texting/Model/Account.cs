using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeText.Services.Texting.Model
{
    internal class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string NickName { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
