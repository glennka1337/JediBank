using System;
using System.Collections.Generic;
using System.Reflection;
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
        public string RunMainWindow(Dictionary<string, string[]> menuItems, string? message)
        {
            int width = 26;
            int height = 15;//Buttons.Max(b => b.Y);
            int posX = (Console.WindowWidth - width) / 2;
            int posY = (Console.WindowHeight - height) / 2;
            List<Button> Buttons = new List<Button>();
            int index = 0;
            foreach (var item in menuItems) 
            {
                Buttons.Add(new Dropdown
                {
                    X = posX + 3,
                    Y = posY + 5 + index,
                    Text = item.Key,
                    subOptions = ArrayToSubOp(item.Value)
                });
                index++;
            }
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            do
            {
                
                do
                {
                    Console.Clear();
                    Bank bank = new Bank();
                    bank.DisplayLogo();
                    PaintBox(message, width, height, posX, posY);
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
                    HandleMoveAction(ref Buttons, ref currentSelection, key);

                } while (key != ConsoleKey.Enter);
                HandleDropdown(ref Buttons, ref currentSelection);
                if (Buttons[currentSelection] is  Dropdown dropdown) 
                { 
                    if(dropdown is SubOptions option) 
                    {
                        return option.Option;
                    }
                    for(int i = 0; i < Buttons.Count-1; i++) 
                    {
                        Buttons[i + 1].Y = Buttons[i].Y + 1; 
                    }
                }     
            }while(key != ConsoleKey.Escape );
            return "Log out";
        } 
        public Dictionary<decimal?, Account[]> RunLoanWindow(User user)
        {
            int width = 26;
            int height = 15;//Buttons.Max(b => b.Y);
            int posX = (Console.WindowWidth - width) / 2;
            int posY = (Console.WindowHeight - height) / 2;
            Account? Sender = null;
            Account? Receiver = null;
            decimal? amount = -1;
            Dictionary<decimal?, Account[]> initialOutput = new Dictionary<decimal?, Account[]>{
                    { -1, new Account[] { null}  }
                };

            List<Button> Buttons = new List<Button>
            {
                new Dropdown
                {
                    X = posX + 3,
                    Y = posY + 5,
                    Name = "Sender",
                    Text = "Välj konto",
                    subOptions = ToOpList(user)

                },
                new TextBox
                {
                    X = posX + 3,
                    Y = posY + 10,
                    Text = "Belopp",
                    Rubric = "Ange belopp",
                    Width = 10
                },
                new ClickButton
                {
                    X = posX +  width/5,
                    Y = posY +  height-1,
                    Text = "Cancel",
                    BackColor = ConsoleColor.Red,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/5 * 3,
                    Y = posY +  height-1,
                    Text = "Submit",
                    BackColor = ConsoleColor.Green,
                    ForeColor = ConsoleColor.White
                }

            };
            
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            do
            {

                do
                {
                    Console.Clear();
                    Bank bank = new Bank();
                    bank.DisplayLogo();
                    PaintBox("Loan", width, height, posX, posY);
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
                    HandleMoveAction(ref Buttons, ref currentSelection, key);

                } while (key != ConsoleKey.Enter);

                //Dictionary<decimal?, Account[]> output = new ;
                HandleButtonAction(
                    ref Buttons,
                    ref currentSelection,
                    ref Sender,
                    ref Receiver,
                    ref amount,
                    user,
                    out Dictionary<decimal?, Account[]> output);
                if (!output.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null)))
                {
                    // Exiting method with complete state; action performed elsewhere
                    return output;
                }
                else if (output.Any(kvp => kvp.Key == -9))
                {
                    return initialOutput;
                }

            } while (key != ConsoleKey.Escape);
            return initialOutput;
        }
        public Dictionary<decimal?, Account[]> RunWithdrawWindow(User user) 
        {
            int width = 26;
            int height = 15;//Buttons.Max(b => b.Y);
            int posX = (Console.WindowWidth - width) / 2;
            int posY = (Console.WindowHeight - height) / 2;
            Account? Sender = null;
            Account? Receiver = null;
            decimal? amount = -1;
            Dictionary<decimal?, Account[]> initialOutput = new Dictionary<decimal?, Account[]>{
                { -1, new Account[] { null}  }
            };

            List<Button> Buttons = new List<Button>
            {
                new Dropdown
                {
                    X = posX + 3,
                    Y = posY + 5,
                    Name = "Sender",
                    Text = "Välj konto",
                    subOptions = ToOpList(user)

                },
                new TextBox
                {
                    X = posX + 3,
                    Y = posY + 7,
                    Text = "Belopp",
                    Rubric = "Ange belopp",
                    Width = 10
                },
                new ClickButton
                {
                    X = posX + width/5,
                    Y = posY+ height-1,
                    Text = "Cancel",
                    BackColor = ConsoleColor.Red,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/5*3,
                    Y = posY + height-1,
                    Text = "Submit",
                    BackColor = ConsoleColor.Green,
                    ForeColor = ConsoleColor.White
                }

            };
            //int height = Buttons.Max(b => b.Y);
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            do
            {

                do
                {
                    Console.Clear();
                    Bank bank = new Bank();
                    bank.DisplayLogo();
                    PaintBox("Withdraw", width, height, posX, posY);
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
                    HandleMoveAction(ref Buttons, ref currentSelection, key);

                } while (key != ConsoleKey.Enter);
                HandleButtonAction(
                    ref Buttons,
                    ref currentSelection,
                    ref Sender,
                    ref Receiver,
                    ref amount,
                    user,
                    out Dictionary<decimal?, Account[]> output);
                if (!output.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null)))
                {
                    // Exiting method with complete state; action performed elsewhere
                    return output;
                }
                else if (output.Any(kvp => kvp.Key == -9))
                {
                    return initialOutput;
                }

            } while (key != ConsoleKey.Escape);
            return initialOutput;
        }
        public Dictionary<decimal?, Account[]> RunTransferWindow(User user, List<User> Users)
        {
            int width = 26;
            int height = 15;//Buttons.Max(b => b.Y);
            int posX = (Console.WindowWidth - width) / 2;
            int posY = (Console.WindowHeight - height) / 2;
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
                    X = posX+ 3,
                    Y = posY + 3,
                    Name = "Sender",
                    Text = "Välj konto",
                    subOptions = ToOpList(user)

                },
                new Dropdown
                {
                    X = posX+3,
                    Y = posY+7,
                    Name = "Receiver",
                    Text = "Välj mottagare",
                    subOptions = ToOpList(user)

                },
                new TextBox
                {
                    X = posX+3,
                    Y = posY+11,
                    Text = "Kontonummer",
                    Rubric = "Ange mottagare",
                    Width = 10
                },
                new TextBox
                {
                    X = posX+3,
                    Y = posY+14,
                    Text = "Belopp",
                    Rubric = "Ange belopp",
                    Width = 10
                },
                new ClickButton
                {
                    X = posX + width/5,
                    Y = posY + height+1,
                    Text = "Cancel",
                    BackColor = ConsoleColor.Red,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/5*3,
                    Y = posY + height+1,
                    Text = "Submit",
                    BackColor = ConsoleColor.Green,
                    ForeColor = ConsoleColor.White
                }
                
            };
            //int height = Buttons.Max(b => b.Y );
            
                
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            do
            {
                
                do
                {
                    Console.Clear();
                    Bank bank = new Bank();
                    bank.DisplayLogo();
                    PaintBox("Transfer", width, height, posX, posY);
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
                    HandleMoveAction(ref Buttons,ref currentSelection, key);
                    

                } while (key != ConsoleKey.Enter);
                HandleButtonAction(
                    ref Buttons,
                    ref currentSelection,
                    ref Sender,
                    ref Receiver,
                    ref amount,
                    user,
                    out Dictionary<decimal?, Account[]> output);
                if (!output.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null)))
                {
                    // Exiting method with complete state; action performed elsewhere
                    return output;
                } 
                else if(output.Any(kvp => kvp.Key == -9)) 
                {
                    return initialOutput;
                }
            }while(key != ConsoleKey.Escape );
            return initialOutput;
        } 
        public Account RunCreateAccountWindow() 
        {
            int width = 26;
            int height = 15;//Buttons.Max(b => b.Y);
            int posX = (Console.WindowWidth - width) / 2;
            int posY = (Console.WindowHeight - height) / 2;
            bool submit = false;
            Account newAccount = new Account();
            List<Button> Buttons = new List<Button>
            {
                new Dropdown
                {
                    X = posX + 3,
                    Y = posY + 5,
                    Text = "Type of account",
                    subOptions = ArrayToSubOp(["Betalkonto", "Sparkonto"])
                },
                new TextBox
                {
                    X = posX + 3,
                    Y = posY + 10,
                    Text = " ",
                    Rubric = "Kontonamn",
                    Width = 10
                },
                new ClickButton
                {
                    X = posX +  width/5,
                    Y = posY +  height-1,
                    Text = "Cancel",
                    BackColor = ConsoleColor.Red,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/5 * 3,
                    Y = posY +  height-1,
                    Text = "Submit",
                    BackColor = ConsoleColor.Green,
                    ForeColor = ConsoleColor.White
                }

            };
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            ConsoleKey key;
            do
            {

                do
                {
                    Console.Clear();
                    Bank bank = new Bank();
                    bank.DisplayLogo();
                    PaintBox("Loan", width, height, posX, posY);
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
                    HandleMoveAction(ref Buttons, ref currentSelection, key);

                } while (key != ConsoleKey.Enter);

                //Dictionary<decimal?, Account[]> output = new ;
                HandleCreateAccountAction(ref Buttons, ref currentSelection, ref newAccount, ref submit);
                if (newAccount != null && submit)
                {
                    // Exiting method with complete state; action performed elsewhere
                    return newAccount;
                }
                else if (newAccount == null)
                {
                    return null;
                }

            } while (key != ConsoleKey.Escape);
            return null;
        }
        private void HandleButtonAction(
            ref List<Button> Buttons,
            ref int currentSelection,
            ref Account? Sender,
            ref Account? Receiver,
            ref decimal? amount,
            User user,
            out Dictionary<decimal?, Account[]> output)
        {
            output = new Dictionary<decimal?, Account[]>();
            output.Add(-1, new Account[] { null, null });
            var currentButton = Buttons[currentSelection];

            if (currentButton is Dropdown selectedDropdown)
            {
                if (currentButton is SubOptions selectedOption)
                {
                    selectedOption.Click();
                    currentSelection = Buttons.IndexOf(selectedOption.ParentDropdown);
                    if (selectedOption.ParentDropdown.Name == "Sender")
                    {
                        Sender = user.Accounts.Find(x => x.Name == selectedOption.Option);
                    }
                    else if (selectedOption.ParentDropdown.Name == "Receiver")
                    {
                        Receiver = user.Accounts.Find(x => x.Name == selectedOption.Option);
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
                currentButton.Click();
                if (currentButton is TextBox input)
                {
                    if (!string.IsNullOrEmpty(input.Text))
                    {
                        amount = Convert.ToDecimal(input.Text);
                    }
                }
                else if (currentButton is ClickButton clickButton)
                {
                    if (clickButton.Text == "Submit")
                    {
                        output.Clear(); // Rensa tidigare data
                        output.Add(amount, new Account[] { Sender, Receiver });
                        if (!output.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null)))
                        {
                            // Exiting method with complete state; action performed elsewhere
                            return;
                        }
                    }
                    else if (clickButton.Text == "Cancel")
                    {
                        // Reset or perform cancelation; return flow managed in the caller
                        output.Clear();
                        output.Add(-9, new Account[] { null, null });
                    }
                }
            }
        }
        public void HandleDropdown(ref List<Button> Buttons,
            ref int currentSelection) 
        {
            var currentButton = Buttons[currentSelection];
            if (currentButton is Dropdown selectedDropdown)
            {
                if (currentButton is SubOptions selectedOption)
                {
                    selectedOption.Click();
                    return;
                    
                    //Buttons.RemoveRange(currentSelection + 1, selectedOption.ParentDropdown.subOptions.Count);
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
            return;
           
        }
        public void HandleCreateAccountAction(ref List<Button> Buttons, ref int currentSelection, ref Account newAccount, ref bool submit) 
        {
            var currentButton = Buttons[currentSelection];

            if (currentButton is Dropdown selectedDropdown)
            {
                if (currentButton is SubOptions selectedOption)
                {
                    selectedOption.Click();
                    newAccount.IsCheckingaccount = selectedOption.Option == "Betalkonto" ? "Betalkonto" : "Sparkonto";
                    
                    // Close dropdown
                    Buttons.RemoveRange(Buttons.IndexOf(selectedOption.ParentDropdown) + 1, selectedOption.ParentDropdown.subOptions.Count);
                    currentSelection = Buttons.IndexOf(selectedOption.ParentDropdown);
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
                currentButton.Click();
                if (currentButton is TextBox input)
                {
                    if (!string.IsNullOrEmpty(input.Text))
                    {
                        newAccount.Name = input.Text;
                    }
                }
                else if (currentButton is ClickButton clickButton)
                {
                    if (clickButton.Text == "Submit")
                    {
                        submit = true;
                        return;
                    }
                    else if (clickButton.Text == "Cancel")
                    {
                        // Reset or perform cancelation; return flow managed in the caller
                        newAccount = null;
                        return;
                    }
                }
            }
        }
        public void HandleMoveAction(
            ref List<Button> Buttons,
            ref int currentSelection,
            ConsoleKey key) 
        {
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
       
        public List<SubOptions> ArrayToSubOp(string[] array)
        {
            List<SubOptions> SubOpList = new List<SubOptions>();
            foreach(var str in array)
            {
                SubOpList.Add(new SubOptions { Option = str });
            }
            return SubOpList;
        }

    }
}
    

    
