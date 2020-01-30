using Assignment2.Models;
using Assignment2.Models.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Assignment2.ViewModels.Factory
{
    public class ViewMyStatementVMFactory
    {
        public static async Task<ViewMyStatementVM> Create(Customer customer, AccountType accountType, int page, int pageSize)
        {
            // Generate transactions for view statement
            BankStatementBuilder bankStatementBuilder = new BankStatementBuilder(customer);
            bankStatementBuilder.SetAccountType(accountType);
            BankStatementDirector bankStatementDirector = new BankStatementDirector(bankStatementBuilder);
            bankStatementDirector.ConstructStatement();

            List<Transaction> resultTransactions = bankStatementDirector.GetBankStatementTransactions();
            List<Account> resultAccounts = bankStatementDirector.GetMatchedAccounts();

            var pagedList = await resultTransactions.ToPagedListAsync((int)page, pageSize);

            ViewMyStatementVM viewModel = new ViewMyStatementVM()
            {
                Accounts = resultAccounts,
                SelectedAccountType = accountType,
                PagedListTransactions = pagedList,
            };

            return viewModel;
        }
    }
}
