using AdminApi.Data;
using AdminApi.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.DataManager
{
    public class CustomerManager : IDataRepository<Customer, int>
    {

        private readonly MainContext _context;

        public CustomerManager(MainContext context)
        {
            _context = context;
        }

        public int Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer.CustomerID;
        }

        public int Delete(int customerID)
        {
            _context.Customers.Remove(_context.Customers.Find(customerID));
            _context.SaveChanges();

            return customerID;
        }

        public Customer Get(int customerID)
        {
            return _context.Customers.Find(customerID);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public int Update(int id, Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges();

            return id;
        }
    }
}
