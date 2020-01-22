using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{
    public enum TransactionType
    {
        Deposit = 'D',
        Withdraw = 'W',
        Transfer = 'T',
        ServiceCharge = 'S'
    }

    public class Transaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(1000, 9999)]
        public int TransactionID { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Range(1000,9999), Required]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }

        [ForeignKey("DestinationAccount"), Range(1000, 9999, ErrorMessage = "CustomerID must be 4 digits")]
        public int? DestinationAccountNumber { get; set; }
        public virtual Account DestinationAccount { get; set; }

        [Column(TypeName = "money"), Range(1, 99999999)]
        public decimal Amount { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        public DateTime TransactionTimeUtc { get; set; }
    }
}