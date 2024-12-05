using System;
using System.Security.Cryptography.X509Certificates;
using JediBank.ButtonsFolder;

namespace JediBank
{
    internal class Window
    {
        public List<SubOptions> ToOpList(User user)
        {
            List<SubOptions> newList = new List<SubOptions>();
            foreach (var account in user.Accounts)
            {
                newList.Add(new SubOptions {
                    Option = account.Name
                });
            }
            return newList;
        }
        public Dictionary<decimal?, Account[]> RunLoanWindow(User user)
        {

        Language language = new Language();
        int width = 26;
        Account? Sender = null;
        decimal? amount = -1;
        Dictionary<decimal?, Account[]> initialOutput = new Dictionary<decimal?, Account[]>{
                { -1, new Account[] { null}  }
            };

        List<Button> Buttons = new List<Button>
            {
                new Dropdown
                {
                    X = 3,
                    Y = 5,
                    Name = "Sender",
                    Text = language.TranslationTool("Select account"),
                    subOptions = ToOpList(user)

                },
                new TextBox
                {
                    X = 3,
                    Y = 10,
                    Text = "Belopp",
                    Rubric = "Ange belopp",
                    Width = 10
                },
                new ClickButton
                {
                    X = 2,
                    Y = 16,
                    Text = "Cancel",
                    BackColor = ConsoleColor.Red,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = 10,
                    Y = 16,
                    Text = "Submit",
                    BackColor = ConsoleColor.Green,
                    ForeColor = ConsoleColor.White
                }

            };
        int height = Buttons.Max(b => b.Y);
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            do
            {

                do
                {
                    Console.Clear();
                    PaintBox("Loan", width, height, 0, 0);
                    foreach (var button in Buttons)
                    {
                        button.Paint();
                        if (button is Dropdown thisDropdown)
                        {
                            thisDropdown.PositionSubOptions();
                        }
                    }
                    var keyPressed = Console.ReadKey(intercept: true);
                    key = keyPressed.Key;
                    if (key == ConsoleKey.UpArrow)
                    {
                        Buttons[currentSelection].IsSelected = false;
                        currentSelection = currentSelection == 0 ? Buttons.Count - 1 : currentSelection - 1;
                        Buttons[currentSelection].IsSelected = true;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        Buttons[currentSelection].IsSelected = false;
                        currentSelection = currentSelection == Buttons.Count - 1 ? 0 : currentSelection + 1;
                        Buttons[currentSelection].IsSelected = true;
                    }

                } while (key != ConsoleKey.Enter);
                if (Buttons[currentSelection] is Dropdown selectedDropdown)
                {
                    if (Buttons[currentSelection] is SubOptions selectedOption)
                    {
                        selectedOption.Click();
                        currentSelection = Buttons.IndexOf(selectedOption.ParentDropdown);
                        if (selectedOption.ParentDropdown.Name == "Sender")
                        {
                            Sender = user.Accounts.Find(x => x.Name == selectedOption.Option);

                        }

                        // Close dropdown
                        Buttons.RemoveRange(currentSelection + 1, selectedOption.ParentDropdown.subOptions.Count);
                    }
                    selectedDropdown.IsOpen = !selectedDropdown.IsOpen;
                    if (selectedDropdown.IsOpen)
                    {
                        Buttons.InsertRange(currentSelection + 1, selectedDropdown.subOptions);
                    }
                    else
                    {
                        Buttons.RemoveRange(currentSelection + 1, selectedDropdown.subOptions.Count);
                    }
                }
                else
                {
                    Buttons[currentSelection].Click();
                    if (Buttons[currentSelection] is TextBox input)
                    {
                        //Buttons.Find(x => user.Accounts.x.Text)
                        if (input.Text.Length != 0)
                        {
                            amount = Convert.ToDecimal(input.Text);

                        }
                    }
                    else if (Buttons[currentSelection] is ClickButton clickButton)
                    {
                        if (clickButton.Text == "Submit")
                        {
                            Dictionary<decimal?, Account[]> output = new Dictionary<decimal?, Account[]>
                            {
                                { amount, new Account[] { Sender} }
                            };
                            if (!output.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null)))
                            {
                                return output;
                            }
                        }
                        else if (clickButton.Text == "Cancel")
                        {
                            return initialOutput;
                        }

                    }

                }


            } while (key != ConsoleKey.Escape);
            return initialOutput;
        }
        public Dictionary<decimal?, Account[]> RunWithdrawWindow(User user) 
        {
            int width = 26;
            Account? Sender = null;
            decimal? amount = -1;
            Dictionary<decimal?, Account[]> initialOutput = new Dictionary<decimal?, Account[]>{
                { -1, new Account[] { null}  }
            };

            List<Button> Buttons = new List<Button>
            {
                new Dropdown
                {
                    X = 3,
                    Y = 5,
                    Name = "Sender",
                    Text = "Välj konto",
                    subOptions = ToOpList(user)

                },
                new TextBox
                {
                    X = 3,
                    Y = 10,
                    Text = "Belopp",
                    Rubric = "Ange belopp",
                    Width = 10
                },
                new ClickButton
                {
                    X = 2,
                    Y = 16,
                    Text = "Cancel",
                    BackColor = ConsoleColor.Red,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = 10,
                    Y = 16,
                    Text = "Submit",
                    BackColor = ConsoleColor.Green,
                    ForeColor = ConsoleColor.White
                }

            };
            int height = Buttons.Max(b => b.Y);
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            do
            {

                do
                {
                    Console.Clear();
                    PaintBox("Transfer", width, height, 0, 0);
                    foreach (var button in Buttons)
                    {
                        button.Paint();
                        if (button is Dropdown thisDropdown)
                        {
                            thisDropdown.PositionSubOptions();
                        }
                    }
                    var keyPressed = Console.ReadKey(intercept: true);
                    key = keyPressed.Key;
                    if (key == ConsoleKey.UpArrow)
                    {
                        Buttons[currentSelection].IsSelected = false;
                        currentSelection = currentSelection == 0 ? Buttons.Count - 1 : currentSelection - 1;
                        Buttons[currentSelection].IsSelected = true;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        Buttons[currentSelection].IsSelected = false;
                        currentSelection = currentSelection == Buttons.Count - 1 ? 0 : currentSelection + 1;
                        Buttons[currentSelection].IsSelected = true;
                    }

                } while (key != ConsoleKey.Enter);
                if (Buttons[currentSelection] is Dropdown selectedDropdown)
                {
                    if (Buttons[currentSelection] is SubOptions selectedOption)
                    {
                        selectedOption.Click();
                        currentSelection = Buttons.IndexOf(selectedOption.ParentDropdown);
                        if (selectedOption.ParentDropdown.Name == "Sender")
                        {
                            Sender = user.Accounts.Find(x => x.Name == selectedOption.Option);

                        }

                        // Close dropdown
                        Buttons.RemoveRange(currentSelection + 1, selectedOption.ParentDropdown.subOptions.Count);
                    }
                    selectedDropdown.IsOpen = !selectedDropdown.IsOpen;
                    if (selectedDropdown.IsOpen)
                    {
                        Buttons.InsertRange(currentSelection + 1, selectedDropdown.subOptions);
                    }
                    else
                    {
                        Buttons.RemoveRange(currentSelection + 1, selectedDropdown.subOptions.Count);
                    }
                }
                else
                {
                    Buttons[currentSelection].Click();
                    if (Buttons[currentSelection] is TextBox input)
                    {
                        //Buttons.Find(x => user.Accounts.x.Text)
                        if(input.Text.Length != 0) 
                        {
                            amount = Convert.ToDecimal(input.Text);
                        
                        }
                    }
                    else if (Buttons[currentSelection] is ClickButton clickButton)
                    {
                        if (clickButton.Text == "Submit")
                        {
                            Dictionary<decimal?, Account[]> output = new Dictionary<decimal?, Account[]>
                            {
                                { amount, new Account[] { Sender} }
                            };
                            if (!output.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null)))
                            {
                                return output;
                            }
                        }
                        else if(clickButton.Text == "Cancel")
                        {
                            return initialOutput;
                        }

                    }

                }


            } while (key != ConsoleKey.Escape);
            return initialOutput;
        }
        public Dictionary<decimal?, Account[]> RunTransferWindow(User user, List<User> Users)
        {
            int width = 26; 
            Account? Sender = null;
            Account? Receiver = null;
            decimal? amount = -1;
            Dictionary<decimal?, Account[]> initialOutput = new Dictionary<decimal?, Account[]>{
                { amount, new Account[] { Sender, Receiver } }
            }; 
                     
            List<Button> Buttons = new List<Button>
            {
                new Dropdown
                {
                    X = 3,
                    Y = 3,
                    Name = "Sender",
                    Text = "Välj konto",
                    subOptions = ToOpList(user)

                },
                new Dropdown
                {
                    X = 3,
                    Y = 7,
                    Name = "Receiver",
                    Text = "Välj mottagare",
                    subOptions = ToOpList(user)

                },
                new TextBox
                {
                    X = 3,
                    Y = 11,
                    Text = "Kontonummer",
                    Rubric = "Ange mottagare",
                    Width = 10
                },
                new TextBox
                {
                    X = 3,
                    Y = 14,
                    Text = "Belopp",
                    Rubric = "Ange belopp",
                    Width = 10
                },
                new ClickButton
                {
                    X = 2,
                    Y = 16,
                    Text = "Cancel",
                    BackColor = ConsoleColor.Red,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = 10,
                    Y = 16,
                    Text = "Submit",
                    BackColor = ConsoleColor.Green,
                    ForeColor = ConsoleColor.White
                }
                
            };
            int height = Buttons.Max(b => b.Y );
            
                
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            do
            {
                
                do
                {
                    Console.Clear();
                    PaintBox("Transfer", width, height, 0, 0);
                    foreach(var button in Buttons)
                    {
                        button.Paint();
                        if(button is Dropdown thisDropdown)
                        {
                            thisDropdown.PositionSubOptions();
                        }
                    }
                    var keyPressed = Console.ReadKey(intercept: true);
                    key = keyPressed.Key;
                    if (key == ConsoleKey.UpArrow)
                    {
                        Buttons[currentSelection].IsSelected = false;
                        currentSelection = currentSelection == 0 ? Buttons.Count - 1 : currentSelection - 1;
                        Buttons[currentSelection].IsSelected = true;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        Buttons[currentSelection].IsSelected = false;
                        currentSelection = currentSelection == Buttons.Count - 1 ? 0 : currentSelection + 1;
                        Buttons[currentSelection].IsSelected = true;
                    }

                } while (key != ConsoleKey.Enter);
                if(Buttons[currentSelection] is Dropdown selectedDropdown)
                {
                    if(Buttons[currentSelection] is SubOptions selectedOption)
                    {
                        selectedOption.Click();
                        currentSelection = Buttons.IndexOf(selectedOption.ParentDropdown);
                        if(selectedOption.ParentDropdown.Name == "Sender")
                        {
                            Sender = user.Accounts.Find(x => x.Name == selectedOption.Option);
                            
                        }
                        else
                        {
                            Receiver = user.Accounts.Find(x => x.Name == selectedOption.Option);
                        }
                        
                        // Close dropdown
                        Buttons.RemoveRange(currentSelection +1, selectedOption.ParentDropdown.subOptions.Count);
                    }
                    selectedDropdown.IsOpen = !selectedDropdown.IsOpen;
                    if(selectedDropdown.IsOpen)
                    {
                        Buttons.InsertRange(currentSelection+1, selectedDropdown.subOptions);
                    }
                    else
                    {
                        Buttons.RemoveRange(currentSelection+1, selectedDropdown.subOptions.Count);
                    }
                }
                else
                {
                    Buttons[currentSelection].Click();
                    if(Buttons[currentSelection] is TextBox input)
                    {       
                        //Buttons.Find(x => user.Accounts.x.Text)
                        amount = Convert.ToDecimal(input.Text);
                    }
                    else if(Buttons[currentSelection] is ClickButton clickButton && clickButton.Text == "Submit")
                    {       
                        Dictionary<decimal?, Account[]> output = new Dictionary<decimal?, Account[]>
                        {
                            { amount, new Account[] { Sender, Receiver } }
                        }; 
                        if(!output.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null)))
                        {
                            return output;
                        }
                        
                    }

                }

                    
            }while(key != ConsoleKey.Escape );
            return initialOutput;
        } 
        public void PaintBox(string? message, int width, int height, int X, int Y)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            int center = width - message.Length;
            for (int i = 0; i < height+3; i++)
            {
                Console.SetCursorPosition(X, Y + i);
                Console.Write(i == 1 ? $"{new string(' ', (center/2))}{message}{new string(' ', (center / 2 + (center % 2)))}" : $"{new string(' ', width)}");

            }
            Console.ResetColor();
        }


    }
}
    

    
