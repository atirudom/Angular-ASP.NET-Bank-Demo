using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Dto
{
    public class AccountDto
    {
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public int CustomerID { get; set; }
        public decimal Balance { get; set; }
        public DateTime ModifyDate { get; set; }
        public List<BillPayDto> BillPays { get; set; }

        [JsonIgnore]
        public Account Account;

        public AccountDto(Account acc)
        {
            AccountNumber = acc.AccountNumber;
            AccountType = acc.AccountType.ToString();
            CustomerID = acc.CustomerID;
            Balance = acc.Balance;
            ModifyDate = acc.ModifyDate;

            Account = acc;
        }

        public void PopulateBillPays()
        {
            BillPays = new List<BillPayDto>();
            var billPayList = Account.BillPays.ToList();

            foreach (var billPay in billPayList)
            {
                BillPays.Add(new BillPayDto(billPay));
            };
        }
    }
}
