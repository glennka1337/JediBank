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

            await Task.Delay(900000); // Millisec

            SenderAccount.Subtract(Amount);
            ReciverAccount.Add(Amount);

            SenderAccountName = SenderAccount.Name;
            ReciverAccountName = ReciverAccount.Name;

            SenderAccount.TransactionHistory.Add(this);
            ReciverAccount.TransactionHistory.Add(this);
        }

        public void ShowTransaction()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"From: {SenderAccountName} To: {ReciverAccountName} Amount transferred: {Amount.ToString("c", SenderAccount?.Currency.GetOutputFormat())} Time of transfer: {DateTime}\n");
            Console.ResetColor();

        }

    }


}
