using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.CustomExceptions
{
    class TransactionRuleException : System.Exception
    {
        public string errMsg;
        public TransactionRuleException(string errMsg) : base(errMsg)
        {
            this.errMsg = errMsg;
        }
    }
}
