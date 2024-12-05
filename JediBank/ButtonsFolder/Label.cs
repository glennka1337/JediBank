using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace JediBank.ButtonsFolder
    {
        internal class Label : Button
        {
            public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;
            public ConsoleColor ForeColor { get; set; } = ConsoleColor.White;
            public bool OnlyDigits { get; set; } = true;

            public override void Paint()
            {
                Console.BackgroundColor = BackColor;
                Console.ForegroundColor = ForeColor;
                Console.SetCursorPosition(X, Y);
                Console.Write($"{Text}");
                Console.ResetColor();
            }

            public override void Click()
            {

                
            }
        }
}


