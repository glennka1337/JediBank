using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank
{
    internal class Generator
    {
        private static Random rng = new Random();
        private static readonly string _chars = "QWERTYUIOPASDFGHJKLZXCVBNM";
        private static readonly string _nums = "0123456789";
        public static string GenerateAccountId()
        {
            while (true)
            {
                string tmpId = "A-";
                for (int i = 0; i < 4; i++)
                {
                    tmpId += _nums[rng.Next(_nums.Length)];
                }
                int count = 0;
                int totalAccountsChecked = 0;
                for (int i = 0; i < Bank.Users.Count; i++)
                {
                    for (int j = 0; j < Bank.Users[i].Accounts.Count; j++)
                    {
                        if (Bank.Users[i].Accounts[j].AccountId != tmpId)
                        {
                            count++;
                        }
                        totalAccountsChecked++;
                    }
                }
                if (count == totalAccountsChecked)
                {
                    return tmpId;
                }
            }
        }
    }
}
