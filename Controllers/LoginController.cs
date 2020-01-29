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
using Assignment2.Controllers.Functions;
using Assignment2.Persistence;

namespace Assignment2.Controllers
{
    public class LoginController : Controller
    {
        private readonly MainContext _context;

        public LoginController(MainContext context)
        {
            _context = context;
            //MainPersistence.RunBillPayPersistence(context);
            //_context.SaveChangesAsync();
        }

        public IActionResult Index() => View();

        [Route("/Login")]
        [HttpPost]
        public async Task<IActionResult> Login(string userID, string password)
        {
            Customer loggedInCustomer = await Authentication.AuthenticateAsync(_context, userID, password);
            if (loggedInCustomer == null)
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(nameof(Index), new Login { UserID = userID });
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

            return View(nameof(Index));
        }
    }
}
