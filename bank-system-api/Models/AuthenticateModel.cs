using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bank_system_api.Models
{
    public class AuthenticateModel
    {
        [Required]
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{0}$")]
        public string AccountNumber { get; set; }
        [Required]

        public string Pin { get; set; }
    }
}
