using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Analyzer
{
    public class TransactionPerTypeAns
    {
        public string Type { get; set; }
        public int TransactionCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
