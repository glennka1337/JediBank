using System;

namespace JediBank.ButtonsFolder
{
    public class ClickButton : Button
    {
        public ConsoleColor BackColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor ForeColor { get; set; } = ConsoleColor.White;
        public bool center { get; set; } = true;

        public override void Paint()
        {
            Console.BackgroundColor = IsSelected ? ConsoleColor.White : BackColor;
            Console.ForegroundColor = IsSelected ? ConsoleColor.Black : ForeColor;
            Console.SetCursorPosition(X, Y);
            if(Width > 0 && center)
            {
                string space = new string(' ', (Math.Abs(Width-Text.Length)/2));
                Console.Write($"{space}{Text}{space}");

            }
            else if (!center)
            {
                string addLength = new string(' ', Math.Abs(Width - Text.Length));
                Console.Write($" {Text}{addLength}");
            }
            else
            {
                Console.Write($" {Text} ");
            }
            Console.ResetColor();
        }

        public override void Click()
        {
            //Console.WriteLine("Du klickade på clickbutton");
        }
    }
}