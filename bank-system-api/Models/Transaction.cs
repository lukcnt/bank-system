using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace bank_system_api.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference { get; set; }
        public decimal TransactionAmount { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public bool IsSuccessful => TransactionStatus.Equals(TransactionStatus.Success);
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public string TransactionParticulars { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }


        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 17)}";
        }

    }

    public enum TransactionStatus
    {
        Failed,
        Success,
        Error
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
