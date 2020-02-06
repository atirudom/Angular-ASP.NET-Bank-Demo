using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Factory
{
    public static class TransactionFactory
    {
        // Generate transaction with one responsible account
        public static Transaction GenerateTransaction(int accountNumber, TransactionType type, decimal amount)
            => GenerateTransaction(accountNumber, null, type, amount);

        // Generate transaction between 2 accounts
        public static Transaction GenerateTransaction(int accountNumber, int? destinationAccountNumber, TransactionType type, decimal amount)
        {
            return GenerateTransaction(accountNumber, destinationAccountNumber, type, amount, "");
        }

        // Generate transaction between 2 accounts with comments
        public static Transaction GenerateTransaction(int accountNumber, int? destinationAccountNumber, TransactionType type, decimal amount, string comment)
        {
            DateTime currentDateTime = DateTime.UtcNow;
            Transaction transaction = new Transaction()
            {
                AccountNumber = accountNumber,
                TransactionType = type,
                DestinationAccountNumber = destinationAccountNumber,
                Amount = amount,
                Comment = comment,
                TransactionTimeUtc = currentDateTime
            };
            return transaction;
        }
    }
}
