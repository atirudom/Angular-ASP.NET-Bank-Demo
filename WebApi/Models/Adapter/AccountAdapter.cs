using AdminApi.CustomExceptions;
using AdminApi.Models.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Adapter
{
    public class AccountAdapter
    {
        private decimal WithdrawServiceFee = 0.1m;
        private Account Account;

        // Set account on creating with constructor
        public AccountAdapter(Account account)
        {
            this.Account = account;
        }

        internal void Deposit(decimal amount) => Deposit(amount, null);

        internal void Deposit(decimal amount, string comment)
        {
            if (amount <= 0) throw new BusinessRulesException("Amount cannot be lower than 1!");
            Account.ChangeBalance(amount);
            Transaction transaction = TransactionFactory.GenerateTransaction(Account.AccountNumber, TransactionType.Deposit, amount);
            transaction.Comment = comment;
            Account.Transactions.Add(transaction);
            Console.WriteLine("\nGenerated Transaction: \n{0}", transaction);
        }

        internal void Withdraw(decimal amount)
        {
            if (amount <= 0) throw new BusinessRulesException("Amount cannot be lower than 0!");

            // get incurred fee and add to total amount to reduce
            decimal totalAmountToReduce = 0;
            decimal serviceFee = GetIncurredFee();
            totalAmountToReduce += amount;
            totalAmountToReduce += serviceFee;

            // Perform changing balance first to check if satisfying the transferring conditions
            Account.ChangeBalance(-totalAmountToReduce);

            // Generate transaction
            Transaction transaction = TransactionFactory.GenerateTransaction(Account.AccountNumber, TransactionType.Withdraw, amount);
            Account.Transactions.Add(transaction);
            Console.WriteLine("\nGenerated Transaction: \n{0}", transaction);

            // apply incurred fee and generate service fee transaction
            if (serviceFee != 0)
            {
                Transaction feeTransaction = TransactionFactory.GenerateTransaction(Account.AccountNumber, TransactionType.ServiceCharge, serviceFee);
                Account.Transactions.Add(feeTransaction);
                Console.WriteLine("\nGenerated Transaction: \n{0}", feeTransaction);
            }
        }

        internal void DepositWithSpecTime(decimal amount, string comment, DateTime dateTime)
        {
            if (amount <= 0) throw new BusinessRulesException("Amount cannot be lower than 1!");
            Account.ChangeBalance(amount);
            Transaction transaction = TransactionFactory.GenerateTransactionWithSpecTime(Account.AccountNumber, null, TransactionType.Deposit, amount, null, dateTime);
            transaction.Comment = comment;
            Account.Transactions.Add(transaction);
            Console.WriteLine("\nGenerated Transaction: \n{0}", transaction);
        }

        internal void WithdrawWithSpecTime(decimal amount, DateTime dateTime)
        {
            if (amount <= 0) throw new BusinessRulesException("Amount cannot be lower than 0!");

            // get incurred fee and add to total amount to reduce
            decimal totalAmountToReduce = 0;
            decimal serviceFee = GetIncurredFee();
            totalAmountToReduce += amount;
            totalAmountToReduce += serviceFee;

            // Perform changing balance first to check if satisfying the transferring conditions
            Account.ChangeBalance(-totalAmountToReduce);

            // Generate transaction
            Transaction transaction = TransactionFactory.GenerateTransactionWithSpecTime(Account.AccountNumber, null, TransactionType.Withdraw, amount, null, dateTime);
            Account.Transactions.Add(transaction);
            Console.WriteLine("\nGenerated Transaction: \n{0}", transaction);

            // apply incurred fee and generate service fee transaction
            if (serviceFee != 0)
            {
                Transaction feeTransaction = TransactionFactory.GenerateTransactionWithSpecTime(Account.AccountNumber, null, TransactionType.ServiceCharge, serviceFee, null, dateTime);
                Account.Transactions.Add(feeTransaction);
                Console.WriteLine("\nGenerated Transaction: \n{0}", feeTransaction);
            }
        }

        private decimal GetIncurredFee()
        {
            decimal incurredFee = 0;
            List<Transaction> accountTransactions = Account.Transactions;
            int allowedNumOfFreeTransactions = 4;
            foreach (Transaction t in accountTransactions)
            {
                if (allowedNumOfFreeTransactions <= 0) break;
                if (t.TransactionType == TransactionType.Withdraw || t.TransactionType == TransactionType.Transfer && t.AccountNumber == Account.AccountNumber)
                    allowedNumOfFreeTransactions -= 1;
            }
            if (allowedNumOfFreeTransactions <= 0) incurredFee += WithdrawServiceFee;
            return incurredFee;
        }
    }
}
