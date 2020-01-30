using Assignment2.CustomExceptions;
using Assignment2.Models;
using Assignment2.Models.Adapter;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Controllers.Functions
{
    public class ATMMediator
    {
        public static void Deposit(Account account, decimal amount, ModelStateDictionary ModelState)
        {
            try
            {
                AccountAdapter accountAdapter = new AccountAdapter(account);
                accountAdapter.Deposit(amount);
            }
            catch (BusinessRulesException e)
            {
                ModelState.AddModelError(nameof(amount), e.errMsg);
                return;
            }
        }

        public static void Withdraw(Account account, decimal amount, ModelStateDictionary ModelState)
        {
            try
            {
                AccountAdapter accountAdapter = new AccountAdapter(account);
                accountAdapter.Withdraw(amount);
            }
            catch (BusinessRulesException e)
            {
                ModelState.AddModelError(nameof(amount), e.errMsg);
                return;
            }
        }

        public static void Transfer(Account account, Account destinationAccount, decimal amount, string comment, ModelStateDictionary ModelState)
        {
            try
            {
                AccountTransferAdapter accountTransferAdapter = new AccountTransferAdapter(account, destinationAccount);
                accountTransferAdapter.CreateTransferTransaction(amount, comment);
                accountTransferAdapter.ExecuteTransferTransaction();
            }
            catch (TransactionRuleException e)
            {
                ModelState.AddModelError("destinationAccountNumber", e.errMsg);
            }
            catch (BusinessRulesException e)
            {
                ModelState.AddModelError(nameof(amount), e.errMsg);
            }
        }
    }
}
