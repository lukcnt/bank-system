﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace bank_system_api.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountName { get; set; }
        public string Email { get; set; }
        public string AccountNumberGenerated { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        
        [JsonIgnore]
        public byte[] PinStoredHash { get; set; }
        [JsonIgnore]
        public byte[] PinStoredSalt { get; set; }

        Random rand = new Random();
        public Account()
        {
            AccountNumberGenerated = Convert.ToString((long) Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));
            AccountName = $"{FirstName} {LastName}";
        }

    }

    public enum AccountType
    {
        Savings,
        Current,
        Corporate,
        Government
    }
}
