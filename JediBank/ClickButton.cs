using System;

namespace JediBank
{
    public class ClickButton : Button
    {
        public ConsoleColor BackColor {get; set;} = ConsoleColor.Red;
        public ConsoleColor ForeColor {get; set;} = ConsoleColor.White;

        public override void Paint()
        {
            Console.BackgroundColor = IsSelected ?  ConsoleColor.White : BackColor;
            Console.ForegroundColor = IsSelected ?  ConsoleColor.Black : ForeColor;
            Console.SetCursorPosition(X,Y);
            Console.Write(Text);
            Console.ResetColor();
        }

        public override void Click()
        {
            System.Console.WriteLine("Du klickade på clickbutton");
        }
    }
}