using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Dto
{
    public class CustomerDto
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string TFN { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public LoginDto Login { get; set; }
        public List<AccountDto> Accounts { get; set; }

        [JsonIgnore]
        public Customer Customer;

        public CustomerDto() { }

        public CustomerDto(Customer cus) : this(cus, null) { }

        public CustomerDto(Customer cus, Login login)
        {
            CustomerID = cus.CustomerID;
            Name = cus.Name;
            TFN = cus.TFN;
            Address = cus.Address;
            City = cus.City;
            State = cus.State.ToString();
            PostCode = cus.PostCode;
            Phone = cus.Phone;
            Login = login != null ? new LoginDto(login) : null;

            Customer = cus;
        }

        internal void PopulateAccounts()
        {
            Accounts = new List<AccountDto>();
            var accountList = Customer.Accounts.ToList();

            foreach (var acc in accountList)
            {
                Accounts.Add(new AccountDto(acc));
            };
        }

        internal Customer GenerateModel()
        {
            Customer = new Customer()
            {
                CustomerID = CustomerID,
                Name = Name,
                TFN = TFN,
                Address = Address,
                City = City,
                State = (AustralianState)Enum.Parse(typeof(AustralianState), State),
                PostCode = PostCode,
                Phone = Phone,
            };
            return Customer;
        }
    }
}
