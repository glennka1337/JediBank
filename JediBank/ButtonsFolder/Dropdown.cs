using System;
using System.Runtime.InteropServices;

namespace JediBank.ButtonsFolder
{
    internal class Dropdown : Button
    {
        public string? SetText { get; set; } = default;
        public bool IsOpen { get; set; } = false;
        public List<SubOptions> subOptions { get; set; } = new List<SubOptions>();

        public int width { get; set; }
        
        public void PositionSubOptions()
        {
            foreach (var option in subOptions)
            {
                option.X = X + 2;
                option.Y = Y + subOptions.IndexOf(option) + 1;
                option.ParentDropdown = this;
            }
        }
        public override void Paint()
        {
            if(Rubric != default)
            {
                Console.BackgroundColor = ConsoleColor.Black; 
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(X, Y-1);
                Console.Write(Rubric);
            }
            Console.BackgroundColor = IsSelected ? ConsoleColor.White : ConsoleColor.DarkBlue;
            Console.ForegroundColor = IsSelected ? ConsoleColor.Black : ConsoleColor.White;
            Console.SetCursorPosition(X, Y);
            if (SetText == default)
            {
                string addLength = new string(' ', Math.Abs(Width - Text.Length-1));
                Console.Write(IsOpen ?' ' + Text + addLength+ " ▼ " : ' ' + Text + addLength + " ▲ ");

            }
            else
            {

                string addLength = new string(' ', Math.Abs(Width - SetText.Length));
                Console.Write($" {SetText} {addLength}  ✔️ ");
            }
            Console.ResetColor();
        }
    }
}
