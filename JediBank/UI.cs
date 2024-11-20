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
    }
}
