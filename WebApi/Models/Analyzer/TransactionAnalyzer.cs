using AdminApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Analyzer
{
    public class TransactionAnalyzer
    {
        private IEnumerable<Transaction> _transactions;
        private List<TransactionDateAns> resultTransactionDateAns;

        private DateTime ThresholdDate = DateTime.UtcNow.AddYears(-200);

        public TransactionAnalyzer(List<Transaction> transactions)
        {
            _transactions = transactions;
            resultTransactionDateAns = new List<TransactionDateAns>();
        }

        public List<TransactionDateAns> GenerateResults(DateTime from, DateTime to)
        {
            // if from is not set
            if(from < ThresholdDate)
            {
                var orderedTrans = _transactions.OrderBy(t => t.TransactionTimeUtc);
                from = orderedTrans.ToList()[0].TransactionTimeUtc;
            }
            // if to is not set
            if (to < ThresholdDate)
            {
                to = DateTime.UtcNow;
            }

            int dayDiff = Convert.ToInt32((to - from).TotalDays);
            dayDiff += 1; // For counting its own day
            return GenerateResults(from, dayDiff);
        }

        public List<TransactionDateAns> GenerateResults(DateTime from, int dayDiff)
        {
            var result = new List<TransactionDateAns>(new TransactionDateAns[dayDiff]);
            var date = from;   // If outside of duration = get today
            for (int i = 0; i < dayDiff; i++, date = date.AddDays(1))
            {
                IEnumerable<Transaction> tmpTrans = _transactions.Where(t => t.TransactionTimeUtc.Date == date.Date);
                decimal totalAmount = 0;
                foreach(var t in tmpTrans)
                {
                    totalAmount += t.Amount;
                }
                result[i] = new TransactionDateAns
                {
                    Date = date.ToString("dd/MM/yyyy"),
                    TransactionCount = tmpTrans.Count(),
                    TotalAmount = totalAmount
                };
            }
            return result;
        }
    }
}
