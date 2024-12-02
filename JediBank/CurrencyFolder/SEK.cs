using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank.CurrencyFolder
{
    internal class SEK : Currency
    {
        public override string Name
        {
            get
            {
                return "Svensk krona";
            }
        }
        public override string CurrencyCode
        {
            get
            {
                return "SEK";
            }
        }
    }
}
