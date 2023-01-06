using bank_system_api.DAL;
using bank_system_api.Models;
using bank_system_api.Services.Interfaces;
using bank_system_api.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bank_system_api.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private BankSystemDbContext _dbContext;
        ILogger<TransactionService> _logger;
        private AppSettings _settings;
        private static string _ourBankSettlementAccount;
        private readonly IAccountService _accountService;

        public TransactionService(BankSystemDbContext dbContext, ILogger<TransactionService> logger, IOptions<AppSettings> settings,
            IAccountService accountService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _settings = settings.Value;
            _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountService = accountService;
        }
        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created sucessfully!";
            response.Data = null;

            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = _dbContext.Transactions.Where(x => x.TransactionDate == date).ToList();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created sucessfully!";
            response.Data = transaction;

            return response;
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser != null) throw new ApplicationException("Invalid credentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);
                destinationAccount = _accountService.GetByAccountNumber(AccountNumber);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfull!";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
            }

            transaction.TransactionType = TransactionType.Deposit;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = @$"NEW TRANSACTION FROM SOURCE {JsonConvert.SerializeObject
                (transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject
                (transaction.TransactionDestinationAccount)} ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject
                (transaction.TransactionAmount)} TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)} TRANSACTION STATUS
                => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;
        }

        public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(FromAccount, TransactionPin);
            if (authUser != null) throw new ApplicationException("Invalid credentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(FromAccount);
                destinationAccount = _accountService.GetByAccountNumber(ToAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfull!";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
            }

            transaction.TransactionType = TransactionType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = @$"NEW TRANSACTION FROM SOURCE {JsonConvert.SerializeObject
                (transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject
                (transaction.TransactionDestinationAccount)} ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject
                (transaction.TransactionAmount)} TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)} TRANSACTION STATUS
                => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser != null) throw new ApplicationException("Invalid credentials");

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(AccountNumber);
                destinationAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfull!";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failed!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AN ERROR OCCURRED... => {ex.Message}");
            }

            transaction.TransactionType = TransactionType.Withdrawal;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.TransactionDestinationAccount = _ourBankSettlementAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = @$"NEW TRANSACTION FROM SOURCE {JsonConvert.SerializeObject
                (transaction.TransactionSourceAccount)} TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject
                (transaction.TransactionDestinationAccount)} ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject
                (transaction.TransactionAmount)} TRANSACTION TYPE => {JsonConvert.SerializeObject(transaction.TransactionType)} TRANSACTION STATUS
                => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();

            return response;
        }
    }
}
