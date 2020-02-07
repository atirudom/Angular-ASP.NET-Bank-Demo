﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.Data;
using AdminApi.Models;
using AdminApi.Models.DataManager;
using AdminApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly LoginManager _repo;
        private MainContext _context;

        public LoginsController(LoginManager repo, MainContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<LoginDto> Get()
        {
            IEnumerable<Login> loginList = _repo.GetAll();
            List<LoginDto> loginDtoList = new List<LoginDto>();
            foreach (var login in loginList)
            {
                loginDtoList.Add(new LoginDto(login, login.Customer));
            }

            return loginDtoList;
        }

        // GET: api/Logins/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
    }
}