using Microsoft.Extensions.Logging;
using bank_system_api.DAL;
using bank_system_api.Models;
using bank_system_api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bank_system_api.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private BankSystemDbContext _dbContext;

        public AccountService(BankSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Account Authenticate(string AccountNumber, string Pin)
        {
            var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated== AccountNumber).SingleOrDefault();
            if (account == null)
                return null;
            
            if (!VerifyPinHash(Pin, account.PinStoredHash, account.PinStoredSalt))
                return null;

            return account;
        }

        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException("Pin");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i])
                        return false;
                }
            }
            return true;
        }

        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            if (_dbContext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("An account already exist with this email");
            if (!Pin.Equals(ConfirmPin)) throw new ArgumentException("Pins do not match", "Pin");

            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);

            account.PinStoredHash = pinHash;
            account.PinStoredSalt = pinSalt;

            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();

            return account;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public void Delete(int Id)
        {
            var account = _dbContext.Accounts.Find(Id);
            if (account != null) 
            {
                _dbContext.Accounts.Remove(account);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _dbContext.Accounts.ToList();
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null)
                return null;

            return account;
        }

        public Account GetById(int Id)
        {
            var account = _dbContext.Accounts.Where(x => x.Id == Id).FirstOrDefault();
            if (account == null)
                return null;

            return account;
        }

        public void Update(Account account, string Pin = null)
        {
            var accountToBeUpdated = _dbContext.Accounts.Where(x => x.Email == account.Email).SingleOrDefault();
            if (accountToBeUpdated == null) throw new ApplicationException("Account does not exist");

            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                if (_dbContext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException($"This Email {account.Email} already exist");

                accountToBeUpdated.Email = account.Email;
            }

            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                if (_dbContext.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber)) throw new ApplicationException($"This Phone number {account.PhoneNumber} already exist");

                accountToBeUpdated.PhoneNumber = account.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);

                accountToBeUpdated.PinStoredHash = pinHash;
                accountToBeUpdated.PinStoredSalt = pinSalt;                
            }
            accountToBeUpdated.DateLastUpdated = DateTime.Now;

            _dbContext.Accounts.Update(accountToBeUpdated);
            _dbContext.SaveChanges();
        }
    }
}
