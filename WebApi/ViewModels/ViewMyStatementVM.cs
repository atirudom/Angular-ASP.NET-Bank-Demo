using AdminApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace AdminApi.ViewModels
{
    public class ViewMyStatementVM
    {
        public List<Account> Accounts { get; set; }
        public AccountType SelectedAccountType { get; set; }
        public IEnumerable<SelectListItem> AccountTypesSelectList { get; set; } =
            new SelectListItem[]
                {
                     new SelectListItem() { Text = "Saving", Value = "Saving" },
                     new SelectListItem() { Text = "Checking", Value = "Checking" }
                };
        public IPagedList<Transaction> PagedListTransactions { get; set; }
    }
}
