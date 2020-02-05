﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.CustomExceptions
{
    class BusinessRulesException : System.Exception
    {
        public string errMsg;
        public BusinessRulesException(string errMsg) : base(errMsg)
        {
            this.errMsg = errMsg;
        }
    }
}
