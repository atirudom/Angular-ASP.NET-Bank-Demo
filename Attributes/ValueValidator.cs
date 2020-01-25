using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Attributes
{
    public static class ValueValidator
    {
        public static bool HasMoreThanNDecimalPlaces(this decimal value, int n) => decimal.Round(value, n) != value;
        public static bool HasMoreThanTwoDecimalPlaces(this decimal value) => value.HasMoreThanNDecimalPlaces(2);
    }
}
