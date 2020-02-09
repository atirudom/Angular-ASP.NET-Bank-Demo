using AdminApi.CustomExceptions;
using AdminApi.Models.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Adapter
{
    // AccountAdapter class contains a set of transaction services that occur between two accounts
    // which currently contain only Transferring between personal accounts. 
    // The class is designed based on Adapter pattern.
    public class AccountTransferAdapter
    {
        private decimal TransferServiceFee = 0.2m;
        private Account RootAccount;
        private Account DestinationAccount;
        private Transaction Transaction;

        // Set root account and destination account on creating with constructor
        public AccountTransferAdapter(Account rootAccount, Account destinationAccount)
        {
            if (rootAccount.AccountNumber == destinationAccount.AccountNumber)
                throw new TransactionRuleException("Cannot transfer between same accounts");
            RootAccount = rootAccount;
            DestinationAccount = destinationAccount;
        }

        // Transfer between accounts set in this adapter
        private void Transfer(decimal amount, DateTime dateTime)
        {
            if (amount <= 0) throw new BusinessRulesException("Amount cannot be lower than 0!");

            // get incurred fee and add to total amount to reduce
            decimal serviceFee = GetIncurredFee();
            decimal totalAmountToReduce = 0;
            totalAmountToReduce += amount;
            totalAmountToReduce += serviceFee;

            // Perform changing balance first to check if satisfying the transferring conditions
            RootAccount.ChangeBalance(-totalAmountToReduce);
            DestinationAccount.ChangeBalance(amount);

            // apply incurred fee and generate its transaction to root account
            if (serviceFee != 0)
            {
                Transaction feeTransaction = TransactionFactory.GenerateTransactionWithSpecTime(RootAccount.AccountNumber, null, TransactionType.ServiceCharge, serviceFee, null, dateTime);
                RootAccount.Transactions.Add(feeTransaction);
            }

            // Add transaction
            RootAccount.Transactions.Add(Transaction);

            // Remove to avoid Save changes glitch where account ant Destination becomes the same
            DestinationAccount.ReceivingTransactions.Add(Transaction);

            Transaction.DestinationAccount = DestinationAccount;
            Transaction.Account = RootAccount;
        }

        // This method return the amount of transferring service fees incurred in the action
        private decimal GetIncurredFee()
        {
            decimal incurredFee = 0;
            List<Transaction> accountTransactions = RootAccount.Transactions;
            int allowedNumOfFreeTransactions = 4;
            foreach (Transaction t in accountTransactions)
            {
                if (allowedNumOfFreeTransactions <= 0) break;
                if (t.TransactionType == TransactionType.Withdraw || t.TransactionType == TransactionType.Transfer && t.AccountNumber == RootAccount.AccountNumber)
                    allowedNumOfFreeTransactions -= 1;
            }
            if (allowedNumOfFreeTransactions <= 0) incurredFee += TransferServiceFee;
            return incurredFee;
        }

        internal void CreateTransferTransaction(decimal amount, string comment)
        {
            Transaction = TransactionFactory.GenerateTransaction(RootAccount.AccountNumber, DestinationAccount.AccountNumber, TransactionType.Transfer, amount, comment);
        }

        internal void TransferTransactionWithSpecTime(decimal amount, string comment, DateTime dateTime)
        {
            Transaction = TransactionFactory.GenerateTransactionWithSpecTime(RootAccount.AccountNumber, DestinationAccount.AccountNumber, TransactionType.Transfer, amount, comment, dateTime);
        }

        internal void ExecuteTransferTransaction()
        {
            Transfer(Transaction.Amount, Transaction.TransactionTimeUtc);
        }
    }
}
