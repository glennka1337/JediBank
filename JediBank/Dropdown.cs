using System;
using System.Runtime.InteropServices;

namespace JediBank
{
    internal class Dropdown : Button
    {
		public string? SetText {get; set;} = default;
		public bool IsOpen {get; set;} = false;
		public List<SubOptions> subOptions {get; set;} = new List<SubOptions>();

	/*
		public Dropdown(int x, int y, User? user)
		{
			X = X;
			Y = Y;
			if(user != default)
			{
				foreach(var account in user.Accounts)
				{
					subOptions.Add(new SubOptions{ Text = $" {account.Name} : {account.Balance} " });
				}
			}
			
		}
		*/
		public void PositionSubOptions()
		{
			foreach(var option in subOptions)
			{
				option.X = X + 2;
				option.Y = Y + subOptions.IndexOf(option) + 1; 
				option.ParentDropdown = this;
			}
		}
        public override void Paint()
        {
			Console.BackgroundColor = IsSelected ?  ConsoleColor.White : ConsoleColor.Green;
            Console.ForegroundColor = IsSelected ?  ConsoleColor.Black : ConsoleColor.White;
			Console.SetCursorPosition(X,Y);
			if(SetText == default)
			{
            	Console.Write(IsOpen ? Text+" ▼ " : Text+" ▲ " );

			}
			else
			{
				Console.Write($" {SetText} ");
			}
			Console.ResetColor();
        }
    }
}
