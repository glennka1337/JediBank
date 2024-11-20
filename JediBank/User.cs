using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank
{
    internal class User
    {
        public string Name { get; set; }
        private string _password;
        public List<Account> Accounts { get; set; }

        public void ShowAccounts()
        {

        }

        public bool TransferFunds(decimal amount)
        {
            return true;
        }

        public bool Withdraw(decimal amount)
        {
            return true;
        }

        public bool AddFunds(decimal amount)
        {
            return true;
        }

        public void Exchange(decimal amount, string currency)
        {

        }

        public void AddAccount()
        {

        }
    }
}
