using System;
using System.Collections.Generic;
using System.Linq;
using JediBank.CurrencyFolder;
using System.Text;
using System.Threading.Tasks;

namespace JediBank
{
    internal class UI
    {
        public string ReadUserName()
        {
            Console.CursorVisible = true;
            Console.SetCursorPosition((Console.WindowWidth - "Enter your Username: ".Length) / 2, Console.WindowHeight / 2);
            Console.Write("Enter your Username: ");

            ConsoleKey key;
            string input = "";
            Console.SetCursorPosition((Console.WindowWidth - "Enter your Username: ".Length) / 2, Console.WindowHeight / 2 + 1);
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
            Console.SetCursorPosition((Console.WindowWidth - "Enter your Username: ".Length) / 2, Console.GetCursorPosition().Top);
            Console.Write("Enter your pin code: ");

            ConsoleKey key;
            string input = "";

            Console.SetCursorPosition((Console.WindowWidth - "Enter your Username: ".Length) / 2, Console.GetCursorPosition().Top + 1);

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

                if (key == ConsoleKey.Enter)
                {
                    int cursorLeft = Console.CursorLeft;
                    int cursorTop = Console.CursorTop;
                    Console.SetCursorPosition(cursorLeft - input.Length, cursorTop);
                    for (int i = 0; i < input.Length; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.SetCursorPosition(cursorLeft - input.Length, cursorTop);
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
                    Console.SetCursorPosition((Console.WindowWidth - items[0].Length) / 2, Console.WindowHeight / 2 + i);
                    if (i == currentSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
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
            int X = 2;
            int width = 26;
            int height = menuItems.Keys.Count;
            foreach (var item in menuItems) 
            {
                height += item.Value.Length;
            }

            //Menu loop, exits loop if a sub options is cklicked
            ConsoleKey key;
            int currentSelection = 0;
            while (true)
            {
                do
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 3);
                    PaintBox("Main Menu", width, height, 0);
                    //Console.WriteLine("Välkommen " + message.ToUpper());
                    var time = DateTime.Now.TimeOfDay;
                    Console.SetCursorPosition(Console.WindowWidth - 15, 0);
                    Console.Write($"Time: {DateTime.Now.ToString("HH:mm")}\n");
                    Console.SetCursorPosition(0, 7);
                    for (int i = 0; i < items.Count; i++)
                    {

                        string triangle = headClicked.ContainsKey(items[i]) ? (headClicked[items[i]] ? "▼" : "▲") : "";
                        bool selected = i == currentSelection ? true : false;
                        if (selected)
                        {
                            SetColor(menuItems.ContainsKey(items[i]), X, selected);
                            /*Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.White;*/
                            Console.WriteLine(menuItems.ContainsKey(items[i]) ? $"{items[i]} {new string(' ', maxLength - items[i].Length + 4)} {triangle}" : $" ◯ {items[i]} {new string(' ', maxLength - items[i].Length + 1)}");

                            Console.ResetColor();
                        }
                        else
                        {
                            SetColor(menuItems.ContainsKey(items[i]), X, selected);
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

        public void SetColor(bool type, int X, bool selected)
        {
            if (selected)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(type ? X : X +2 , Console.GetCursorPosition().Top);
            }
            else if (type)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(X, Console.GetCursorPosition().Top);
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(X+2, Console.GetCursorPosition().Top);
            }
        }

        public void AccountMenu(User user, Account account)
        {
            Console.OutputEncoding = Encoding.UTF8;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Konto: {account.Name} | Saldo: {account.Balance.ToString("c", account.Currency.GetOutputFormat())} ");
                Console.WriteLine("--------------------------------------");

                Console.WriteLine("1. Visa senaste transaktioner ");
                Console.WriteLine("2. Ta ut pengar ");
                Console.WriteLine("3. Överför pengar ");
                Console.WriteLine("4. Återgå till huvudmenyn ");
                Console.Write("Välj något av ovanstående alternativ: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        account.ShowHistory();
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
                Console.WriteLine($"{i + 1}. {otherAccounts[i].Name} - {otherAccounts[i].Balance.ToString("c", otherAccounts[i].Currency.GetOutputFormat())}");
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
                        Console.WriteLine($" {amount.ToString("c", account.Currency.GetOutputFormat())} har överförts till {toAccount.Name}. ");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Account[] TransferMenu(User user)
        {
            Account? sender = null;
            Account? reciever = null;
            Console.OutputEncoding = Encoding.UTF8;
            Dictionary<string, bool> textButton = new Dictionary<string, bool>
            {
                {"Amount", false },
                {"Reciever", false}
            };
            Dictionary<string, bool> buttonsClicked = new Dictionary<string, bool>
            {
                {"Cancel", false },
                {"Submit", false }
            };
            Dictionary<string, bool> headClicked = new Dictionary<string, bool>();
            Dictionary<string, string[]> menuItems = new Dictionary<string, string[]>
            {
                { "Sender account", user.GetAccountNames() },
                { "Reciever account", user.GetAccountNames() }
            };
            List<string> items = new List<string>();
            foreach (var item in menuItems)
            {
                //Add the head options to the list
                items.Add(item.Key);
                //add the head options to the list and set all to false since none have been clicked yet.
                headClicked.Add(item.Key, false);
            }
            //Lägger till cancel och submit knapparna
            items.Add(buttonsClicked.Keys.ToList()[0]);
            items.Add(buttonsClicked.Keys.ToList()[1]);
            items.Add(textButton.Keys.ToList()[0]);
            items.Add(textButton.Keys.ToList()[1]);
            List<string> chosenSender = new List<string>();
            List<string> chosenReciever = new List<string>();

            //Menu loop, exits loop if a sub options is cklicked
            ConsoleKey key;
            int currentSelection = 0;
            int maxL = 17;
            int yStart = 2;
            int width = 26;
            int height = 11 + 2 * menuItems["Sender account"].Count();
            while (true)
            {
                do
                {
                    Console.Clear();
                    Console.SetCursorPosition(9, 2);
                    PaintBox("Transfer", width, height, 9);
                    Console.SetCursorPosition(9, 6);
                    for (int i = 0; i < items.Count; i++)
                    {

                        int distance = headClicked[items[0]] ? 0 : menuItems["Sender account"].Length;
                        int space = i == items.IndexOf(menuItems.Keys.ToList()[1]) ? 1 : 0;
                        string triangle = headClicked.ContainsKey(items[i]) ? (headClicked[items[i]] ? "▼" : "▲") : "";
                        if (menuItems.ContainsKey(items[i]))
                        {

                            Console.SetCursorPosition(11, Console.GetCursorPosition().Top + distance * i + space);
                            Console.BackgroundColor = i == currentSelection ? ConsoleColor.Green : ConsoleColor.White;
                            Console.ForegroundColor = i == currentSelection ? ConsoleColor.White : ConsoleColor.Black;
                            if (chosenSender.Count > 0 && i < items.IndexOf(menuItems.Keys.ToList()[1]))
                            {
                                //✅✔️
                                Console.Write($" {chosenSender[0]} {new string(' ', maxL - chosenSender[0].Length)} ✅\n");
                                Console.ResetColor();
                            }
                            else if (chosenReciever.Count > 0 && i == items.IndexOf(menuItems.Keys.ToList()[1]))
                            {
                                Console.Write($" {chosenReciever[0]} {new string(' ', maxL - chosenReciever[0].Length)} ✅\n");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.Write($" {items[i]} {new string(' ', maxL - items[i].Length)} {triangle} \n");
                                Console.ResetColor();
                            }

                            Console.ResetColor();
                        }
                        else if (!buttonsClicked.Keys.Contains(items[i]))
                        {
                            if (textButton.Keys.Contains(items[i]))
                            {
                                /*int k = textButton.Keys.ToList().IndexOf(items[i]);
                                Textbox(textButton.Keys.ToList()[k], 14, 15, textButton[items[k]]);
                                Console.ResetColor();*/
                            }
                            else
                            {
                                Console.SetCursorPosition(14, Console.GetCursorPosition().Top);
                                Console.BackgroundColor = i == currentSelection ? ConsoleColor.Green : ConsoleColor.DarkGray;
                                Console.ForegroundColor = i == currentSelection ? ConsoleColor.White : ConsoleColor.White;
                                Console.Write($" ◯ {items[i]} {new string(' ', maxL - items[i].Length - 2)}\n");
                               
                                Console.ResetColor();

                            }
                        }
                        else if (textButton.Keys.ToList().Contains(items[i]))
                        {
                            int k = textButton.Keys.ToList().IndexOf(items[i]);
                            Textbox(textButton.Keys.ToList()[k], 14, 15, textButton[items[k]]);
                            Console.ResetColor();
                        }
                        else
                        {
                            //Writes out the buttons cancel and submit
                            int d1 = headClicked["Reciever account"] ? 0 : menuItems["Reciever account"].Length;
                            //int d2 = i == items.IndexOf(menuItems.Keys.ToList()[1]) ? 3 : 0;
                            if (buttonsClicked.Keys.ToList()[0] == items[i])
                            {
                                Console.BackgroundColor = i == currentSelection ? ConsoleColor.White : ConsoleColor.DarkRed;
                                Console.ForegroundColor = i == currentSelection ? ConsoleColor.DarkRed : ConsoleColor.White;
                                Console.SetCursorPosition(11, Console.GetCursorPosition().Top + d1 + 3);
                                Console.Write($" {buttonsClicked.Keys.ToList()[0]} ");
                                Console.ResetColor();
                            }
                            else if (buttonsClicked.Keys.ToList()[1] == items[i])
                            {
                                Console.BackgroundColor = i == currentSelection ? ConsoleColor.White : ConsoleColor.DarkGreen;
                                Console.ForegroundColor = i == currentSelection ? ConsoleColor.DarkGreen : ConsoleColor.White;
                                Console.SetCursorPosition(Console.GetCursorPosition().Left + 5, Console.GetCursorPosition().Top);
                                Console.Write($" {buttonsClicked.Keys.ToList()[1]} ");
                                Console.ResetColor();
                            }


                        }
                    }

                    var keyPressed = Console.ReadKey(intercept: true);
                    key = keyPressed.Key;
                    if (key == ConsoleKey.UpArrow || key == ConsoleKey.LeftArrow)
                    {
                        currentSelection = currentSelection == 0 ? items.Count - 1 : currentSelection - 1;
                    }
                    else if (key == ConsoleKey.DownArrow || key == ConsoleKey.RightArrow)
                    {
                        currentSelection = currentSelection == items.Count - 1 ? 0 : currentSelection + 1;
                    }
                } while (key != ConsoleKey.Enter);
                //if a sub menu is chosen return the name of that choice
                if (buttonsClicked.ContainsKey(items[currentSelection]))
                {
                    //Cancel button is clicked
                    if (buttonsClicked.Keys.ToList()[0] == items[currentSelection])
                    {
                        return [sender, reciever];
                    }
                    else if (buttonsClicked.Keys.ToList()[1] == items[currentSelection])
                    {
                        //Logic for checking if accounts have been chosen or if they are valid;
                        sender = chosenSender.Count() > 0 ? user.Accounts.Find(x => x.Name == chosenSender[0]) : sender;
                        reciever = chosenReciever.Count() > 0 ? user.Accounts.Find(x => x.Name == chosenReciever[0]) : reciever;
                        if (sender != reciever)
                        {
                            return [sender, reciever];
                        }
                        ErrorMessage("skicka ej till samma konto!");
                        Console.ReadKey();
                        chosenSender.Clear();
                        chosenReciever.Clear();
                        sender = null;
                        reciever = null;


                    }
                }
                else if (textButton.ContainsKey(items[currentSelection]))
                {
                    textButton[items[currentSelection]] = !textButton[items[currentSelection]];
                }
                else if (!menuItems.ContainsKey(items[currentSelection]))
                {
                    if (currentSelection < items.IndexOf(menuItems.Keys.ToList()[1]))
                    {
                        if (chosenSender.Count > 0)
                        {
                            chosenSender.Clear();
                        }
                        chosenSender.Add(items[currentSelection]);
                        //Remove sub options
                        items.RemoveRange(items.IndexOf(menuItems.Keys.ToList()[0]) + 1, menuItems[menuItems.Keys.ToList()[0]].Length);
                        //changes the bool for if the head option is clicked and adjusts the currentselection
                        headClicked[menuItems.Keys.ToList()[0]] = headClicked[menuItems.Keys.ToList()[0]] ? false : true;
                        currentSelection = items.IndexOf(menuItems.Keys.ToList()[0]);
                    }
                    else
                    {
                        if (chosenReciever.Count > 0)
                        {
                            chosenReciever.Clear();
                        }
                        chosenReciever.Add(items[currentSelection]);
                        items.RemoveRange(items.IndexOf(menuItems.Keys.ToList()[1]) + 1, menuItems[menuItems.Keys.ToList()[1]].Length);
                        //changes the bool for if the head option is clicked and adjusts the currentselection
                        headClicked[menuItems.Keys.ToList()[1]] = headClicked[menuItems.Keys.ToList()[1]] ? false : true;
                        currentSelection = items.IndexOf(menuItems.Keys.ToList()[1]);
                    }
                    //ändra rubrik
                }
                else
                {
                    string thisHead = items[currentSelection];
                    //checks if we clicked a previously clicked head option.
                    if (headClicked[items[currentSelection]])
                    {
                        //Remove the sub options from the list for the klicked head option.
                        //items.RemoveAll(item => menuItems[items[currentSelection]].Contains(item));
                        items.RemoveRange(currentSelection + 1, menuItems[items[currentSelection]].Length);
                    }
                    else
                    {
                        //Adds the string array of the sub options after the head option in the list.
                        items.InsertRange(currentSelection + 1, menuItems[items[currentSelection]]);

                    }

                    //change the value of the clicked option, if it was clicked and now clicked again it returns to false. else it is set to true
                    headClicked[thisHead] = headClicked[thisHead] ? false : true;
                    currentSelection = items.IndexOf(thisHead);
                }

            }
            return [sender, reciever];

        }

        public void PaintBox(string? message, int width, int height, int X)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            int center = width - message.Length;
            for (int i = 0; i < height+3; i++)
            {
                Console.SetCursorPosition(X, Console.GetCursorPosition().Top + 1);
                Console.Write(i == 1 ? $"{new string(' ', (center/2))}{message}{new string(' ', (center / 2 + (center % 2)))}" : $"{new string(' ', width)}");

            }
            Console.ResetColor();
        }

        public void ErrorMessage(string? message)
        {
            int width = 50;
            int height = 7;
            Console.Clear();
            Console.SetCursorPosition(9, 3);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(9, Console.GetCursorPosition().Top + 1);
                Console.Write(i == height / 2 ? $"{new string(' ', (width - message.Length) / 2)}{message} {new string(' ', (width - message.Length) / 2)}" : $"{new string(' ', width)}");

            }
            Console.ResetColor();
        }

        public void Textbox(string defaultText, int X, int Y, bool clicked)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(X, Y);
            Console.Write(defaultText);

            if (clicked) 
            { 
                ConsoleKey key;
                string input = defaultText;
            
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
            
            }
            Console.ResetColor ();
        }
    }
}
