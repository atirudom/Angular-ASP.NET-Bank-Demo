﻿using Assignment2.Data;
using Assignment2.Models;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Controllers.Functions
{
    public class Authentication
    {
        public static async Task<Customer> AuthenticateAsync(MainContext context, string userID, string password)
        {
            //var login = context.Logins.FirstOrDefault(x => x.UserID == userID);
            var login = await context.Logins.FirstOrDefaultAsync(x => x.UserID == userID);

            // If userID is not found or password does not match
            if (login == null || password == null || !PBKDF2.Verify(login.PasswordHash, password))
            {
                login.AttemptLoginFailed();
                context.SaveChanges();
                return null;
            }
            else
            {
                login.AttemptLoginSuccessful();
                context.SaveChanges();
                return login.Customer;
            }
        }
    }
}
