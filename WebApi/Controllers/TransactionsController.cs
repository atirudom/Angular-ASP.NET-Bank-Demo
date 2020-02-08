using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.Models;
using AdminApi.Models.Analyzer;
using AdminApi.Models.DataManager;
using AdminApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionManager _repo;

        public TransactionsController(TransactionManager repo)
        {
            _repo = repo;
        }

        // GET: api/Transactions
        [HttpGet]
        public IEnumerable<Transaction> Get()
        {
            return _repo.GetAll();
        }

        // GET: api/Transactions/GetFromCustomer/5
        [HttpPost("GetFromCustomer")]
        public object GetFromCustomer([FromBody] RAnsTransactionReqFormDto bodyDto)
        {
            TransactionAnalyzer transactionAnalyzer = new TransactionAnalyzer(_repo.GetAll().ToList());
            var result = transactionAnalyzer.GenerateResults(bodyDto.FromDate, bodyDto.ToDate);
            return result;
        }

        // GET: api/Transactions/GetFromCustomer/5
        [HttpPost("GetFromCustomer/{customerID}")]
        public List<TransactionDateAns> GetFromCustomer(int customerID, [FromBody] RAnsTransactionReqFormDto bodyDto)
        {
            TransactionAnalyzer transactionAnalyzer = new TransactionAnalyzer(_repo.GetFromCustomerID(customerID).ToList());
            List<TransactionDateAns> result = transactionAnalyzer.GenerateResults(bodyDto.FromDate, bodyDto.ToDate);
            return result;
        }
    }
}
