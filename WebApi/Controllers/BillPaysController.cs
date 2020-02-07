using System;
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
    public class BillPaysController : ControllerBase
    {
        private readonly BillPayManager _repo;
        private MainContext _context;

        public BillPaysController(BillPayManager repo, MainContext context)
        {
            _repo = repo;
            _context = context;
        }

        // GET: api/BillPays/FromAccount/5
        [HttpGet("FromAccount/{accountNumber}", Name = "Get")]
        public List<BillPayDto> GetFromAccount(int accountNumber)
        {
            List<BillPayDto> billPayDtos = new List<BillPayDto>();
            _repo.GetFromAccount(accountNumber).ToList().ForEach(bill =>
            {
                billPayDtos.Add(new BillPayDto(bill));
            });
            return billPayDtos;
        }

        // PUT: api/BillPays/Block/5
        [HttpPut("Block/{billPayID}")]
        public object BlockBillPay(int billPayID)
        {
            BillPay billPay = _repo.Get(billPayID);
            billPay.Status = BillStatus.Blocked;
            billPay.StatusMessage = "blocked";
            _context.SaveChanges();

            return new { success = true };
        }

        // PUT: api/BillPays/Unblock/5
        [HttpPut("Unblock/{billPayID}")]
        public object UnblockBillPay(int billPayID)
        {
            BillPay billPay = _repo.Get(billPayID);
            billPay.Status = BillStatus.Normal;
            billPay.StatusMessage = "recently unblocked";
            _context.SaveChanges();

            return new { success = true };
        }
    }
}
