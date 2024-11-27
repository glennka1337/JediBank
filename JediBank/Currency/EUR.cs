using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank.Currency
{
    internal class EUR : ICurrency
    {
        public string Name
        {
            get
            {
                return "Euro";
            }
        }
        public string CurrencyCode
        {
            get
            {
                return "EUR";
            }
        }
        public NumberFormatInfo Format
        {
            get
            {
                return CultureInfo.GetCultureInfo("fi-FI").NumberFormat;
            }
        }
    }
}
