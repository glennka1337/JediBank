using JediBank.CurrencyFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JediBank
{
    internal class Transaction
    {
        public string SenderAccountName { get; set; }
        public string ReciverAccountName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public Currency Currency { get; set; }

        // Detta skrivs ej in i Json filen
        public Account SenderAccount { get; set; }
        public Account ReciverAccount { get; set; }

        public async Task ExecuteTransaction()
        {

            await Task.Delay(900); // Millisec

            SenderAccountName = SenderAccount.Name;
            ReciverAccountName = ReciverAccount.Name;

            //Auto swap Currencies
            if (SenderAccount.Currency != ReciverAccount.Currency)
            {
                ApiCaller apiCaller = new ApiCaller();
                decimal convertedAmount = apiCaller.Convert(SenderAccount.Currency, ReciverAccount.Currency, Amount);
                SenderAccount.Subtract(Amount);
                ReciverAccount.Add(convertedAmount);
            }
            else
            {
                SenderAccount.Subtract(Amount);
                ReciverAccount.Add(Amount);
            }




            SenderAccount.TransactionHistory.Add(this);
            ReciverAccount.TransactionHistory.Add(this);
        }

        public void ShowTransaction()
        {
            Language language = new Language();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(language.TranslationTool("From:") + $"{SenderAccountName}" + language.TranslationTool("To:") + $"{ReciverAccountName}" + language.TranslationTool("Amount transferred:") + $"{Amount.ToString("c", SenderAccount?.Currency.GetOutputFormat())}" + language.TranslationTool("Time of transfer:") + $"{DateTime}\n"); Console.ResetColor();

        }

    }


}
