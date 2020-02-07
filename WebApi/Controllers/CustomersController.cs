using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.Models;
using AdminApi.Models.DataManager;
using AdminApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly CustomerManager _repo;

        public CustomersController(CustomerManager repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<CustomerDto> Get()
        {
            IEnumerable<Customer> customerLists = _repo.GetAll();
            List<CustomerDto> customerDtoLists = new List<CustomerDto>();
            foreach (var customer in customerLists)
            {
                CustomerDto cusDto = new CustomerDto(customer, _repo.GetLogin(customer.CustomerID));
                customerDtoLists.Add(cusDto);
            }

            return customerDtoLists;
        }

        // GET: api/Customers/5
        [HttpGet("{customerID}")]
        public CustomerDto Get(int customerID)
        {
            var customer = _repo.Get(customerID);
            CustomerDto cusDto = new CustomerDto(customer, _repo.GetLogin(customer.CustomerID));
            cusDto.PopulateAccounts();

            return cusDto;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public object Put(int id, CustomerDto cusDto)
        {
            Customer customer = cusDto.GenerateModel();
            if (id != customer.CustomerID)
            {
                return BadRequest();
            }
            _repo.Update(id, customer);

            return new { success = true };
        }
    }
}