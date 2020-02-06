using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Controllers.Functions
{
    public class Authentication
    {
        public static bool AuthenticateAdmin(string userID, string password)
        {

            // If userID is not found or password does not match
            if (userID == "admin" && password == "admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
