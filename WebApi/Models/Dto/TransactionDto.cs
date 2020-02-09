using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Dto
{
    public class TransactionDto
    {
        public int TransactionID { get; set; }
        public string TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int? DestinationAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public DateTime TransactionTime { get; set; }
        public string TransactionTimeStr { get; set; }

        [JsonIgnore]
        public Transaction Transaction;

        public TransactionDto() { }

        public TransactionDto(Transaction tran)
        {
            TransactionID = tran.TransactionID;
            TransactionType = tran.TransactionType.ToString();
            AccountNumber = tran.AccountNumber;
            DestinationAccountNumber = tran.DestinationAccountNumber;
            Amount = tran.Amount;
            Comment = tran.Comment;

            DateTime newDate = DateTime.SpecifyKind(tran.TransactionTimeUtc, DateTimeKind.Utc);
            TransactionTime = newDate.ToLocalTime();
            TransactionTimeStr = newDate.ToLocalTime().ToString("dd/MM/yyyy hh:mm:ss tt");

            Transaction = tran;
        }
    }
}
