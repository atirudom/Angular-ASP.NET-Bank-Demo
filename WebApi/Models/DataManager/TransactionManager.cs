using AdminApi.Data;
using AdminApi.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.DataManager
{
    public class TransactionManager : IDataRepository<Transaction, int>
    {
        private readonly MainContext _context;

        public TransactionManager(MainContext context)
        {
            _context = context;
        }

        public int Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return transaction.TransactionID;
        }

        public int Delete(int transactionID)
        {
            _context.Transactions.Remove(_context.Transactions.Find(transactionID));
            _context.SaveChanges();

            return transactionID;
        }

        public Transaction Get(int transactionID)
        {
            return _context.Transactions.Find(transactionID);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions.ToList();
        }

        public IEnumerable<Transaction> GetFromCustomerID(int customerID)
        {
            var allCusAccounts = _context.Customers.FindAsync(customerID).Result.Accounts;
            IEnumerable<Transaction> transactions = _context.Transactions.Where(tran => tran.Account.CustomerID == customerID);
            return transactions;
        }

        public int Update(int id, Transaction transaction)
        {
            _context.Update(transaction);
            _context.SaveChanges();

            return id;
        }
    }
}
