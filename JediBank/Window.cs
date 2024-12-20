using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using JediBank.ButtonsFolder;
using JediBank.CurrencyFolder;

namespace JediBank
{
    
    internal class Window
    {
        public int width {get; set;} = 52;
        public int height {get; set;} = 20;
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
        //int startY = 6;
        public string RunLoginWindow()
        {
            Language language = new Language(Program.ChoosenLangugage);
            
            int posX = (Console.WindowWidth - width) / 2;
            int posY = 7;// (Console.WindowHeight - height) / 2;
            List<Button> Buttons = new List<Button>
            {
                new ClickButton
                {
                    X = posX + (width-20)/2,
                    Y = posY + 5,
                    Width = 22,
                    center = false,
                    Text = language.TranslationTool("Login"),
                    BackColor = ConsoleColor.DarkBlue,
                    ForeColor = ConsoleColor.White

                },
                new ClickButton
                {
                    X = posX + (width-20)/2,
                    Y = posY + 6,
                    Width = 22,
                    center = false,
                    Text = language.TranslationTool("Exit"),
                    BackColor = ConsoleColor.DarkBlue,
                    ForeColor = ConsoleColor.White

                },
                new Dropdown
                {
                    X = posX + (width-20)/2,
                    Y = posY + 7,
                    Width = 20,
                    Text = language.TranslationTool("Language"),
                    subOptions = ArrayToSubOp(["Svenska", "English"])// Add emojis (country flag)

                }

            };
            
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            Bank bank = new Bank();
            bank.DisplayLogo();
            do
            {
                PaintBox(language.TranslationTool("Login"), width, height, posX, posY);
                do
                {
                    //Console.Clear();
                    
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
                        Program.ChoosenLangugage = option.Option;
                        return option.Option;
                    }
                    for(int i = 0; i < Buttons.Count-1; i++) 
                    {
                        Buttons[i + 1].Y = Buttons[i].Y + 1; 
                    }
                }
                else if(Buttons[currentSelection] is  ClickButton Clicked)
                {
                    return Clicked.Text;
                }     
            }while(key != ConsoleKey.Escape );
            return language.TranslationTool("Exit");
        }
        public string RunMainWindow(Dictionary<string, string[]> menuItems, string? message)
        {
            Language language = new Language(Program.ChoosenLangugage);
            
            int posX = (Console.WindowWidth - width) / 2;
            int posY = 7;//(Console.WindowHeight - height) / 2;
            List<Button> Buttons = new List<Button>();
            int index = 0;
            foreach (var item in menuItems) 
            {
                Buttons.Add(new Dropdown
                {
                    X = posX + width / 3,
                    Y = posY + 4 + index,
                    Text = item.Key,
                    Width = 18,
                    subOptions = ArrayToSubOp(item.Value)
                });
                index++;
            }
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            Bank bank = new Bank();
            bank.DisplayLogo();
            do
            {

                PaintBox(language.TranslationTool("Home"), width, height, posX, posY);
                do
                {
                    //Console.Clear();
                    
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
            return language.TranslationTool("Log out");
        } 
        public Dictionary<decimal?, Account[]> RunLoanWindow(User user)
        {
            Language language = new Language(Program.ChoosenLangugage);
            Loan newLoan = new Loan();
            int posX = (Console.WindowWidth - width) / 2;
            int posY = 7;// (Console.WindowHeight - height) / 2;
            Account? Sender = null;
            Account? Receiver = new Account();
            decimal? amount = 0;
            Dictionary<decimal?, Account[]> initialOutput = new Dictionary<decimal?, Account[]>{
                    { -1, new Account[] { null}  }
                };

            List<Button> Buttons = new List<Button>
            {
                new Dropdown
                {
                    X = posX + (width-20)/2,
                    Y = posY + 6,
                    Width = 20,
                    Name = "Sender",
                    Rubric = language.TranslationTool("Select account"),
                    Text = language.TranslationTool("Account"),
                    subOptions = ToOpList(user)

                },
                new TextBox
                {
                    X = posX + (width-20)/2,
                    Y = posY+ height-5,
                    Text = language.TranslationTool("Amount"),
                    Rubric = language.TranslationTool("Enter amount"),
                    Width = 20
                },
               new ClickButton
                {
                    X = posX + 3,//width/5,
                    Y = posY+ height-2,
                    Width = width/2 - 3,
                    Name = "Cancel",
                    Text = language.TranslationTool("Cancel"),
                    BackColor = ConsoleColor.DarkRed,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/2+1,
                    Y = posY + height-2,
                    Width = width/2-3,
                    Name = "Submit",
                    Text = language.TranslationTool("Submit"),
                    BackColor = ConsoleColor.DarkGreen,
                    ForeColor = ConsoleColor.White
                }

            };
            
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            Bank bank = new Bank();
            bank.DisplayLogo();
            do
            {
                PaintBox(language.TranslationTool("Loan"), width, height, posX, posY);
                newLoan.LoanAmount = (decimal)amount;

                do
                {
                    //Console.Clear();
                    
                    
                    foreach (var button in Buttons)
                    {
                        if (button is Dropdown thisDropdown)
                        {
                            newLoan.CalculateInterest();
                            thisDropdown.Rubric = language.TranslationTool("Interest on loan:")+$"{newLoan.Interest*100 - 100}%";
                            thisDropdown.PositionSubOptions();
                        }
                        button.Paint();
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
            Language language = new Language(Program.ChoosenLangugage);
            int posX = (Console.WindowWidth - width) / 2;
            int posY = 7;// (Console.WindowHeight - height) / 2;
            Account? Sender = null;
            Account Receiver = user.Accounts[0];
            decimal? amount = -1;
            Dictionary<decimal?, Account[]> initialOutput = new Dictionary<decimal?, Account[]>{
                { -1, new Account[] { null}  }
            };

            List<Button> Buttons = new List<Button>
            {
                new Dropdown
                {
                    X = posX + (width-20)/2,
                    Y = posY + 5,
                    Name = "Sender",
                    Width = 20,
                    Text = language.TranslationTool("Select account"),
                    subOptions = ToOpList(user)

                },
                new TextBox
                {
                    X = posX + (width-20)/2,
                    Y = posY + height-4,
                    Text = language.TranslationTool("Amount"),
                    Rubric = language.TranslationTool("Enter amount"),
                    Width = 20
                },
                new ClickButton
                {
                    X = posX + 3,//width/5,
                    Y = posY+ height-2,
                    Width = width/2 - 3,
                    Name = "Cancel",
                    Text = language.TranslationTool("Cancel"),
                    BackColor = ConsoleColor.DarkRed,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/2+1,
                    Y = posY + height-2,
                    Width = width/2-3,
                    Name = "Submit",
                    Text = language.TranslationTool("Submit"),
                    BackColor = ConsoleColor.DarkGreen,
                    ForeColor = ConsoleColor.White
                }

            };
            //int height = Buttons.Max(b => b.Y);
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;

            Bank bank = new Bank();
            bank.DisplayLogo();
            do
            {
                PaintBox(language.TranslationTool("Withdraw"), width, height, posX, posY);
                do
                {
                    //Console.Clear();
                    
                    
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
        public Dictionary<decimal?, Account[]> RunExternalTransferWindow(User user, List<User> Users)
        {
            Language language = new Language(Program.ChoosenLangugage);
            
            int posX = (Console.WindowWidth - width) / 2;
            int posY = 7;// (Console.WindowHeight - height) / 2;
            Account? Sender = null;
            Account? Receiver = null;
            string accountID;
            decimal? amount = -1;
            Dictionary<decimal?, Account[]> initialOutput = new Dictionary<decimal?, Account[]>{
                { amount, new Account[] { Sender, Receiver } }
            };

            List<Button> Buttons = new List<Button>
            {
                new Dropdown
                {
                    X = posX + 3,
                    Y = posY + 5,
                    Width = 20,
                    Name = "Sender",
                    Text = language.TranslationTool("Account"),
                    Rubric = language.TranslationTool("Select account"),
                    subOptions = ToOpList(user)

                },
                new TextBox
                {
                    X = posX+width/2+1,
                    Y = posY+5,
                    Name = "Receiver",
                    Text = language.TranslationTool("Account number"),
                    Rubric = language.TranslationTool("Enter recipient"),
                    Width = 20,
                    OnlyDigits = false
                },
                new TextBox
                {
                    X = posX+width/2+1,
                    Y = posY+9,
                    Name = "Amount",
                    Text = language.TranslationTool("Amount"),
                    Rubric = language.TranslationTool("Enter amount"),
                    Width = 20
                },
                new ClickButton
                {
                    X = posX + 3,//width/5,
                    Y = posY+ height-2,
                    Width = width/2 - 3,
                    Name = "Cancel",
                    Text = language.TranslationTool("Cancel"),
                    BackColor = ConsoleColor.DarkRed,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/2+1,
                    Y = posY + height-2,
                    Width = width/2-3,
                    Name = "Submit",
                    Text = language.TranslationTool("Submit"),
                    BackColor = ConsoleColor.DarkGreen,
                    ForeColor = ConsoleColor.White
                }

            };
            //int height = Buttons.Max(b => b.Y );
            
                
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            Bank bank = new Bank();
            bank.DisplayLogo();
            do
            {
                PaintBox(language.TranslationTool("External transfer"), width, height, posX, posY);
                do
                {
                    //Console.Clear();
                    
                    
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
                if (!output.Any(kvp => kvp.Key < 0 || kvp.Value.Any(item => item == null)))
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
        public Dictionary<decimal?, Account[]> RunInternalTransferWindow(User user, List<User> Users)
        {
            Language language = new Language(Program.ChoosenLangugage);
            
            int posX = (Console.WindowWidth - width) / 2;
            int posY = 7;//(Console.WindowHeight - height) / 2;
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
                    X = posX + 3,
                    Y = posY + 5,
                    Width = 20,
                    Name = "Receiver",
                    Text = language.TranslationTool("Select recipient"),
                    subOptions = ToOpList(user)

                },
                new Dropdown
                {
                    X = posX + width/2 +1,
                    Y = posY + 5,
                    Width = 20,
                    Name = "Sender",
                    Text = language.TranslationTool("Select account"),
                    subOptions = ToOpList(user)

                },
                new TextBox
                {
                    X = posX+(width-20)/2,
                    Y = posY+height-5,
                    Text = language.TranslationTool("Amount"),
                    Rubric = language.TranslationTool("Enter amount"),
                    Width = 20
                },
                new ClickButton
                {
                    X = posX + 3,//width/5,
                    Y = posY+ height-2,
                    Width = width/2 - 3,
                    Name = "Cancel",
                    Text = language.TranslationTool("Cancel"),
                    BackColor = ConsoleColor.DarkRed,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/2+1,
                    Y = posY + height-2,
                    Width = width/2-3,
                    Name = "Submit",
                    Text = language.TranslationTool("Submit"),
                    BackColor = ConsoleColor.DarkGreen,
                    ForeColor = ConsoleColor.White
                }

            };
            //int height = Buttons.Max(b => b.Y );


            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            Bank bank = new Bank();
            bank.DisplayLogo();
            do
            {
                PaintBox(language.TranslationTool("Transfer"), width, height, posX, posY);
                do
                {
                    //Console.Clear();
                    
                    
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
        public Account RunCreateAccountWindow() 
        {
            Language language = new Language(Program.ChoosenLangugage);
            
            int posX = (Console.WindowWidth - width) / 2;
            int posY = 7;// (Console.WindowHeight - height) / 2;
            bool submit = false;
            Account newAccount = new Account();
            List<Button> Buttons = new List<Button>
            {
                new Dropdown
                {
                    X = posX + 3,
                    Y = posY + 5,
                    Name = "AccountType",
                    Width = 20,
                    Text = language.TranslationTool("Type of account"),
                    subOptions = ArrayToSubOp(["Betalkonto", "Sparkonto"])
                },
                new Dropdown
                {
                    X = posX + width/2 + 2,
                    Y = posY + 5,
                    Width = 20,
                    Name = "Currency",
                    Text = language.TranslationTool("Type of currency"),
                    subOptions = ArrayToSubOp(["SEK", "USD","EUR"])
                },
                new TextBox
                {
                    X = posX + (width-20)/2,
                    Y = posY + height/2+1,
                    Text = " ",
                    Rubric = language.TranslationTool("Account Name"),
                    Width = 20,
                    OnlyDigits = false
                },
                new ClickButton
                {
                    X = posX + 3,//width/5,
                    Y = posY+ height-2,
                    Width = width/2 - 3,
                    Name = "Cancel",
                    Text = language.TranslationTool("Cancel"),
                    BackColor = ConsoleColor.DarkRed,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/2+1,
                    Y = posY + height-2,
                    Width = width/2-3,
                    Name = "Submit",
                    Text = language.TranslationTool("Submit"),
                    BackColor = ConsoleColor.DarkGreen,
                    ForeColor = ConsoleColor.White
                }

            };
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            ConsoleKey key;
            Bank bank = new Bank();
            bank.DisplayLogo();
            do
            {
                
                PaintBox(language.TranslationTool("Create account"), width, height, posX, posY);
                do
                {
                    //Console.Clear();
                    
                    
                    
                    foreach (var button in Buttons)
                    {
                        button.Paint();
                        if (button is Dropdown thisDropdown)
                        {
                            Console.SetCursorPosition(posX + 3, posY + 4);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(thisDropdown.SetText == "Betalkonto" ? "Interest : 0 %" : (thisDropdown.SetText == "Sparkonto" ? "Interest : 4 %" : "")) ;
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

        public bool RunCreateUserWindow(List<User> Users)
        {
            Language language = new Language(Program.ChoosenLangugage);
            int posX = (Console.WindowWidth - width) / 2;
            int posY = 7;// (Console.WindowHeight - height) / 2;
            string Username = default;
            string Password = default;

            List<Button> Buttons = new List<Button>
            {
                new TextBox
                {
                    X = posX + (width-20)/2,//width/5,
                    Y = posY +  height/2 - 2,
                    Name = "Username",
                    Text = language.TranslationTool("Enter username"),
                    Rubric = language.TranslationTool("Enter username"),
                    OnlyDigits = false,
                    Width=20
                },
                new TextBox
                {
                    X = posX + (width-20)/2,//width/5,
                    Y = posY + height/2 +2,
                    Name = "Password",
                    Text = language.TranslationTool("Enter password"),
                    Rubric = language.TranslationTool("Enter password"),
                    Width=20
                },
                new ClickButton
                {
                    X = posX + 3,//width/5,
                    Y = posY+ height-2,
                    Width = width/2 - 3,
                    Name = "Cancel",
                    Text = language.TranslationTool("Cancel"),
                    BackColor = ConsoleColor.DarkRed,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/2+1,
                    Y = posY + height-2,
                    Width = width/2-3,
                    Name = "Submit",
                    Text = language.TranslationTool("Submit"),
                    BackColor = ConsoleColor.DarkGreen,
                    ForeColor = ConsoleColor.White
                }

            };
            //int height = Buttons.Max(b => b.Y );
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            Bank bank = new Bank();
            bank.DisplayLogo();
            do
            {
                PaintBox(language.TranslationTool("Add user"), width, height, posX, posY);
                do
                {
                    //Console.Clear();


                    foreach (var button in Buttons)
                    {
                        if (button is Dropdown thisDropdown)
                        {
                            thisDropdown.PositionSubOptions();
                        }
                        button.Paint();
                    }
                    var keyPressed = Console.ReadKey(intercept: true);
                    key = keyPressed.Key;
                    HandleMoveAction(ref Buttons, ref currentSelection, key);

                } while (key != ConsoleKey.Enter);
                //Buttons[currentSelection].Click();
                if (Buttons[currentSelection] is TextBox textbox)
                {
                    textbox.Click();
                    if(textbox.Name == "Username")
                    {
                        Username = textbox.Text;

                    }
                    else if (textbox.Name == "Password")
                    {
                        Password = textbox.Text;

                    }

                }
                else if (Buttons[currentSelection].Name == "Submit")
                {
                    if (Username != default && Password != default )
                    {
                        Users.Add(new User {Name = Username, Password = Password});
                        return true;

                    }
                }
                else if (Buttons[currentSelection].Name == "Cancel")
                {
                    return false;
                }

            } while (key != ConsoleKey.Escape);
            return false;
        
        }
        public bool RunRemoveUserWindow(List<User> Users)
        {
            Language language = new Language(Program.ChoosenLangugage);
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
                    X = posX+ (width-20)/2,
                    Y = posY+5,
                    Width = 20,
                    Name = "RemoveUser",
                    Text = language.TranslationTool("Select user to remove"),
                    subOptions = ArrayToSubOp(Users.FindAll(x => x.IsAdmin == false).Select(p => p.Name).ToArray())

                },
                new ClickButton
                {
                    X = posX + 3,//width/5,
                    Y = posY+ height-2,
                    Width = width/2 - 3,
                    Name = "Cancel",
                    Text = language.TranslationTool("Cancel"),
                    BackColor = ConsoleColor.DarkRed,
                    ForeColor = ConsoleColor.White
                },
                new ClickButton
                {
                    X = posX + width/2+1,
                    Y = posY + height-2,
                    Width = width/2-3,
                    Name = "Submit",
                    Text = language.TranslationTool("Submit"),
                    BackColor = ConsoleColor.DarkGreen,
                    ForeColor = ConsoleColor.White
                }

            };
            //int height = Buttons.Max(b => b.Y );
            ConsoleKey key;
            int currentSelection = 0;
            Buttons[currentSelection].IsSelected = true;
            Bank bank = new Bank();
            bank.DisplayLogo();
            do
            {
                PaintBox(language.TranslationTool("Remove user"), width, height, posX, posY);
                do
                {
                    //Console.Clear();
                    
                    
                    foreach (var button in Buttons)
                    {
                        if (button is Dropdown thisDropdown)
                        {
                            thisDropdown.PositionSubOptions();
                        }
                        button.Paint();
                    }
                    var keyPressed = Console.ReadKey(intercept: true);
                    key = keyPressed.Key;
                    HandleMoveAction(ref Buttons, ref currentSelection, key);

                } while (key != ConsoleKey.Enter);

                //Dictionary<decimal?, Account[]> output = new ;
                HandleDropdown(ref Buttons, ref currentSelection);
                if (Buttons[currentSelection].Name == "Submit")
                {
                    if(((Dropdown)Buttons[0]).SetText != default)
                    {
                        Users.Remove(Users.Find(x => x.Name == ((Dropdown)Buttons[0]).SetText));
                        return true;

                    }
                }
                else if (Buttons[currentSelection].Name == "Cancel")
                {
                    return false;
                }

            } while (key != ConsoleKey.Escape);
            return false;
        }
        // Methods for interacting with buttons


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
                    if(input.Name == "Receiver" && !string.IsNullOrEmpty(input.Text))
                    {
                        User? tempUser = Bank.Users.Find(user  => user.Accounts.Any(x => x.AccountId == input.Text));
                        Receiver = tempUser == null ? Receiver : tempUser.Accounts.Find(x => x.AccountId == input.Text);
                    }
                    else if (!string.IsNullOrEmpty(input.Text))
                    {
                        amount = Convert.ToDecimal(input.Text);
                    }
                }
                else if (currentButton is ClickButton clickButton)
                {
                    if (clickButton.Name == "Submit")
                    {
                        output.Clear(); // Rensa tidigare data
                        output.Add(amount, new Account[] { Sender, Receiver });
                        if (!output.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null)))
                        {
                            // Exiting method with complete state; action performed elsewhere
                            return;
                        }
                    }
                    else if (clickButton.Name == "Cancel")
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
                    if(selectedOption.ParentDropdown.Name == "Currency")
                    {
                        switch (selectedOption.Option)
                        {
                            case "SEK":
                                newAccount.Currency = new SEK();
                                break;
                            case "USD":
                                newAccount.Currency = new USD();
                                break;
                            case "EUR":
                                newAccount.Currency = new EUR();
                                break;

                        }
                    }
                    else
                    {
                        newAccount.IsCheckingaccount = selectedOption.Option == "Betalkonto" ? "Betalkonto" : "Sparkonto";
                    }
                    
                    
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
                    if (clickButton.Name == "Submit")
                    {
                        submit = true;
                        return;
                    }
                    else if (clickButton.Name == "Cancel")
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
            
            if (key == ConsoleKey.UpArrow || key == ConsoleKey.LeftArrow)
            {
                Buttons[currentSelection].IsSelected = false;
                currentSelection = currentSelection == 0 ? Buttons.Count - 1 : currentSelection - 1;
                if (Buttons[currentSelection] is Label label)
                {
                    currentSelection = currentSelection == 0 ? Buttons.Count - 1 : currentSelection - 1;
                }
                Buttons[currentSelection].IsSelected = true;
            }
            else if (key == ConsoleKey.DownArrow || key == ConsoleKey.RightArrow)
            {
                Buttons[currentSelection].IsSelected = false;
                currentSelection = currentSelection == Buttons.Count - 1 ? 0 : currentSelection + 1;
                if (Buttons[currentSelection] is Label label)
                {
                    currentSelection = currentSelection == Buttons.Count - 1 ? 0 : currentSelection + 1;
                }
                Buttons[currentSelection].IsSelected = true;
            }

        }
        public void PaintBox(string? message, int width, int height, int X, int Y)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Blue;
            int center = width - message.Length;
            Console.BackgroundColor = ConsoleColor.Cyan;
            
            Console.SetCursorPosition(X, Y);
            Console.Write(new string(' ',width+2));
            int setHeight = height < Console.WindowHeight ? Console.WindowHeight : height;
            setHeight = height < Console.WindowHeight/2 ? Console.WindowHeight/2 : height;
            Console.SetCursorPosition(X, Y + setHeight);//
            Console.Write(new string(' ', width+2));
            for (int i = 1; i < setHeight; i++)
            {
                
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(X,Y+i);
                Console.Write("  ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(new string(' ',width-2));
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(X+width, Y + i);
                Console.Write("  ");
                if(i == 2)
                {
                    Console.SetCursorPosition(X + center/2, Y + i);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{message}");

                }

                
                //Console.Write(j == 0 || j == width+1 ? " " : "");


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
    

    
