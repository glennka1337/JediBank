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
        public Account? SenderAccount { get; set; }
        public Account? ReciverAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime  { get; set; }
        public Currency Currency { get; set; }


        public void ExectureTransaction()
        {
            SenderAccount.Subtract(Amount);
            ReciverAccount.Add(Amount);
            SenderAccount.TransactionHistory.Add(this);
            ReciverAccount.TransactionHistory.Add(this);
        }

        public void ShowTransaction()
        {
            Console.WriteLine($"From: {SenderAccount} To: {ReciverAccount} Amount transfered: {Amount("c", SenderAccount.Currency.GetOutputFormat())} Time of transfer: {DateTime}");
        }
    }


}
