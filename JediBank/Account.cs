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
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }

        // HISTORY PROPERTIE WIP (LEAVE FOR LATER)
        public void Show()
        {
            Console.WriteLine($"{Name},{Balance}");
        }

        public static void ShowHistory()
        {
            // WIP
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
    }
}
