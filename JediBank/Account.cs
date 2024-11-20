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
        public static void Show()
        {

        }

        public static void ShowHistory()
        {

        }

        // Måste returna något, de därför de visar fel atm
        public bool Add(decimal amount)
        {
            // TASK: CHANGE RETURN 
            return true;
        }

        // Samma här
        public bool Subtract(decimal amount)
        {
            // TASK: CHANGE RETURN 
            return true;
        }


    }
}
