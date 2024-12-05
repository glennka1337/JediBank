using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank.CurrencyFolder
{
    public class Currency
    {
        /// <summary>
        /// Full Currency name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// ISO 4217 Standard
        /// https://en.wikipedia.org/wiki/ISO_4217
        /// </summary>
        public virtual string CurrencyCode { get; set; }

        public CultureInfo GetOutputFormat()
        {
            switch (CurrencyCode)
            {
                case "USD":
                    return CultureInfo.GetCultureInfo("en-US");
                case "EUR":
                    return CultureInfo.GetCultureInfo("fi-FI");
                case "SEK":
                    return CultureInfo.GetCultureInfo("sv-SE");
                default:
                    return CultureInfo.InvariantCulture;
            }
        }
    }
}
