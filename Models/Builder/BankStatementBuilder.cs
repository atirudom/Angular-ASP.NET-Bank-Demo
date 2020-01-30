using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models.Builder
{
    public class BankStatementBuilder
    {
        private Customer Customer;
        private AccountType AccountType;

        private List<Transaction> ResultTransactions;
        private List<Account> MatchedAccounts;

        public BankStatementBuilder(Customer customer)
        {
            Customer = customer;
            ResultTransactions = new List<Transaction>();
        }

        public void SetAccountType(AccountType accountType)
        {
            AccountType = accountType;
        }

        internal void CreateNewBankStatement()
        {
            List<Account> accounts = Customer.Accounts;

            MatchedAccounts = accounts.Where(x => x.AccountType == AccountType).ToList();
            MatchedAccounts.ForEach(account =>
            {
                ResultTransactions.AddRange(account.GetAllTransactions());
            });
        }

        public void SortTransactionsByDateDESC()
        {
            List<Transaction> transactions = ResultTransactions;

            // Sort DateTime
            transactions.Sort((x, y) => DateTime.Compare(y.TransactionTimeUtc, x.TransactionTimeUtc));
        }

        public List<Transaction> GetBankStatementTransactions()
        {
            return ResultTransactions;
        }

        public List<Account> GetMatchedAccounts()
        {
            return MatchedAccounts;
        }
    }
}
