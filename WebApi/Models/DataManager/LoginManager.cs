using AdminApi.Data;
using AdminApi.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.DataManager
{
    public class LoginManager : IDataRepository<Login, int>
    {
        private readonly MainContext _context;

        public LoginManager(MainContext context)
        {
            _context = context;
        }

        public int Add(Login login)
        {
            _context.Logins.Add(login);
            _context.SaveChanges();

            return login.CustomerID;
        }

        public int Delete(int loginID)
        {
            _context.Logins.Remove(_context.Logins.Find(loginID));
            _context.SaveChanges();

            return loginID;
        }

        public Login Get(int loginID)
        {
            return _context.Logins.Find(loginID);
        }

        public IEnumerable<Login> GetAll()
        {
            return _context.Logins.ToList();
        }

        public int Update(int id, Login login)
        {
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }
    }
}
