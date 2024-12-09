using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace JediBank
{
    class Bank
    {
        public static List<User> Users = new List<User>();
        public User? currentUser { get; set; } = null;
        public Account? currentAccount { get; set; } = null;
        //public string action { get; set;} 
        public bool IsLocked { get; internal set; } = false;

        Queue<Transaction> _TransferQue = new Queue<Transaction>();
        public void RunProgram()
        {
            Language language = new Language(Program.ChoosenLangugage);
            Users = DataBase.LoadUsers();
            UI uI = new UI();
            Window window = new Window();

            while (true)
            {
                if (uI.Menu(new string[] { language.TranslationTool("Login"), language.TranslationTool("Exit") }) == 0)
                {
                    currentUser = Login();
                    Dictionary<string, Delegate> actionMap = ActionMap(currentUser);
                    while (currentUser != null)
                    {
                        string action = currentUser.IsAdmin ? uI.MainMenu(AdminMenuOptions(currentUser), currentUser.Name) : uI.MainMenu(MainMenuOptions(currentUser), currentUser.Name);
                        currentAccount = currentUser.Accounts.Find(x => x.Name == action);
                        action = currentUser.Accounts.Contains(currentAccount) ? language.TranslationTool("Account") : action;
                        actionMap[action].DynamicInvoke();

                    }
                    
                }
            }
        }


        public Dictionary<string, string[]> MainMenuOptions(User user)
        {
            Language language = new Language(Program.ChoosenLangugage);
            Dictionary<string, string[]> alt = new Dictionary<string, string[]>
             {
                 { language.TranslationTool("💰 Accounts"), user.GetAccountNames() },
                 { language.TranslationTool("💼 Transactions"), [language.TranslationTool("Withdraw"), language.TranslationTool("InternalTransfer"), language.TranslationTool("ExternalTransfer")] },
                 { language.TranslationTool("⚙️ Manage"), [language.TranslationTool("Open account"), language.TranslationTool("Take loan")] },
                 { language.TranslationTool("🏦 Sign out"), [language.TranslationTool("Log out"), language.TranslationTool("Shut down")] }
             };
            return alt;

        }
        public Dictionary<string, string[]> AdminMenuOptions(User user)
        {
            Language language = new Language(Program.ChoosenLangugage);
            Dictionary<string, string[]> alt = new Dictionary<string, string[]>
             {
                 { language.TranslationTool("⚙️ Manage users"),[language.TranslationTool("Create user"), language.TranslationTool("Remove user")]},
                 { language.TranslationTool("🏦 Sign out"), [language.TranslationTool("Log out"), language.TranslationTool("Shut down")] }
             };
            return alt;

        }

        public Dictionary<string, Delegate> ActionMap(User user)
        {
            Language language = new Language(Program.ChoosenLangugage);
            Dictionary<string, Delegate> actionMap = new Dictionary<string, Delegate>
             {
                 { language.TranslationTool("Withdraw"), Withdraw },
                 { language.TranslationTool("InternalTransfer"), InternalTransfer },
                 { language.TranslationTool("ExternalTransfer"), ExternalTransfer },
                 { language.TranslationTool("Log out"), LogOut },
                 { language.TranslationTool("Account"), AccountShow },
                 { language.TranslationTool("Create user"), CreateUser },
                 { language.TranslationTool("Remove user"), RemoveUser },
                 { language.TranslationTool("Open account"), CreateAccount },
                 { language.TranslationTool("Take loan"), TakeLoan }


             };
            return actionMap;
        }
        public void AccountShow()
        {
            UI uI = new UI();

            uI.AccountMenu(currentUser, currentAccount);
            Console.Clear();
        }
        public void Withdraw()
        {
            Window window = new Window();
            Dictionary<decimal?, Account[]> withdrawInfo = window.RunWithdrawWindow(currentUser);
            if (!withdrawInfo.Any(kvp => kvp.Key == -1 || kvp.Value.Any(item => item == null)))
            {
                foreach (var kvp in withdrawInfo)
                {

                    kvp.Value[0].Subtract((decimal)kvp.Key);
                    DataBase.ArchiveUsers(Users);
                }
                DisplayMessage("Success");
            }
        }
        public async Task InternalTransfer()
        {
            // UI uI = new UI();
            //Account[] transferInfo = uI.TransferMenu(currentUser);
            Window window = new Window();

            Dictionary<decimal?, Account[]> transferInfo = window.RunInternalTransferWindow(currentUser, Users);
            foreach(var kvp in transferInfo) 
            { 
                if(kvp.Key != -9 && !kvp.Value.Any(x => x == null))
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
                    DisplayMessage("Success");
                }
            }



        }

        public async Task ExternalTransfer()
        {
            // UI uI = new UI();
            //Account[] transferInfo = uI.TransferMenu(currentUser);
            Window window = new Window();
            Dictionary<decimal?, Account[]> transferInfo = window.RunExternalTransferWindow(currentUser, Users);
            foreach (var kvp in transferInfo)
            {
                if (kvp.Key != -9 && !kvp.Value.Any(x => x == null))
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
                    DisplayMessage("Success");
                }
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
                DisplayMessage("Success");
            }

        }
        public void CreateUser()
        {
            Language language = new Language(Program.ChoosenLangugage);
            Window window = new Window();
            if (window.RunCreateUserWindow(Users)) 
            {
                DisplayMessage("Success");
            }
            /*
            Console.Write(language.TranslationTool("Select name: "));
            string username = Console.ReadLine();
            Console.Write(language.TranslationTool("Select password: "));
            string password = Console.ReadLine();
            Users.Add(new User
            {
                Name = username,
                Password = password

            });
            DisplayMessage("Success");
            DataBase.ArchiveUsers(Users);*/
        }

        public void RemoveUser()
        {
            Window window = new Window();
            window.RunRemoveUserWindow(Users);
            /*foreach (var user in Users)
            {
                Console.WriteLine(user.Name);
            }*/
            //Users.RemoveAt()
        }

        public void CreateAccount()
        {
            Window window = new Window();
            Account newAccount = window.RunCreateAccountWindow();
            if (newAccount != null)
            {
                currentUser.AddAccount(newAccount);
                DisplayMessage("Success");
            }
            DataBase.ArchiveUsers(Users);
        }
        public void LogOut()
        {
            currentUser = null;
        }
        private User Login()
        {
            Language language = new Language(Program.ChoosenLangugage);
            Console.Clear();

            DisplayLogo();
            UI uI = new UI();
            //User? currentUser = null;
            do
            {
                string userName = uI.ReadUserName();
                currentUser = Users.Find(i => i.Name == userName);

                if (currentUser == null)
                {
                    Console.WriteLine(language.TranslationTool("User not found"));
                    continue;
                }
                else if (currentUser.IsLocked)
                {
                    currentUser.UnlockUser();
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
                    Console.Clear();
                    Console.SetCursorPosition((Console.WindowWidth - language.TranslationTool("Too many failed attempts. Locking your account.").Length) / 2, Console.WindowHeight / 2 - 1);
                    Console.WriteLine(language.TranslationTool("Too many failed attempts. Locking your account."));
                    Console.SetCursorPosition((Console.WindowWidth - language.TranslationTool("Your account is locked. Please try again later.").Length) / 2, Console.WindowHeight / 2);
                    Console.WriteLine(language.TranslationTool("Your account is locked. Please try again later."));
                    Console.ReadLine();
                    currentUser.IsLocked = true;
                    break;
                }

                Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);

            } while (currentUser == null);

            return null;
        }
        public void DisplayMessage(string text)
        {
            Console.Clear();
            //Translation
            Console.SetCursorPosition((Console.WindowWidth - text.Length)/2, Console.WindowHeight/2);
            Console.WriteLine(text);
            Thread.Sleep(1000);
        }

      


        public void DisplayLogo()
        {
            int Xpos = (Console.WindowWidth/2 - "       __   _______  _______   __ ".Length);
            Console.SetCursorPosition(Xpos, 0);
            Green(); Console.Write("       __   _______  _______   __ "); Blue(); Console.Write(" .______        ___      .__   __.  __  ___ \r\n");
            Console.SetCursorPosition(Xpos, 1);
            Green(); Console.Write("      |  | |   ____||       \\ |  |"); Blue(); Console.Write(" |   _  \\      /   \\     |  \\ |  | |  |/  / \r\n");
            Console.SetCursorPosition(Xpos, 2);
            Green(); Console.Write("      |  | |  |__   |  .--.  ||  |"); Blue(); Console.Write(" |  |_)  |    /  ^  \\    |   \\|  | |  '  / \r\n");
            Console.SetCursorPosition(Xpos, 3);
            Green(); Console.Write(".--.  |  | |   __|  |  |  |  ||  |"); Blue(); Console.Write(" |   _  <    /  /_\\  \\   |  . `  | |    < \r\n");
            Console.SetCursorPosition(Xpos, 4);
            Green(); Console.Write("|  `--'  | |  |____ |  '--'  ||  |"); Blue(); Console.Write(" |  |_)  |  /  _____  \\  |  |\\   | |  .  \\ \r\n");
            Console.SetCursorPosition(Xpos, 5);
            Green(); Console.Write(" \\______/  |_______||_______/ |__|"); Blue(); Console.Write(" |______/  /__/     \\__\\ |__| \\__| |__|\\__\\ \r\n");
        }

        public void Green()
        {
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
