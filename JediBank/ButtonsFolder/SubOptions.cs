using System;

namespace JediBank.ButtonsFolder
{
    internal class SubOptions : Dropdown
    {
        public Dropdown ParentDropdown { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public string Option { get; set; }

        public override void Paint()
        {
            Console.BackgroundColor = IsSelected ? ConsoleColor.White : ConsoleColor.Blue;
            Console.ForegroundColor = IsSelected ? ConsoleColor.Black : ConsoleColor.White;
            Console.SetCursorPosition(X, Y);
            string addLength = new string(' ', Math.Abs(ParentDropdown.Width - Option.Length-1)); 
            Console.Write($" {Option} {addLength}");
            Console.ResetColor();
        }

        public override void Click()
        {
            ParentDropdown.SetText = Option;
            ParentDropdown.IsOpen = !ParentDropdown.IsOpen;
            //Console.WriteLine("du valde detta alternativ");
        }

    }
}
