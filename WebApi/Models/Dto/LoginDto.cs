using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Dto
{
    public class LoginDto
    {
        public int CustomerID { get; set; }
        public string UserID { get; set; }
        public DateTime ModifyDate { get; set; }
        public int NumOfFailedLoginAttempt { get; set; }
        public DateTime LockUntilTime { get; set; }
        public string Status { get; set; }
        public CustomerDto Customer { get; set; }

        public LoginDto() { }

        public LoginDto(Login login) : this(login, null) { 
        }

        public LoginDto(Login login, Customer customer)
        {
            CustomerID = login.CustomerID;
            UserID = login.UserID;
            ModifyDate = login.ModifyDate;
            NumOfFailedLoginAttempt = login.NumOfFailedLoginAttempt;
            LockUntilTime = login.LockUntilTime;
            Status = login.Status.ToString();
            Customer = customer != null ? new CustomerDto(customer) : null;
        }
    }
}
