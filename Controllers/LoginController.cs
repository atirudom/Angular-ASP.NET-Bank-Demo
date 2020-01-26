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
using Microsoft.AspNetCore.Http;
using Assignment2.Attributes;

namespace Assignment2.Controllers
{
    public class LoginController : Controller
    {
        private readonly MainContext _context;

        public LoginController(MainContext context)
        {
            _context = context;
        }

        // GET: Login
        public async Task<IActionResult> Index()
        {
            var mainContext = _context.Logins.Include(l => l.Customer);
            return View(await mainContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userID, string password)
        {
            Customer loggedInCustomer = await Authentication.AuthenticateAsync(_context, userID, password);
            if (loggedInCustomer == null)
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(nameof(Index));
            }
            // Set session for loggedIn customer.
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), loggedInCustomer.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.Name), loggedInCustomer.Name);

            return RedirectToAction("Index", "ATM");
        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
