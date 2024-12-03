using JediBank.CurrencyFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace JediBank
{
    internal class Transaction
    {
        public Account SenderAccount { get; set; }
        public Account ReciverAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime  { get; set; }
        public Currency Currency { get; set; }


        public void ExecuteTransaction()
        {
            SenderAccount.Subtract(Amount);
            ReciverAccount.Add(Amount);
            SenderAccount.TransactionHistory.Add(this);
            ReciverAccount.TransactionHistory.Add(this);
        }

        public void ShowTransaction()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"From: {SenderAccount?.Name} To: {ReciverAccount?.Name} Amount transferred: {Amount.ToString("c", SenderAccount?.Currency.GetOutputFormat())} Time of transfer: {DateTime}\n");
            Console.ResetColor();

        }
    }


}
