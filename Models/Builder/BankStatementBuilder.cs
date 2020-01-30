using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models.Builder
{
    public class BankStatementBuilder
    {
        private Customer Customer;
        private List<Transaction> Transactions;
        private AccountType AccountType;

        public BankStatementBuilder(Customer customer)
        {
            Customer = customer;
            Transactions = new List<Transaction>();
        }

        public void SetAccountType(AccountType accountType)
        {
            AccountType = accountType;
        }

        internal void CreateNewBankStatement()
        {
            List<Account> accounts = Customer.Accounts;
            IEnumerable<Account> matchedAccounts;

            matchedAccounts = accounts.Where(x => x.AccountType == AccountType);
            matchedAccounts.ToList().ForEach(account =>
            {
                Transactions.AddRange(account.GetAllTransactions());
            });
        }

        public void SortTransactionsByDateDESC()
        {
            List<Transaction> transactions = Transactions;

            // Sort DateTime
            transactions.Sort((x, y) => DateTime.Compare(y.TransactionTimeUtc, x.TransactionTimeUtc));
        }

        public List<Transaction> GetBankStatementTransactions()
        {
            return Transactions;
        }
    }
}
