using System;
using System.ComponentModel.DataAnnotations;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Create the Transaction Object.
 */

namespace wdt_Assignment1_s3757573
{
    public class Transactions
    {
        [Required]
        public DateTime TransactionTimeUtc { get; set; }

        [Required]
        public int TransactionID { get; set; }

        [Required]
        public int AccountNumber { get; set; }

        [Required]
        public string TransactionType { get; set; }
        public int? DestinationAccountNumber { get; set; }

        [StringLength(250)]
        public string Comment { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public Transactions(){}

        public Transactions(int AccountNumber, int DestinationAccountNumber, string Comment, decimal Amount,string TransactionType, DateTime TransactionTimeUtc)
        {
            this.AccountNumber = AccountNumber;
            this.DestinationAccountNumber = DestinationAccountNumber;
            this.Amount = Amount;
            this.Comment = Comment;
            this.TransactionType = TransactionType;
            this.TransactionTimeUtc = TransactionTimeUtc;
        }
    }
}
