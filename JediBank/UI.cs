using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediBank
{
    internal class UI
    {
        public string ReadUserName()
        {
            Console.CursorVisible = true; 
            Console.Write("\rEnter your Username: \n");

            ConsoleKey key;
            string input = "";
            do
            {
                var keyPressed = Console.ReadKey(intercept: true);
                key = keyPressed.Key;
                if (char.IsLetter(keyPressed.KeyChar))
                {
                    input += keyPressed.KeyChar;
                    Console.Write(keyPressed.KeyChar);
                }
                else if (key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine();
            return input;
        }
        public string ReadPassword()
        {
            Console.CursorVisible = true;
            Console.Write("\rEnter your pin code: \n");
            ConsoleKey key;
            string input = "";
            do
            {
                var keyPressed = Console.ReadKey(intercept: true);
                key = keyPressed.Key;
                if (char.IsDigit(keyPressed.KeyChar))
                {
                    input += keyPressed.KeyChar;
                    Console.Write("*");
                }
                else if (key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine();
            return input;
        }

        public int Menu(string[] items)
        {
            Console.CursorVisible = false;
            ConsoleKey key;
            int currentSelection = 0;
            do
            {
                Console.Clear();
                for (int i = 0; i < items.Length; i++)
                {
                    if (i == currentSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($" {items[i]} ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($" {items[i]} ");
                    }
                }
                var keyPressed = Console.ReadKey(intercept: true);
                key = keyPressed.Key;
                if (key == ConsoleKey.UpArrow)
                {
                    currentSelection = currentSelection == 0 ? items.Length - 1 : currentSelection - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    currentSelection = currentSelection == items.Length - 1 ? 0 : currentSelection + 1;
                }

            } while (key != ConsoleKey.Enter);
            return currentSelection;
        }
        public string MainMenu(Dictionary<string, string[]> menuItems, string? message)
        {
            Console.CursorVisible = false;
            int maxLength = menuItems.Keys.Max(key => key.Length);
            Console.OutputEncoding = Encoding.UTF8;
            //initialize the options menu with the head options;
            Dictionary<string, bool> headClicked = new Dictionary<string, bool>();
            List<string> items = new List<string>();
            foreach (var item in menuItems)
            {
                //Add the head options to the list
                items.Add(item.Key);
                //add the head options to the list and set all to false since none have been clicked yet.
                headClicked.Add(item.Key, false);
            }

            //Menu loop, exits loop if a sub options is cklicked
            ConsoleKey key;
            int currentSelection = 0;
            while (true)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Välkommen " + message.ToUpper());
                    var time = DateTime.Now.TimeOfDay;
                    Console.SetCursorPosition(Console.WindowWidth - 15, 0);
                    Console.Write($"Time: {DateTime.Now.ToString("HH:mm")}\n");
                    Console.SetCursorPosition(0, 1);
                    for (int i = 0; i < items.Count; i++)
                    {

                        string triangle = headClicked.ContainsKey(items[i]) ? (headClicked[items[i]] ? "▼" : "▲") : "";
                        if (i == currentSelection)
                        {
                            SetColor(menuItems.ContainsKey(items[i]));
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(menuItems.ContainsKey(items[i]) ? $"{items[i]} {new string(' ', maxLength - items[i].Length + 4)} {triangle}" : $" ◯ {items[i]} {new string(' ', maxLength - items[i].Length + 1)}");

                            Console.ResetColor();
                        }
                        else
                        {
                            SetColor(menuItems.ContainsKey(items[i]));
                            Console.WriteLine(menuItems.ContainsKey(items[i]) ? $"{items[i]} {new string(' ', maxLength - items[i].Length + 4)} {triangle}" : $" ◯ {items[i]} {new string(' ', maxLength - items[i].Length + 1)}");

                            Console.ResetColor();
                        }
                    }
                    var keyPressed = Console.ReadKey(intercept: true);
                    key = keyPressed.Key;
                    if (key == ConsoleKey.UpArrow)
                    {
                        currentSelection = currentSelection == 0 ? items.Count - 1 : currentSelection - 1;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        currentSelection = currentSelection == items.Count - 1 ? 0 : currentSelection + 1;
                    }

                } while (key != ConsoleKey.Enter);
                //if a sub menu is chosen return the name of that choice
                if (!menuItems.ContainsKey(items[currentSelection]))
                {
                    return items[currentSelection];
                }
                else
                {
                    //checks if we clicked a previously clicked head option.
                    if (headClicked[items[currentSelection]])
                    {
                        //Remove the sub options from the list for the klicked head option.
                        items.RemoveAll(item => menuItems[items[currentSelection]].Contains(item));
                    }
                    else
                    {
                        //Adds the string array of the sub options after the head option in the list.
                        items.InsertRange(currentSelection + 1, menuItems[items[currentSelection]]);

                    }

                    //change the value of the clicked option, if it was clicked and now clicked again it returns to false. else it is set to true
                    headClicked[items[currentSelection]] = headClicked[items[currentSelection]] ? false : true;
                }

            }
        }

        public void SetColor(bool type)
        {
            if (type)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(2, Console.GetCursorPosition().Top);
            }
        }

        public void AccountMenu(User user, Account account)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Konto: {account.Name} | Saldo: {account.Balance} {account.Currency} ");
                Console.WriteLine("--------------------------------------");

                Console.WriteLine("1. Visa senaste transaktioner (WIP) ");
                Console.WriteLine("2. Ta ut pengar ");
                Console.WriteLine("3. Överför pengar ");
                Console.WriteLine("4. Återgå till huvudmenyn ");
                Console.Write("Välj något av ovanstående alternativ: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Här ska historik vara. WIP ");
                        Console.WriteLine("Tryck på valfri tangent för att fortsätta. ");
                        Console.ReadKey();
                        break;

                    case "2":
                        ExecuteWithdraw(user, account);
                        break;   
                    
                    case "3":
                        ExecuteTransfer(user, account);
                        break;   
                    
                    case "4":
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val. Ange ett nummer mellan 1-4. ");
                        Console.ResetColor();
                        Console.WriteLine("Tryck på valfri tangent för att fortsätta. ");
                        Console.ReadKey();
                        break;  
                }
            }
        }

        private void ExecuteWithdraw(User user, Account account)
        {
            Console.WriteLine("\nAnge belopp som du vill ta ut: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (account.Subtract(amount))
                {
                    Console.WriteLine($"{amount} {account.Currency} har tagits ut. ");
                }
                else
                {
                    Console.WriteLine("Ej tillräckligt stort saldo. ");
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt belopp. ");
            }
            Console.WriteLine("Tryck på valfri tangent för att fortsätta. ");
            Console.ReadKey();
        }

        private void ExecuteTransfer(User user, Account account)
        {
            Console.WriteLine("\nVälj ett konto att överföra till: ");
            var otherAccounts = user.Accounts.Where(acc => acc != account).ToList();
            for (int i = 0; i < otherAccounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {otherAccounts[i].Name} - {otherAccounts[i].Balance} {otherAccounts[i].Currency}");
            }
            Console.WriteLine("\nAnge siffra för att välja konto: ");
            if (int.TryParse(Console.ReadLine(), out int selected) && selected > 0 && selected <= otherAccounts.Count)
            {
                Account toAccount = otherAccounts[selected - 1];
                Console.Write("Ange belopp att överföra: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    if (account.TransferFunds(amount, toAccount))
                    {
                        Console.WriteLine($" {amount} {account.Currency} har överförts till {toAccount.Name}. ");
                    }
                    else
                    {
                        Console.WriteLine("Otillräckligt saldo eller ogiltigt belopp. ");
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt belopp. ");
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt val. ");
            }
            Console.WriteLine("Tryck på valfri tangent för att fortsätta: ");
            Console.ReadKey();
        }

    }
}
