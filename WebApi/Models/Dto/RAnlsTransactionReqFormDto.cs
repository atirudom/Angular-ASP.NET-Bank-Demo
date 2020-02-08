using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models.Dto
{
    // Receiving Analysis Transaction Request Form DTO
    public class RAnsTransactionReqFormDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ChartType { get; set; }
    }
}
