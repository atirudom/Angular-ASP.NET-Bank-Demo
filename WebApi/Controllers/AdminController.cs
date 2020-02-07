using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.Controllers.Functions;
using AdminApi.Data;
using AdminApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly MainContext _context;

        public AdminController(MainContext context)
        {
            _context = context;
        }

        // GET: api/Admin
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/Admin/KingLionSecLogin
        [HttpPost("[action]")]
        public object KingLionSecLogin([FromBody] AdminLoginDto data)
        {
            bool isLoginSuccess = Authentication.AuthenticateAdmin(data.UserID, data.Password);
            return new { LoginSuccess = isLoginSuccess };
        }
    }
}