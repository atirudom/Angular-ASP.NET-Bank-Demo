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
        // Moe timer variable outside to avoid session destroying for auto optimazation
        static System.Threading.Timer timer;
        public static void RunBillPayPersistence(IServiceProvider service)
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(10);

            // Will be executed every 1 minute
            timer = new System.Threading.Timer((e) =>
            {
                ExecuteBillPaySchedule(service);
                ExecuteLoginUnlockTime(service);
            }, null, startTimeSpan, periodTimeSpan);
        }

        private static void ExecuteBillPaySchedule(IServiceProvider service)
        {
            var context = new MainContext(service.GetRequiredService<DbContextOptions<MainContext>>());

            BillPaysAdapter billPayAdapter = new BillPaysAdapter(context.BillPays.ToList(), context);
            billPayAdapter.ExecuteBillPaySchedule();

            context.SaveChangesAsync();
        }

        private static void ExecuteLoginUnlockTime(IServiceProvider service)
        {
            var context = new MainContext(service.GetRequiredService<DbContextOptions<MainContext>>());

            foreach (Login login in context.Logins)
            {
                // If lockUntilTime is passed
                if(login.Status == LoginStatus.Locked && login.LockUntilTime < DateTime.UtcNow)
                {
                    login.Unlock();
                }
            }

            context.SaveChangesAsync();
        }
    }
}
