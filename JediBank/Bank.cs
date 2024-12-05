using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace JediBank
{
    class Bank
    {
        public List<User> Users = new List<User>();
        public User? currentUser { get; set; } = null;
        public Account? currentAccount { get; set; } = null;
        //public string action { get; set;} 
        public bool IsLocked { get; internal set; } = false;

        Queue<Transaction> _TransferQue = new Queue<Transaction>();
        public void RunProgram()
        {

            Users = DataBase.LoadUsers();
            UI uI = new UI();
            Window window = new Window();
            
            while (true)
            {
                if (uI.Menu(new string[] { "Login", "Exit" }) == 0)
                {
                    currentUser = Login();
                    Dictionary<string, Delegate> actionMap = ActionMap(currentUser);
                    while (currentUser != null)
                    {
                        string action = currentUser.IsAdmin ? uI.MainMenu(AdminMenuOptions(currentUser), currentUser.Name) : uI.MainMenu(MainMenuOptions(currentUser), currentUser.Name);
                        currentAccount = currentUser.Accounts.Find(x => x.Name == action);
                        action = currentUser.Accounts.Contains(currentAccount) ? "Account" : action;
                        actionMap[action].DynamicInvoke();

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
                    */
                }
            }
        }

      
        public Dictionary<string, string[]> MainMenuOptions(User user)
        {
            Dictionary<string, string[]> alt = new Dictionary<string, string[]>
             {
                 { "💰 Accounts", user.GetAccountNames() },
                 { "💼 Transactions", ["Withdraw", "Transfer"] },
                 { "⚙️ Manage", ["Open account", "Take loan"] },
                 { "🏦 Sign out", ["Log out", "Shut down"] }
             };
            return alt;

        }
        public Dictionary<string, string[]> AdminMenuOptions(User user)
        {
            Dictionary<string, string[]> alt = new Dictionary<string, string[]>
             {
                 { "⚙️ Manage users",["Create user", "Remove user"]},
                 { "🏦 Sign out", ["Log out", "Shut down"] }
             };
            return alt;

        }

        public Dictionary<string, Delegate> ActionMap(User user)
        {
            Dictionary<string, Delegate> actionMap = new Dictionary<string, Delegate>
             {
                 { "Withdraw", Withdraw },
                 { "Transfer", Transfer },
                 { "Log out", LogOut },
                 { "Account", AccountShow },
                 { "Create user", CreateUser },
                 { "Remove user", RemoveUser },
                 { "Open account", CreateAccount },
                { "Take loan", TakeLoan }


             };
            return actionMap;
        }
        public void AccountShow()
        {
            UI uI = new UI();

            uI.AccountMenu(currentUser, currentAccount);
        }
        public void Withdraw()
        {
            Window window = new Window();
            Dictionary<decimal?, Account[]> withdrawInfo = window.RunWithdrawWindow(currentUser);
            if(!withdrawInfo.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null))) 
            { 
                foreach (var kvp in withdrawInfo)
                {

                    kvp.Value[0].Subtract((decimal)kvp.Key);
                    DataBase.ArchiveUsers(Users);
                }
            } 
        }
        public async Task Transfer()
        {
           // UI uI = new UI();
            //Account[] transferInfo = uI.TransferMenu(currentUser);
            Window window = new Window();
            Dictionary<decimal?, Account[]> transferInfo = window.RunTransferWindow(currentUser, Users);
            foreach(var kvp in transferInfo) 
            { 
                Transaction transferDetails = new Transaction
                {
                    SenderAccount = kvp.Value[0], 
                    ReciverAccount = kvp.Value[1], 
                    Amount = (decimal)kvp.Key, // Amount behövs läggas in i UI
                    DateTime = DateTime.Now

                };
                _TransferQue.Enqueue(transferDetails);
                await _TransferQue.Peek().ExecuteTransaction();
                _TransferQue.Dequeue();
                DataBase.ArchiveUsers(Users);
            }

            

        }
        public void TakeLoan() 
        {
            Window window = new Window();
            Dictionary<decimal?, Account[]> loanInfo = window.RunLoanWindow(currentUser);
            if (!loanInfo.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null)))
            {
                foreach (var kvp in loanInfo)
                {
                    currentUser.CreateLoan(kvp.Value[0], (decimal)kvp.Key);
                    DataBase.ArchiveUsers(Users);
                }
            }

        }
        public void CreateUser()
        {
            Console.Write("Select name: ");
            string username = Console.ReadLine();
            Console.Write("Select password: ");
            string password = Console.ReadLine();
            Users.Add(new User
            {
                Name = username,
                Password = password

            });
            DataBase.ArchiveUsers(Users);
        }

        public void RemoveUser()
        {
            foreach (var user in Users)
            {
                Console.WriteLine(user.Name);
            }
            //Users.RemoveAt()
        }

        public void CreateAccount()
        {
            Window window = new Window();
            Account newAccount = window.RunCreateAccountWindow();
            if(newAccount != null)
            {
                currentUser.AddAccount(newAccount);
            }
            DataBase.ArchiveUsers(Users);
        }
        public void LogOut()
        {
            currentUser = null;
        }
        private User Login()
        {
            Console.Clear();

            DisplayLogo();
            UI uI = new UI();
            //User? currentUser = null;
            do
            {
                string userName = uI.ReadUserName();
                currentUser = Users.Find(i => i.Name == userName);

                if (currentUser.IsLocked) 
                { 
                    currentUser.UnlockUser();
                }
                if (currentUser == null)
                {
                    Console.WriteLine("User not found");
                    continue; 
                }

                int count = 0;
                while (count < 3 && !currentUser.IsLocked)
                {
                    if (currentUser.Password == uI.ReadPassword())
                    {
                        return currentUser;
                    }

                    count++;
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
                }

                if (count == 3)
                {
                    currentUser.IsLocked = true;
                    Console.WriteLine("Too many failed attempts. Locking your account.");
                    Console.WriteLine("Your account is locked. Please try again later.");
                    Console.ReadLine();
                    break;
                }
            } while (currentUser == null);

            return null;
        }

        public void DisplayLogo()
        {
            Green(); Console.Write("       __   _______  _______   __ "); Blue(); Console.Write(" .______        ___      .__   __.  __  ___ \r\n");
            Green(); Console.Write("      |  | |   ____||       \\ |  |"); Blue(); Console.Write(" |   _  \\      /   \\     |  \\ |  | |  |/  / \r\n");
            Green(); Console.Write("      |  | |  |__   |  .--.  ||  |"); Blue(); Console.Write(" |  |_)  |    /  ^  \\    |   \\|  | |  '  / \r\n");
            Green(); Console.Write(".--.  |  | |   __|  |  |  |  ||  |"); Blue(); Console.Write(" |   _  <    /  /_\\  \\   |  . `  | |    < \r\n");
            Green(); Console.Write("|  `--'  | |  |____ |  '--'  ||  |"); Blue(); Console.Write(" |  |_)  |  /  _____  \\  |  |\\   | |  .  \\ \r\n");
            Green(); Console.Write(" \\______/  |_______||_______/ |__|"); Blue(); Console.Write(" |______/  /__/     \\__\\ |__| \\__| |__|\\__\\ \r\n");
        }

        public void Green()
        {
            Console.SetCursorPosition((Console.WindowWidth / 5)-4, Console.GetCursorPosition().Top);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Blue;
        }

        public void Blue()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

    }
}
