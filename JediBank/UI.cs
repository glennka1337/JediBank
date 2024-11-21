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
            Console.WriteLine(" Enter your Username: ");
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
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine();
            return input;
        }
        public string ReadPassword()
        {
            Console.WriteLine(" Enter your pin code: ");
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
                else if(key == ConsoleKey.Backspace)
                {
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine();
            return input;
        }

        public int Menu(string[] items)
        {
            ConsoleKey key;
            int currentSelection = 0;
            do
            {
                Console.Clear();
                for (int i = 0; i < items.Length; i++)
                {
                    if(i == currentSelection)
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

            } while(key != ConsoleKey.Enter);
            return currentSelection;
        }
        public string MainMenu(Dictionary<string, string[]> menuItems)
        {
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
                    for (int i = 0; i < items.Count; i++)
                    {
                        string triangle = headClicked.ContainsKey(items[i]) ? (headClicked[items[i]] ? "▼" : "▲") : "";
                        if (i == currentSelection)
                        {
                            SetColor(menuItems.ContainsKey(items[i]));
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(menuItems.ContainsKey(items[i]) ? $"{items[i]} {new string(' ',maxLength- items[i].Length+4)} {triangle}": $" ◯ {items[i]} {new string(' ', maxLength - items[i].Length+1)}");
                            
                            Console.ResetColor();
                        }
                        else
                        {
                            SetColor(menuItems.ContainsKey(items[i]));
                            Console.WriteLine(menuItems.ContainsKey(items[i]) ? $"{items[i]} {new string(' ', maxLength - items[i].Length+4)} {triangle}" : $" ◯ {items[i]} {new string(' ', maxLength - items[i].Length + 1)}");
                            
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

    }
}
