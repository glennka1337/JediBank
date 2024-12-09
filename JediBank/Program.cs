using System.Text;

namespace JediBank
{
    internal class Program
    {
        public static string ChoosenLangugage { get; set; }
        static void Main(string[] args)
        {
            var language = new Language();
            ChoosenLangugage = "English";//language.ChooseLanguage();
            Console.OutputEncoding = Encoding.UTF8;
            Bank bank = new Bank();
            bank.RunProgram();
            

            /*        Bank bank = new Bank();
                    bank.RunProgram();*/

        }
    }
}


//◊►▶-◯💲💱🏦🏧💰💸🪙 💳✅❌💼📉📈💹 📃🪪📶📊💷💶💵💴
