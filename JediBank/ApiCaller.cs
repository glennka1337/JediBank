using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using JediBank.CurrencyFolder;
using System.Threading.Tasks;

namespace JediBank
{
    internal class ApiCaller
    {
        /// <summary>
        /// Gets the conversion rate between the specified currencies
         /// </summary>
        /// <param name="from">The currency to convert</param>
        /// <param name="to">The currency to convert to</param>
        /// <returns>Conversion Rate in decimal</returns>
        public decimal GetRate(Currency from, Currency to)
        {
            decimal rate = getConversionRate(from, to);
            return rate;
        }
        /// <summary>
        /// Converts an amount on currency to another.
        /// </summary>
        /// <param name="from">The currency to convert</param>
        /// <param name="to">The currency to convert to</param>
        /// <param name="amount">The amount to convert</param>
        /// <returns>Amount converted to [to] currency</returns>
        public decimal Convert(Currency from, Currency to, decimal amount)
        {
            decimal rate = getConversionRate(from, to);
            return rate * amount;
        }
        private decimal getConversionRate(Currency fromCurrency, Currency toCurrency)
        {
            try
            {
                string fromCurrencyCode = fromCurrency.CurrencyCode.ToLower();
                string toCurrencyCode = toCurrency.CurrencyCode.ToLower();

                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://cdn.jsdelivr.net/npm/@fawazahmed0/currency-api@latest/v1/currencies/{fromCurrencyCode}.json";
                    HttpResponseMessage res = client.GetAsync(url).Result;
                    res.EnsureSuccessStatusCode();
                    JsonDocument doc = JsonDocument.Parse(res.Content.ReadAsStream());
                    var values = doc.RootElement.GetProperty(fromCurrencyCode);
                    return decimal.Parse(values.GetProperty(toCurrencyCode).ToString().Replace('.', ','));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }
    }
}
