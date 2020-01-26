using Assignment2.CustomExceptions;
using Assignment2.Models.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models.Adapter
{
    // AccountAdapter class contains a set of transaction services that occur between two accounts
    // which currently contain only Transferring between personal accounts. 
    // The class is designed based on Adapter pattern.
    public class AccountTransferAdapter
    {
        private decimal TransferServiceFee = 0.2m;
        private Account RootAccount;
        private Account DestinationAccount;

        // Set root account and destination account on creating with constructor
        public AccountTransferAdapter(Account rootAccount, Account destinationAccount)
        {
            if (rootAccount.AccountNumber == destinationAccount.AccountNumber)
                throw new TransactionRuleException("Cannot transfer between same accounts");
            RootAccount = rootAccount;
            DestinationAccount = destinationAccount;
        }

        // Transfer between accounts set in this adapter
        internal void Transfer(decimal amount)
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
                Transaction feeTransaction = TransactionFactory.GenerateTransaction(RootAccount.AccountNumber, TransactionType.ServiceCharge, serviceFee);
                RootAccount.Transactions.Add(feeTransaction);
            }

            // Generate transaction
            Transaction transaction = TransactionFactory.GenerateTransaction(RootAccount.AccountNumber, DestinationAccount.AccountNumber, TransactionType.Transfer, amount);

            // Add transaction
            RootAccount.Transactions.Add(transaction);
            DestinationAccount.Transactions.Add(transaction);
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
    }
}
