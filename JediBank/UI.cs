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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void TransferMenu(User user)
        {
            Account? sender = null;
            Account? reciever = null;
            Console.OutputEncoding = Encoding.UTF8;
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

            List<string> chosenSender = new List<string>();
            List<string> chosenReciever = new List<string>();

            //Menu loop, exits loop if a sub options is cklicked
            ConsoleKey key;
            int currentSelection = 0;
            int maxL = 17;
            int yStart = 2;
            int width = 26;
            while (true)
            {
                do
                {
                    Console.Clear();
                    Console.SetCursorPosition(9, 2);
                    PaintBox("Transfer", width);
                    Console.SetCursorPosition(9, 6);
                    for (int i = 0; i < items.Count; i++)
                    {
                        
                        int distance = headClicked[items[0]] ?  0 : menuItems["Sender account"].Length;
                        int space = i == items.IndexOf(menuItems.Keys.ToList()[1]) ? 1 : 0;
                        string triangle = headClicked.ContainsKey(items[i]) ? (headClicked[items[i]] ? "▼" : "▲") : "";
                        if (menuItems.ContainsKey(items[i]))
                        {
                            
                            Console.SetCursorPosition(11,Console.GetCursorPosition().Top + distance*i+space );
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
                        else if(!buttonsClicked.Keys.Contains(items[i]))
                        {
                            Console.SetCursorPosition(14, Console.GetCursorPosition().Top);
                            Console.BackgroundColor = i == currentSelection ? ConsoleColor.Green : ConsoleColor.DarkGray;
                            Console.ForegroundColor = i == currentSelection ? ConsoleColor.White : ConsoleColor.White;
                            Console.Write($" ◯ {items[i]} {new string(' ', maxL - items[i].Length - 2)}\n");
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
                            else if(buttonsClicked.Keys.ToList()[1] == items[i])
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
                    if(currentSelection < items.IndexOf(menuItems.Keys.ToList()[1]))
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

        }

        public void PaintBox(string? message, int width)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            for(int i = 0; i < 15; i++)
            {
                Console.SetCursorPosition(9, Console.GetCursorPosition().Top + 1);
                Console.Write(i == 1 ? $"{new string(' ', (width - message.Length)/2)}{message}{new string(' ', (width - message.Length) / 2)}" : $"{new string(' ', width)}");

            }
            Console.ResetColor();
        }
    }
}
