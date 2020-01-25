using Assignment2.CustomExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{
    public enum AccountType
    {
        Checking = 'C',
        Saving = 'S'
    }

    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(1, 9999)]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Required, Display(Name = "Type"), StringLength(1)]
        public AccountType AccountType { get; set; }

        [Required, Range(1000, 9999)]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [Required]
        public DateTime ModifyDate { get; set; }

        public virtual List<Transaction> Transactions { get; set; }

        public void ChangeBalance(decimal adjustedAmount)
        {
            decimal newBalance = Balance + adjustedAmount;
            if (newBalance < 0) throw new BusinessRulesException("Not enough balance!");
            if (AccountType == AccountType.Saving && newBalance < 0) throw new BusinessRulesException("Account Savings type balance cannot be lower than 0");
            if (AccountType == AccountType.Checking && newBalance < 200) throw new BusinessRulesException("Account Checking type balance cannot be lower than 200");
            Balance = newBalance;
        }
    }
}