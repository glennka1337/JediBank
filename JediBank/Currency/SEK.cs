using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank.Currency
{
    internal class SEK : ICurrency
    {
        public string Name
        {
            get
            {
                return "Svensk krona";
            }
        }
        public string CurrencyCode
        {
            get
            {
                return "SEK";
            }
        }
        public NumberFormatInfo GetOutputFormat()
        {
            return CultureInfo.GetCultureInfo("sv-SE").NumberFormat;
        }
    }
}
