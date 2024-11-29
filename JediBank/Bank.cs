namespace JediBank
{
    class Bank
    {
        public List<User> Users = new List<User>();

        public void RunProgram()
        {
            
            Users = DataBase.LoadUsers();
            UI uI = new UI();
            User? currentUser;
            //startMenu
            while (true)
            {
                if (uI.Menu(new string[] { "Login", "Exit" }) == 0)
                {
                    currentUser = Login();
                    while (currentUser != null)
                    {
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
