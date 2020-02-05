using Assignment2.Models;
using Assignment2.Models.Adapter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new MainContext(serviceProvider.GetRequiredService<DbContextOptions<MainContext>>());
            // Look for customers.
            if (context.Customers.Any())
                return; // DB has already been seeded.

            //const string dateFormat = "dd/MM/yyyy hh:mm:ss tt";
            DateTime now = DateTime.UtcNow;

            context.Customers.AddRange(
                new Customer
                {
                    CustomerID = 2100,
                    Name = "Matthew Bolger",
                    Address = "123 Fake Street",
                    City = "Melbourne",
                    State = AustralianState.VIC,
                    PostCode = "3000",
                    Phone = "(61)- 5154 8454"
                },
                new Customer
                {
                    CustomerID = 2200,
                    Name = "Rodney Cocker",
                    Address = "456 Real Road",
                    City = "Melbourne",
                    State = AustralianState.VIC,
                    PostCode = "3005",
                    Phone = "(61)- 2134 8454"
                },
                new Customer
                {
                    CustomerID = 2300,
                    Name = "Shekhar Kalra",
                    Phone = "(61)- 5154 5245"
                });

            context.Logins.AddRange(
                new Login
                {
                    UserID = "12345678",
                    CustomerID = 2100,
                    PasswordHash = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2",
                    ModifyDate = now,
                    NumOfFailedLoginAttempt = 0,
                    Status = LoginStatus.Normal
                },
                new Login
                {
                    UserID = "38074569",
                    CustomerID = 2200,
                    PasswordHash = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04",
                    ModifyDate = now,
                    NumOfFailedLoginAttempt = 0,
                    Status = LoginStatus.Normal
                },
                new Login
                {
                    UserID = "17963428",
                    CustomerID = 2300,
                    PasswordHash = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE",
                    ModifyDate = now,
                    NumOfFailedLoginAttempt = 0,
                    Status = LoginStatus.Normal
                });

            context.Accounts.AddRange(
                new Account
                {
                    AccountType = AccountType.Saving,
                    CustomerID = 2100,
                    Balance = 0,
                    ModifyDate = now
                },
                new Account
                {
                    AccountType = AccountType.Checking,
                    CustomerID = 2100,
                    Balance = 0,
                    ModifyDate = now
                },
                new Account
                {
                    AccountType = AccountType.Saving,
                    CustomerID = 2200,
                    Balance = 0,
                    ModifyDate = now
                },
                new Account
                {
                    AccountType = AccountType.Checking,
                    CustomerID = 2300,
                    Balance = 0,
                    ModifyDate = now
                });

            context.Payees.AddRange(
                new Payee
                {
                    PayeeName = "Atirudom",
                    Address = "400 King Street",
                    City = "Melbourne",
                    State = AustralianState.VIC,
                    PostCode = "3020",
                    Phone = "(61)- 2937 4612"
                },
                new Payee
                {
                    PayeeName = "Telstra",
                    Address = "402 Test Street",
                    City = "Sydney",
                    State = AustralianState.NSW,
                    PostCode = "3000",
                    Phone = "(61)- 5154 1233"
                }
            );
            context.SaveChanges();
            InitTransactions(serviceProvider);
        }

        private static void InitTransactions(IServiceProvider serviceProvider)
        {
            var context = new MainContext(serviceProvider.GetRequiredService<DbContextOptions<MainContext>>());

            // Initial Deposit

            string comment = "Initial deposit";
            List<Account> accounts = context.Accounts.ToListAsync().Result;
            AccountAdapter accountAdapter = new AccountAdapter(accounts[0]);
            accountAdapter.Deposit(100, comment);
            accountAdapter = new AccountAdapter(accounts[1]);
            accountAdapter.Deposit(500, comment);
            accountAdapter = new AccountAdapter(accounts[2]);
            accountAdapter.Deposit(500.95m, comment);
            accountAdapter = new AccountAdapter(accounts[3]);
            accountAdapter.Deposit(1000, comment);

            context.SaveChanges();
        }
    }
}
