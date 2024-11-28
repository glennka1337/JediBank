using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank.Currency
{
    internal class USD : ICurrency
    {
        public string Name
        {
            get
            {
                return "United States Dollar";
            }
        }
        public string CurrencyCode
        {
            get
            {
                return "USD";
            }
        }
        public NumberFormatInfo Format
        {
            get
            {
                return CultureInfo.GetCultureInfo("en-US").NumberFormat;
            }
        }
    }
}
