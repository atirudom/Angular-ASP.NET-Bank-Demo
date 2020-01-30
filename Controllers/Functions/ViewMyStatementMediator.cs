using Assignment2.Models;
using Assignment2.ViewModels;
using Assignment2.ViewModels.Factory;

namespace Assignment2.Controllers.Functions
{
    public class ViewMyStatementMediator
    {
        public static ViewMyStatementVM GenerateMyStatementViewModel(Customer customer, AccountType accountType, int page, int pageSize)
        {
            return ViewMyStatementVMFactory.Create(customer, accountType, page, pageSize).Result;
        }
    }
}
