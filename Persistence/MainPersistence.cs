using Assignment2.Data;
using Assignment2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2.Utils;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Persistence
{
    public static class MainPersistence
    {
        public static void RunBillPayPersistence(MainContext mainContext)
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(10);

            // Will be executed every 1 minute
            var timer = new System.Threading.Timer((e) =>
            {
                ExecuteBillPaySchedule(mainContext.BillPays);
                //mainContext.SaveChangesAsync();
            }, null, startTimeSpan, periodTimeSpan);
        }

        private static void ExecuteBillPaySchedule(DbSet<BillPay> billPays)
        {
            billPays.ToList().ForEach(bill =>
            {
                var billPeriod = bill.Period;
                switch (billPeriod)
                {
                    case BillPeriod.Annually:
                        if (bill.ScheduleDate.IsSameMinuteInDifferentYear(DateTime.Now))
                            break;
                        break;
                    case BillPeriod.Quarterly:
                        if (bill.ScheduleDate.IsSameMinuteInDifferentQuarter(DateTime.Now))
                            break;
                        break;
                }
            });
        }
    }
}
