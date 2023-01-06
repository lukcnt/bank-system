using Microsoft.EntityFrameworkCore;
using bank_system_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bank_system_api.DAL
{
    public class BankSystemDbContext : DbContext
    {
        public BankSystemDbContext(DbContextOptions<BankSystemDbContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
