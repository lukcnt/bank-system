using bank_system_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bank_system_api.Services.Interfaces
{
    public interface ITransactionService
    {
        Response CreateNewTransaction(Transaction transaction);
        Response FindTransactionByDate(DateTime date);
        Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin);
        Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);
    }
}
