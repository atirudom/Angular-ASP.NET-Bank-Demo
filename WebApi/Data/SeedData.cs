using AdminApi.Models;
using AdminApi.Models.Adapter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Data
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
                    Status = LoginStatus.Normal
                },
                new Login
                {
                    UserID = "38074569",
                    CustomerID = 2200,
                    PasswordHash = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04",
                    ModifyDate = now,
                    Status = LoginStatus.Normal
                },
                new Login
                {
                    UserID = "17963428",
                    CustomerID = 2300,
                    PasswordHash = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE",
                    ModifyDate = now,
                    Status = LoginStatus.Normal
                });

            context.Accounts.AddRange(
                new Account
                {
                    AccountType = AccountType.Saving,
                    CustomerID = 2100,
                    Balance = 90000,
                    ModifyDate = now
                },
                new Account
                {
                    AccountType = AccountType.Checking,
                    CustomerID = 2100,
                    Balance = 90000,
                    ModifyDate = now
                },
                new Account
                {
                    AccountType = AccountType.Saving,
                    CustomerID = 2200,
                    Balance = 90000,
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
            RandomTransactions(serviceProvider);
        }

        private static void RandomTransactions(IServiceProvider serviceProvider)
        {
            var context = new MainContext(serviceProvider.GetRequiredService<DbContextOptions<MainContext>>());
            List<Account> accounts = context.Accounts.ToListAsync().Result;
            int fallbackDays = 30;
            var date = DateTime.Today.AddDays(-fallbackDays);

            //for each day
            for (var i = 0; i < fallbackDays; i++, date = date.AddDays(1))
            {
                int numOfTransactions = 4;
                foreach (TransactionType type in Enum.GetValues(typeof(TransactionType)))
                {
                    RandomTransactionInType(accounts, type, date, numOfTransactions);
                    context.SaveChanges();
                }
            }
        }

        private static void RandomTransactionInType(List<Account> accounts, TransactionType type, DateTime dateTime, int numOfTransactions)
        {
            var random = new Random();

            // for each account
            foreach (var acc in accounts)
            {
                AccountAdapter accountAdapter = new AccountAdapter(acc);
                switch (type)
                {
                    case TransactionType.Deposit:
                        RandomDeposit();
                        break;
                    case TransactionType.Withdraw:
                        RandomWithdraw();
                        break;
                    case TransactionType.Transfer:
                        RandomTransfer();
                        break;
                }

                void RandomDeposit()
                {
                    for (int i = 0; i < numOfTransactions; i++)
                    {
                        accountAdapter.DepositWithSpecTime(random.Next(20, 1000), null, dateTime);
                    }
                }
                void RandomWithdraw()
                {
                    for (int i = 0; i < numOfTransactions; i++)
                    {
                        accountAdapter.WithdrawWithSpecTime(random.Next(20, 500), dateTime);
                    }
                }
                void RandomTransfer()
                {
                    for (int i = 0; i < numOfTransactions; i++)
                    {
                        try
                        {
                            AccountTransferAdapter accTransferAdapter =
                            new AccountTransferAdapter(acc, accounts[random.Next(0, accounts.Count)]);
                            accTransferAdapter.TransferTransactionWithSpecTime(random.Next(10, 800), null, dateTime);
                            accTransferAdapter.ExecuteTransferTransaction();
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
        }

        private static void InitTransactions(IServiceProvider serviceProvider)
        {
            var context = new MainContext(serviceProvider.GetRequiredService<DbContextOptions<MainContext>>());

            // Initial Deposit

            string comment = "";
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
