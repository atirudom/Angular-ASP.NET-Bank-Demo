using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Dto
{
    public class BillPayDto
    {
        public int BillPayID { get; set; }
        public int AccountNumber { get; set; }
        public int PayeeID { get; set; }
        public decimal Amount { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string ScheduleDateStr { get; set; }
        public string Period { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public DateTime LastPaymentTime { get; set; }
        public DateTime ModifyDate { get; set; }

        [JsonIgnore]
        public BillPay BillPay;

        public BillPayDto(BillPay bill)
        {
            BillPayID = bill.BillPayID;
            AccountNumber = bill.AccountNumber;
            PayeeID = bill.PayeeID;
            Amount = bill.Amount;
            //ScheduleDate = bill.ScheduleDate;
            Period = bill.Period.ToString();
            Status = bill.Status.ToString();
            StatusMessage = bill.StatusMessage;
            LastPaymentTime = bill.LastPaymentTime;
            ModifyDate = bill.ModifyDate;
            BillPay = bill;

            DateTime newDate = DateTime.SpecifyKind(bill.ScheduleDate, DateTimeKind.Utc);
            ScheduleDate = newDate.ToLocalTime();
            ScheduleDateStr = newDate.ToLocalTime().ToString("dd/MM/yyyy hh:mm:ss tt");
        }
    }
}
