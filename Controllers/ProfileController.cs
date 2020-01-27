using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using SimpleHashing;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using Assignment2.Attributes;

namespace Assignment2.Controllers
{
    [AuthorizeCustomer]
    public class ProfileController : Controller
    {
        private readonly MainContext _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public ProfileController(MainContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ViewMyStatement(int? page = 1, string accountType = "All")
        {
            // Retrieve customer object from context
            var customer = await _context.Customers.FindAsync(CustomerID);
            ViewBag.Customer = customer;

            // Page the orders, maximum of 4 per page.
            const int pageSize = 4;

            var transactions = _context.Transactions;
            IQueryable<Transaction> resultTransactions;
            switch (accountType)
            {
                case "Saving":
                    resultTransactions = transactions.Where(x => x.Account.AccountType == AccountType.Saving);
                    break;
                case "Checking":
                    resultTransactions = transactions.Where(x => x.Account.AccountType == AccountType.Checking);
                    break;
                default:
                    resultTransactions = transactions;
                    break;
            }
            var pagedList = await resultTransactions.ToPagedListAsync(page, pageSize);

            return View(pagedList);
        }

        // GET: Profile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Profile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Profile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,Name,TFN,Address,City,State,PostCode,Phone")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public IActionResult ChangePassword(int customerID) => View(_context.Logins.Where(x => x.CustomerID == customerID).FirstOrDefault().UserID);

        public async Task<IActionResult> ChangePassword(int userID, string newPassword)
        {
            Login login = await _context.Logins.FindAsync(userID);
            login.PasswordHash = PBKDF2.Hash(newPassword, 50000, 64);

            //_context.Update(login);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), login.CustomerID);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
    }
}
