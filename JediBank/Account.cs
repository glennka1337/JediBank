using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank
{
    internal class Account
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }

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
                return true;
            }
            if (amount > Balance) 
            {
                return true;
            }
            else
            {
                Balance -= amount;
                return true;
            }
        }

        public bool TransferFunds (decimal amount, Account toAccount)
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
