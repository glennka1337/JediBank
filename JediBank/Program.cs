using System.Text;

namespace JediBank
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Language language = new Language();
            Console.OutputEncoding = Encoding.UTF8;
            Bank bank = new Bank();
            bank.RunProgram(language);



            /*        Bank bank = new Bank();
                    bank.RunProgram();*/

        }
    }
}


//◊►▶-◯💲💱🏦🏧💰💸🪙 💳✅❌💼📉📈💹 📃🪪📶📊💷💶💵💴
