using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank.CurrencyFolder
{
    internal class EUR : Currency
    {
        public override string Name
        {
            get
            {
                return "Euro";
            }
        }
        public override string CurrencyCode
        {
            get
            {
                return "EUR";
            }
        }
    }
}
