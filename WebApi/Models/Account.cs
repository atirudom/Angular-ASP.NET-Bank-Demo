using AdminApi.CustomExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminApi.Models
{
    public enum AccountType
    {
        Checking = 'C',
        Saving = 'S'
    }

    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(1000, 9999)]
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

        [InverseProperty("Account")]
        public virtual List<Transaction> Transactions { get; set; }

        [InverseProperty("DestinationAccount")]
        public virtual List<Transaction> ReceivingTransactions { get; set; }

        public virtual List<BillPay> BillPays { get; set; }

        public void ChangeBalance(decimal adjustedAmount)
        {
            decimal newBalance = Balance + adjustedAmount;
            if (newBalance < 0) throw new BusinessRulesException("Not enough balance!");
            if (AccountType == AccountType.Saving && newBalance < 0) throw new BusinessRulesException("Account Savings type balance cannot be lower than 0");
            if (AccountType == AccountType.Checking && newBalance < 200) throw new BusinessRulesException("Account Checking type balance cannot be lower than 200");
            Balance = newBalance;
        }

        public List<Transaction> GetAllTransactions()
        {
            List<Transaction> resultTransactions = new List<Transaction>();
            resultTransactions.AddRange(Transactions);
            resultTransactions.AddRange(ReceivingTransactions);

            return resultTransactions;
        }
    }
}