using System.Text;

namespace JediBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var users = DataBase.LoadUsers();

            foreach (var user in users)
            {
                foreach (Account account in user.Accounts)
                {
                    Console.WriteLine(account.Balance.ToString("c", account.Currency.GetOutputFormat()));
                }
            }
            Console.Read();

            /*        Bank bank = new Bank();
                    bank.RunProgram();*/
        }
    }
}


//◊►▶-◯💲💱🏦🏧💰💸🪙 💳✅❌💼📉📈💹 📃🪪📶📊💷💶💵💴
