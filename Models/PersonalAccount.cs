using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bank_system.Models
{
    public class PersonalAccount : Account
    {
        public string Cpf { get; set; }
    }
}