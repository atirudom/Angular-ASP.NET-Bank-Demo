using Assignment2.Data;
using Assignment2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2.Utils;
using Microsoft.EntityFrameworkCore;
using Assignment2.Models.Adapter;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment2.Persistence
{
    public static class MainPersistence
    {
        public static void RunBillPayPersistence(IServiceProvider service)
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);

            // Will be executed every 1 minute
            var timer = new System.Threading.Timer((e) =>
            {
                ExecuteBillPaySchedule(service);
            }, null, startTimeSpan, periodTimeSpan);
        }

        private static void ExecuteBillPaySchedule(IServiceProvider service)
        {
            var context = new MainContext(service.GetRequiredService<DbContextOptions<MainContext>>());

            BillPaysAdapter billPayAdapter = new BillPaysAdapter(context.BillPays.ToList(), context);
            billPayAdapter.ExecuteBillPaySchedule();

            context.SaveChangesAsync();
        }
    }
}
