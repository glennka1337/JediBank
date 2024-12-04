using System;

namespace JediBank
{
    public class TextBox : Button
    {

        public override void Paint()
        {
            Console.BackgroundColor = IsSelected ?  ConsoleColor.White : ConsoleColor.Magenta;
            Console.ForegroundColor = IsSelected ?  ConsoleColor.Black : ConsoleColor.White;
            Console.SetCursorPosition(X,Y);
            Console.Write($"{Text} {new string(' ', Math.Abs(Width-Text.Length))}");
            Console.ResetColor();
        }

        public override void Click()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(X + Text.Length,Y);
            string input = Text;
            ConsoleKey key;
            do
            {
                var keyPressed = Console.ReadKey(intercept: true);
                key = keyPressed.Key;
                if (char.IsDigit(keyPressed.KeyChar))
                {
                    if(input.Length < Width)
                    {
                        input += keyPressed.KeyChar;
                        Console.Write(keyPressed.KeyChar);
                    }
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
            Text = input;
            Paint();
        }
    }
}