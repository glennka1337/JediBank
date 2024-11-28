using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank.Currency
{
    internal interface ICurrency
    {
        /// <summary>
        /// Full Currency name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// ISO 4217 Standard
        /// https://en.wikipedia.org/wiki/ISO_4217
        /// </summary>
        public string CurrencyCode { get; }
        /// <summary>
        /// Output Format
        /// https://www.csharp-examples.net/culture-names/
        /// </summary>
        public NumberFormatInfo Format { get; }
    }
}
