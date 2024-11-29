namespace JediBank
{
    class Bank
    {
        public List<User> Users = new List<User>();
        public User currentUser { get; set; } = null;
        public string action { get; set;} 
        public void RunProgram()
        {
            
            Users = DataBase.LoadUsers();
            UI uI = new UI();

            while (true)
            {
                if (uI.Menu(new string[] { "Login", "Exit" }) == 0)
                {
                    currentUser = Login();
                    Dictionary<string, Delegate> actionMap = ActionMap(currentUser);
                    while (currentUser != null)
                    {
                        action = uI.MainMenu(MainMenuOptions(currentUser), currentUser.Name);
                        actionMap[action].DynamicInvoke();
                    }
                }
            }

        }
        public Dictionary<string, string[]> MainMenuOptions(User user)
        {
             Dictionary<string, string[]> alt = new Dictionary<string, string[]>
             {
                 { "💰 Accounts", user.GetAccountNames() },
                 { "💼 Transactions", ["Withdraw", "Transfer"] },
                 { "🏦 Sign out", ["Log out", "Shut down"] }
             };
            return alt;
        }
  /*
                        string chosenOption = uI.MainMenu(MainMenuOptions(currentUser), currentUser.Name);
                        if (chosenOption == "🏦 Sign out")
                        {
                            currentUser = null;
                        }
                        else
                        {
                            var selectedAccount = currentUser.Accounts.FirstOrDefault(acc => acc.Name == chosenOption);
                            if (selectedAccount != null)
                            {
                                uI.AccountMenu(currentUser, selectedAccount);
                            }
                        }
                    }
                }
            }

        }
        */

        public Dictionary<string, Delegate> ActionMap(User user)
        {
            Dictionary<string, Delegate> actionMap = new Dictionary<string, Delegate>
             {
                 { "Withdraw", Withdraw },
                 { "Transfer", Transfer },
                 { "Log out", LogOut },

             };
            return actionMap;
        }
        public void AccountShow()
        {
            UI uI = new UI();
            Account currentAccount = currentUser.Accounts.Find(x => x.Name == action);
            //uI.AccountMenu(currentAccount);
        }
        public void Withdraw()
        {

        }
        public void Transfer()
        {
            UI uI = new UI();
            Account[] transferInfo = uI.TransferMenu(currentUser);

        }
        public void LogOut()
        {
            currentUser = null;
        }
        private User Login()
        {
            Console.Clear();
            UI uI = new UI();
            User? currentUser = null;
            do
            {
                string userName = uI.ReadUserName();
                currentUser = Users.Find(i => i.Name == userName);
                int count = 0;
                while (count < 3 && currentUser != null)
                {

                    if (currentUser.Password == uI.ReadPassword())
                    {
                        return currentUser;
                    }
                    count++;
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
                    Console.Write("\r                                      ");
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
                }
                if (count == 3)
                {
                    return null;
                }
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
                Console.Write("\r                                       ");
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top-1);
            } while (currentUser == null);
            return null;    
        }

        public Dictionary<string, string[]> MainMenuOptions(User user)
        {
            Dictionary<string, string[]> alt = new Dictionary<string, string[]>
            {
                 { "💰 Accounts", user.GetAccountNames() },
                 { "💼 Mer", ["hej", "hugo"] },
                 { "🏦 Sign out", new string[] { "Log out" } }
            };
            return alt;
        }



        /* public void AddUser(User user)
         {
             Users.Add(user);
             Console.WriteLine($"Användare {user.Username} har lagts till");
         }*/

        /*public void CreateUser()
        {
            Console.WriteLine("Skapa ny användare:");
            Console.Write("Ange användarnam:");
            string username = Console.ReadLine();
            Console.Write("Ange lösenord: ");
            string password = Console.ReadLine();

            User newUser = new User
            {
                Username = username,
                Password = password
            };
            AddUser(newUser);

        }*/
        /*public void LoadUsers()
        {

        }*/

        /*public void ArchiveUsers()
        {
            Console.WriteLine("Arkiverar användare. ");
            foreach (User user in Users)
            {
                Console.WriteLine($"Användarinfo: {user.Username}, {user.Password}");
            }
        }*/

       /* public bool LoanRequirement(User user)
        {
            Console.WriteLine("Checkar om användare uppfyller krav: ");
            decimal totalBalance = 0;

            foreach (var account in user.Accounts)
            {
                totalBalance += account.Balance;
            }
            decimal maxLoanAmount = totalBalance * 5;
            return user.LoanAmount <= maxLoanAmount;
        }*/
    }
}
