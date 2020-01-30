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
using Assignment2.Controllers.Functions;

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
                ModelState.AddModelError(nameof(amount), "Amount must not be lower than 0.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }

            ATMMediator.Deposit(account, amount, ModelState);
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Withdraw(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Withdraw(int id, decimal amount)
        {
            Account account = await _context.Accounts.FindAsync(id);

            if (amount < 0)
                ModelState.AddModelError(nameof(amount), "Amount must not be lower than 0.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (!ModelState.IsValid)
            {
                ViewBag.Amount = amount;
                return View(account);
            }

            ATMMediator.Withdraw(account, amount, ModelState);
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> TransferToOther(int id) => View(await _context.Accounts.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> TransferToOther(int id, int destinationAccountNumber, decimal amount, string comment)
        {
            return await Transfer(id, destinationAccountNumber, amount, comment, nameof(TransferToOther));
        }

        // Transfer method. TODO: Should be separated
        // actionOrigin parameter is used in case if there is another kind of transfer in the future. Ex: transfer between accounts
        public async Task<IActionResult> Transfer(int id, int destinationAccountNumber, decimal amount, string comment, string actionOrigin)
        {
            Account account = await _context.Accounts.FindAsync(id);
            Account destinationAccount = await _context.Accounts.FindAsync(destinationAccountNumber);

            // input validation
            if (amount < 0)
                ModelState.AddModelError(nameof(amount), "Amount must not be lower than 0.");
            if (amount.HasMoreThanTwoDecimalPlaces())
                ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (destinationAccount == null)
                ModelState.AddModelError(nameof(destinationAccountNumber), "Account with this number is not found");
            if (!ModelState.IsValid)
            {
                return ReturnWithError();
            }

            // operation execution
            ATMMediator.Transfer(account, destinationAccount, amount, comment, ModelState);
            if (!ModelState.IsValid)
            {
                return ReturnWithError();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            // private method for repeatitive calling inside this method
            IActionResult ReturnWithError() {
                ViewBag.DestinationAccountNumber = destinationAccountNumber;
                ViewBag.Amount = amount;
                ViewBag.Comment = comment;
                return View(actionOrigin, account);
            }
        }
    }
}
