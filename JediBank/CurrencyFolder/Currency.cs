using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank.CurrencyFolder
{
    internal class Currency
    {
        /// <summary>
        /// Full Currency name
        /// </summary>
        public string Name { get; set;  }
        /// <summary>
        /// ISO 4217 Standard
        /// https://en.wikipedia.org/wiki/ISO_4217
        /// </summary>
        public string CurrencyCode { get; set;  }
        /// <summary>
        /// Output Format
        /// https://www.csharp-examples.net/culture-names/
        /// </summary>
        public NumberFormatInfo GetOutputFormat()
        {
            return NumberFormatInfo.InvariantInfo;
        }
    }
}
