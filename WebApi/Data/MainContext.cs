using Microsoft.EntityFrameworkCore;
using Assignment2.Models;

namespace Assignment2.Data
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BillPay> BillPays { get; set; }
        public DbSet<Payee> Payees { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Login>().HasCheckConstraint("CH_Login_UserID", "len(UserID) = 8").
                HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 64");
            builder.Entity<Account>().HasCheckConstraint("CH_Account_Balance", "Balance >= 0");
            builder.Entity<Transaction>().
                HasOne(x => x.Account).WithMany(x => x.Transactions).HasForeignKey(x => x.AccountNumber);
            builder.Entity<Transaction>().HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");

            // Implement autoincrement number starting from 1000 (4 digits)
            //builder.HasSequence<int>("AccountNumberSequence", schema: "shared")
            //    .StartsAt(1000)
            //    .IncrementsBy(1);
            builder.Entity<Account>()
                .Property(a => a.AccountNumber)
                .HasDefaultValueSql("NEXT VALUE FOR shared.AccountNumberSequence");

            //builder.HasSequence<int>("BillPaySequence", schema: "shared")
            //    .StartsAt(1000)
            //    .IncrementsBy(1);
            builder.Entity<BillPay>()
                .Property(b => b.BillPayID)
                .HasDefaultValueSql("NEXT VALUE FOR shared.BillPaySequence");

            //builder.HasSequence<int>("PayeeSequence", schema: "shared")
            //    .StartsAt(1000)
            //    .IncrementsBy(1);
            builder.Entity<Payee>()
                .Property(p => p.PayeeID)
                .HasDefaultValueSql("NEXT VALUE FOR shared.PayeeSequence");

            //builder.HasSequence<int>("TransactionSequence", schema: "shared")
            //    .StartsAt(1000)
            //    .IncrementsBy(1);
            builder.Entity<Transaction>()
                .Property(t => t.TransactionID)
                .HasDefaultValueSql("NEXT VALUE FOR shared.TransactionSequence");
        }
    }
}
