using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bank_system.Models
{
    public class Account
    {
        public string Name { get; set; }
        public string Identification { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Pin { get; set; }
    }
}