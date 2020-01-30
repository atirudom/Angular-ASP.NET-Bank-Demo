using Assignment2.Models;
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
                    PostCode = "3000",
                    Phone = "0351548454"
                },
                new Customer
                {
                    CustomerID = 2200,
                    Name = "Rodney Cocker",
                    Address = "456 Real Road",
                    City = "Melbourne",
                    PostCode = "3005",
                    Phone = "0351548454"
                },
                new Customer
                {
                    CustomerID = 2300,
                    Name = "Shekhar Kalra",
                    Phone = "0351548454"
                });

            context.Logins.AddRange(
                new Login
                {
                    UserID = "12345678",
                    CustomerID = 2100,
                    PasswordHash = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2",
                    ModifyDate = now
                },
                new Login
                {
                    UserID = "38074569",
                    CustomerID = 2200,
                    PasswordHash = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04",
                    ModifyDate = now
                },
                new Login
                {
                    UserID = "17963428",
                    CustomerID = 2300,
                    PasswordHash = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE",
                    ModifyDate = now
                });

            context.Accounts.AddRange(
                new Account
                {
                    AccountType = AccountType.Saving,
                    CustomerID = 2100,
                    Balance = 100,
                    ModifyDate = now
                },
                new Account
                {
                    AccountType = AccountType.Checking,
                    CustomerID = 2100,
                    Balance = 500,
                    ModifyDate = now
                },
                new Account
                {
                    AccountType = AccountType.Saving,
                    CustomerID = 2200,
                    Balance = 500.95m,
                    ModifyDate = now
                },
                new Account
                {
                    AccountType = AccountType.Checking,
                    CustomerID = 2300,
                    Balance = 1250.50m,
                    ModifyDate = now
                });

            context.Payees.AddRange(
                new Payee
                {
                    PayeeName = "Atirudom",
                    Address = "400 King Street",
                    City = "Melbourne",
                    State = "VIC",
                    PostCode = "3020",
                    Phone = "(61) - 29374612"
                },
                new Payee
                {
                    PayeeName = "Telstra",
                    Address = "402 Test Street",
                    City = "Sydney",
                    State = "NSW",
                    PostCode = "3000",
                    Phone = "(61) - 12345678"
                }
            );

            context.SaveChanges();
        }
    }
}
