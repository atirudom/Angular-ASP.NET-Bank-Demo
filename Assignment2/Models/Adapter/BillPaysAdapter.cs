using Assignment2.CustomExceptions;
using Assignment2.Data;
using Assignment2.Models.Factory;
using Assignment2.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models.Adapter
{
    public class BillPaysAdapter
    {
        private List<BillPay> BillPays;
        private MainContext MainContext;

        public BillPaysAdapter(List<BillPay> billPays, MainContext context)
        {
            BillPays = billPays;
            MainContext = context;
        }

        public void ExecuteBillPaySchedule()
        {
            BillPays.ForEach(bill =>
            {
                var now = DateTime.UtcNow;
                now.AddSeconds(-now.Second);                // Ignore second
                now.AddMilliseconds(-now.Millisecond);      // Ignore millisecond

                // If bill schedule date is after now = set bill error
                //if (bill.ScheduleDate < now && bill.Status == BillStatus.Normal)
                //{
                //    bill.Status = BillStatus.Error;
                //    bill.StatusMessage = "The schedule time has passed but was not paid";
                //    return;
                //}

                if (bill.Status != BillStatus.Normal) return;            // Avoid error bill
                if (bill.LastPaymentTime.IsSameMinute(now)) return;    // Avoid repeated calling

                // Execute bill operation
                var billPeriod = bill.Period;
                switch (billPeriod)
                {
                    case BillPeriod.Annually:
                        if (bill.ScheduleDate.IsSameMinuteInDifferentYear(now))
                        {
                            PayNow(bill);
                        }
                        break;
                    case BillPeriod.Quarterly:
                        if (bill.ScheduleDate.IsSameMinuteInDifferentQuarter(now))
                        {
                            PayNow(bill);
                        }
                        break;
                    case BillPeriod.Monthly:
                        if (bill.ScheduleDate.IsSameMinuteInDifferentMonth(now))
                        {
                            PayNow(bill);
                        }
                        break;
                    case BillPeriod.OneOff:
                        if (bill.ScheduleDate.IsSameMinute(now))
                        {
                            bool res = PayNow(bill);
                            if (res)
                            {
                                bill.Status = BillStatus.Paid;
                            }
                        }
                        break;
                }
            });
        }

        private bool PayNow(BillPay billPay)
        {
            try
            {
                decimal amountToBeReduced = billPay.Amount;
                billPay.Account.ChangeBalance(-amountToBeReduced);
                Transaction transaction = TransactionFactory.GenerateTransaction(
                    billPay.AccountNumber,
                    null,
                    TransactionType.BillPay,
                    amountToBeReduced,
                    "Pay to Payee " + billPay.Payee.PayeeName);
                billPay.Account.Transactions.Add(transaction);
                billPay.LastPaymentTime = DateTime.UtcNow;
                billPay.StatusMessage = "Paid on " + billPay.LastPaymentTime.ToLocalTime().ToLongTimeString();
                return true;
            }
            catch (BusinessRulesException e)
            {
                billPay.Status = BillStatus.Error;
                billPay.StatusMessage = "ERROR: Bill stops working on " + DateTime.Now + " | Reason: " + e.errMsg;
                return false;
            }
        }
    }
}
