using AdminApi.Data;
using AdminApi.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.DataManager
{
    public class BillPayManager : IDataRepository<BillPay, int>
    {
        private readonly MainContext _context;

        public BillPayManager(MainContext context)
        {
            _context = context;
        }

        public IEnumerable<BillPay> GetFromAccount(int accountNumber)
        {
            return _context.Accounts.Find(accountNumber).BillPays;
        }

        public int Add(BillPay billPay)
        {
            _context.BillPays.Add(billPay);
            _context.SaveChanges();

            return billPay.BillPayID;
        }

        public int Delete(int billPayID)
        {
            _context.BillPays.Remove(_context.BillPays.Find(billPayID));
            _context.SaveChanges();

            return billPayID;
        }

        public BillPay Get(int billPayID)
        {
            return _context.BillPays.Find(billPayID);
        }

        public IEnumerable<BillPay> GetAll()
        {
            return _context.BillPays.ToList();
        }

        public int Update(int id, BillPay billPay)
        {
            _context.Update(billPay);
            _context.SaveChanges();

            return id;
        }
    }
}
