using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JediBank.CurrencyFolder;
namespace JediBank
{
    internal class Account
    {
        public string Name { get; set; }
        public string AccountId { get; set; }
        public decimal Balance { get; set; }
        public string IsCheckingaccount { get; set; }
        public decimal Interest {  get; set; }
        public Currency Currency
        { get; set; }
        public List<Transaction> TransactionHistory { get; set; } = new List<Transaction>();
        public void Show()
        {
            Console.WriteLine($"{Name},{Balance}");
        }

        public void ShowHistory()
        {
            foreach (var t in TransactionHistory)
            {
                t.ShowTransaction();
            }
        }

        public bool Add(decimal amount)
        {
            if (amount < 0)
            {
                return false;
            }
            // CHANGE AFTER CURRENCY UPDATE
            if (amount > 15000)
            {
                return false;
            }
            else
            {
                Balance += amount;
                return true;
            }

        }

        public bool Subtract(decimal amount)
        {
            if (amount < 0)
            {
                return false;
            }
            if (amount > Balance)
            {
                return false;
            }
            else
            {
                Balance -= amount;
                return true;
            }
        }

        public bool TransferFunds(decimal amount, Account toAccount)
        {
            if (amount <= 0 || amount > Balance)
            {
                return false;
            }
            else
            {
                Balance -= amount;
                toAccount.Balance += amount;
                return true;
            }
        }
    }
}
