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

namespace Assignment2.Controllers
{
    public class BillPayController : Controller
    {
        private readonly MainContext _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public BillPayController(MainContext context)
        {
            _context = context;
        }

        // GET: BillPay
        public IActionResult Index()
        {
            List<Account> accounts = _context.Customers.FirstOrDefault(c => c.CustomerID == CustomerID).Accounts;
            List<BillPay> billPays = accounts.SelectMany(a => a.BillPays).ToList();
            var mainContext = billPays;
            return View(mainContext);
        }

        // GET: BillPay/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPay = await _context.BillPays
                .Include(b => b.Account)
                .Include(b => b.Payee)
                .FirstOrDefaultAsync(m => m.BillPayID == id);
            if (billPay == null)
            {
                return NotFound();
            }

            return View(billPay);
        }

        // GET: BillPay/Create
        public IActionResult Create()
        {
            var customerAccounts = _context.Customers.FirstOrDefault(c => c.CustomerID == CustomerID).Accounts;
            ViewData["AccountNumber"] = new SelectList(customerAccounts, "AccountNumber", "AccountNumber");
            ViewData["PayeeID"] = new SelectList(_context.Payees, "PayeeID", "PayeeName");
            ViewData["Period"] = new SelectList(Enum.GetValues(typeof(BillPeriod)));
            return View();
        }

        // POST: BillPay/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillPayID,AccountNumber,PayeeID,Amount,ScheduleDate,Period,ModifyDate")] BillPay billPay)
        {
            // input fixed values
            billPay.ModifyDate = DateTime.UtcNow;
            billPay.Status = BillStatus.Normal;
            billPay.ScheduleDate = DateTime.SpecifyKind(billPay.ScheduleDate, DateTimeKind.Local).ToUniversalTime();

            // input validation
            if (billPay.ScheduleDate <= DateTime.UtcNow)
            {
                ModelState.AddModelError(nameof(billPay.ScheduleDate), "Schedule time must be after this time");
            }
            if (ModelState.IsValid)
            {
                _context.Add(billPay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // operation when error occurs
            var customerAccounts = _context.Customers.FirstOrDefault(c => c.CustomerID == CustomerID).Accounts;
            ViewData["AccountNumber"] = new SelectList(customerAccounts, "AccountNumber", "AccountNumber");
            ViewData["PayeeID"] = new SelectList(_context.Payees, "PayeeID", "PayeeName");
            ViewData["Period"] = new SelectList(Enum.GetValues(typeof(BillPeriod)));
            return View(billPay);
        }

        // GET: BillPay/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPay = await _context.BillPays.FindAsync(id);
            if (billPay == null)
            {
                return NotFound();
            }
            var customerAccounts = _context.Customers.FirstOrDefault(c => c.CustomerID == CustomerID).Accounts;
            ViewData["AccountNumber"] = new SelectList(customerAccounts, "AccountNumber", "AccountNumber");
            ViewData["PayeeID"] = new SelectList(_context.Payees, "PayeeID", "PayeeName");
            ViewData["Period"] = new SelectList(Enum.GetValues(typeof(BillPeriod)));
            return View(billPay);
        }

        // POST: BillPay/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillPayID,AccountNumber,PayeeID,Amount,ScheduleDate,Period,ModifyDate")] BillPay billPay)
        {
            // Input fixed value
            billPay.ModifyDate = DateTime.UtcNow;
            billPay.Status = BillStatus.Normal;
            billPay.ScheduleDate = DateTime.SpecifyKind(billPay.ScheduleDate, DateTimeKind.Local).ToUniversalTime();
            billPay.StatusMessage = null;
            billPay.Status = BillStatus.Normal;

            // User input validation
            if (billPay.ScheduleDate <= DateTime.UtcNow)
            {
                ModelState.AddModelError(nameof(billPay.ScheduleDate), "Schedule time must be after this time");
            }

            if (id != billPay.BillPayID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billPay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillPayExists(billPay.BillPayID))
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
            var customerAccounts = _context.Customers.FirstOrDefault(c => c.CustomerID == CustomerID).Accounts;
            ViewData["AccountNumber"] = new SelectList(customerAccounts, "AccountNumber", "AccountNumber");
            ViewData["PayeeID"] = new SelectList(_context.Payees, "PayeeID", "PayeeName");
            ViewData["Period"] = new SelectList(Enum.GetValues(typeof(BillPeriod)));
            return View(billPay);
        }

        // GET: BillPay/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPay = await _context.BillPays
                .Include(b => b.Account)
                .Include(b => b.Payee)
                .FirstOrDefaultAsync(m => m.BillPayID == id);
            if (billPay == null)
            {
                return NotFound();
            }

            return View(billPay);
        }

        // POST: BillPay/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billPay = await _context.BillPays.FindAsync(id);
            _context.BillPays.Remove(billPay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillPayExists(int id)
        {
            return _context.BillPays.Any(e => e.BillPayID == id);
        }
    }
}
