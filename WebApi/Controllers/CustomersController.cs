using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.Models;
using AdminApi.Models.DataManager;
using AdminApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            foreach(var customer in customerLists)
            {
                customerDtoLists.Add(new CustomerDto(customer));
            }

            return customerDtoLists;
        }
    }
}