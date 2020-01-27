using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using Microsoft.AspNetCore.Http;
using Assignment2.Attributes;
using Assignment2.Models.Adapter;
using Assignment2.CustomExceptions;
using Assignment2.Utils;
using X.PagedList;

namespace Assignment2.Controllers
{
    [AuthorizeCustomer]
    public class ATMController : Controller
    {
        private readonly MainContext _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public ATMController(MainContext context)
        {
            _context = context;
        }

        // GET: Customer
        [Route("/ATM")]
        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);

            return View(customer);
        }

        public async Task<IActionResult> Deposit(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Deposit(int id, decimal amount)
        {
            Account account = await _context.Accounts.FindAsync(id);

            if (amount < 0)
                ModelState.AddModelError(nameof(amount), "Amount must be at least 1.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }

            try
            {
                AccountAdapter accountAdapter = new AccountAdapter(account);
                accountAdapter.Deposit(amount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRulesException e)
            {
                ModelState.AddModelError(nameof(amount), e.errMsg);
                return View(account);
            }
        }

        public async Task<IActionResult> Withdraw(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Withdraw(int id, decimal amount)
        {
            Account account = await _context.Accounts.FindAsync(id);

            if (amount < 0)
                ModelState.AddModelError(nameof(amount), "Amount must be at least 1.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }

            try
            {
                AccountAdapter accountAdapter = new AccountAdapter(account);
                accountAdapter.Withdraw(amount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRulesException e)
            {
                ModelState.AddModelError(nameof(amount), e.errMsg);
                return View(account);
            }
        }

        public async Task<IActionResult> TransferBetweenAccounts() => View(await _context.Customers.FindAsync(CustomerID));

        [HttpPost]
        public async Task<IActionResult> TransferBetweenAccounts(int id, int destAccountNumber, decimal amount, string comment)
        {
            return await Transfer(id, destAccountNumber, amount, comment, nameof(TransferBetweenAccounts));
        }

        public async Task<IActionResult> TransferToOther(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> TransferToOther(int id, int destAccountNumber, decimal amount, string comment)
        {
            return await Transfer(id, destAccountNumber, amount, comment, nameof(TransferToOther));
        }

        public async Task<IActionResult> Transfer(int id, int destAccountNumber, decimal amount, string comment, string actionOrigin)
        {
            Account account = await _context.Accounts.FindAsync(id);
            Account destinationAccount = await _context.Accounts.FindAsync(destAccountNumber);

            if (amount < 0)
                ModelState.AddModelError(nameof(amount), "Amount must be at least 1.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (destinationAccount == null)
                ModelState.AddModelError(nameof(destAccountNumber), "Account with this number is not found");
            if (!ModelState.IsValid)
            {
                return ReturnWithError();
            }

            try
            {
                AccountTransferAdapter accountTransferAdapter = new AccountTransferAdapter(account, destinationAccount);
                accountTransferAdapter.CreateTransferTransaction(amount, comment);
                accountTransferAdapter.ExecuteTransferTransaction();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (TransactionRuleException e)
            {
                ModelState.AddModelError(nameof(destAccountNumber), e.errMsg);
                return ReturnWithError();
            }
            catch (BusinessRulesException e)
            {
                ModelState.AddModelError(nameof(amount), e.errMsg);
                return ReturnWithError();
            }

            IActionResult ReturnWithError() {
                ViewBag.DestAccountNumber = destAccountNumber;
                ViewBag.Amount = amount;
                ViewBag.Comment = comment;
                return View(actionOrigin, account);
            }
        }
    }
}
