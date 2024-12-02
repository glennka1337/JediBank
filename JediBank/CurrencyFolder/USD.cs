using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank.CurrencyFolder
{
    internal class USD : Currency
    {
        public override string Name
        {
            get
            {
                return "United States Dollar";
            }
        }
        public override string CurrencyCode
        {
            get
            {
                return "USD";
            }
        }
    }
}
