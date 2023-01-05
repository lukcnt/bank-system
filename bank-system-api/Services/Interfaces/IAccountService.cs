using bank_system_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bank_system_api.Services.Interfaces
{
    public interface IAccountService
    {
        Account Authenticate(string AccountNumber, string Pin);
        IEnumerable<Account> GetAllAccounts();
        Account Create(Account account, string Pin, string ConfirmPin);
        void Update(Account account, string Pin = null);
        void Delete(int Id);
        Account GetById(int Id);
        Account GetByAccountNumber(string AccountNumber);
    }
}
