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
        public void RunProgram(Language language)
        {

            Users = DataBase.LoadUsers();
            UI uI = new UI();
            Window window = new Window();
            
            language.ChooseLanguage();

            while (true)
            {
                if (uI.Menu(new string[] {
                    language.TranslationTool("Login"),
                    language.TranslationTool("Exit")
                }) == 0)
                {
                    currentUser = Login(language);
                    Dictionary<string, Delegate> actionMap = ActionMap(currentUser, language);
                    while (currentUser != null)
                    {
                        string action = currentUser.IsAdmin ? uI.MainMenu(AdminMenuOptions(currentUser, language), currentUser.Name) : uI.MainMenu(MainMenuOptions(currentUser, language), currentUser.Name);
                        currentAccount = currentUser.Accounts.Find(x => x.Name == action);
                        action = currentUser.Accounts.Contains(currentAccount) ? language.TranslationTool("Account") : action;
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

      
        public Dictionary<string, string[]> MainMenuOptions(User user, Language language)
        {
            Dictionary<string, string[]> alt = new Dictionary<string, string[]>
             {
                 { language.TranslationTool("💰 Accounts"), user.GetAccountNames() },
                 { language.TranslationTool("💼 Transactions"), [language.TranslationTool("Withdraw"), language.TranslationTool("Transfer")] },
                 { language.TranslationTool("⚙️ Manage"), [language.TranslationTool("Open account"), language.TranslationTool("Take loan")] },
                 { language.TranslationTool("🏦 Sign out"), [language.TranslationTool("Log out"), language.TranslationTool("Shut down")] }
             };
            return alt;

        }
        public Dictionary<string, string[]> AdminMenuOptions(User user, Language language)
        {
            Dictionary<string, string[]> alt = new Dictionary<string, string[]>
             {
                 { language.TranslationTool("Handle users"),[language.TranslationTool("create"), language.TranslationTool("remove")]},
                 { language.TranslationTool("💼 Transactions"), [language.TranslationTool("Withdraw"), language.TranslationTool("Transfer")] },
                 { language.TranslationTool("🏦 Sign out"), [language.TranslationTool("Log out"), language.TranslationTool("Shut down")] }
             };
            return alt;

        }

        public Dictionary<string, Delegate> ActionMap(User user, Language language)
        {
            Dictionary<string, Delegate> actionMap = new Dictionary<string, Delegate>
             {
                 { language.TranslationTool("Withdraw"), Withdraw },
                 { language.TranslationTool("Transfer"), Transfer },
                 { language.TranslationTool("Log out"), LogOut },
                 { language.TranslationTool("Account"), AccountShow },
                 { language.TranslationTool("Create user"), CreateUser },
                 { language.TranslationTool("Remove user"), RemoveUser },
                 { language.TranslationTool("Open account"), CreateAccount },
                { language.TranslationTool("Take loan"), TakeLoan }


             };
            return actionMap;
        }
        public void AccountShow(Language language)
        {
            UI uI = new UI();

            uI.AccountMenu(currentUser, currentAccount, language);
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
                    currentUser.CreateLoan(kvp.Value[0], (decimal)kvp.Key, 1.05m);
                    //kvp.Value[0].Subtract((decimal)kvp.Key);
                    //taBase.ArchiveUsers(Users);
                }
            }
        }
        public void CreateUser(Language language)
        {
            Console.Write(language.TranslationTool("Select name:"));
            string username = Console.ReadLine();
            Console.Write(language.TranslationTool("Select password:"));
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
            currentUser.AddAccount();
            DataBase.ArchiveUsers(Users);
        }
        public void LogOut()
        {
            currentUser = null;
        }
        private User Login(Language language)
        {
            Console.Clear();
            UI uI = new UI();
            //User? currentUser = null;
            do
            {
                string userName = uI.ReadUserName(language);
                currentUser = Users.Find(i => i.Name == userName);

                if (currentUser.IsLocked) 
                { 
                    currentUser.UnlockUser();
                }
                if (currentUser == null)
                {
                    Console.WriteLine(language.TranslationTool("User not found"));
                    continue; 
                }

                int count = 0;
                while (count < 3 && !currentUser.IsLocked)
                {
                    if (currentUser.Password == uI.ReadPassword(language))
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
                    Console.WriteLine(language.TranslationTool("Too many failed attempts. Locking your account."));
                    Console.WriteLine(language.TranslationTool("Your account is locked. Please try again later."));
                    Console.ReadLine();
                    break;
                }
            } while (currentUser == null);

            return null;
        }
    }
}
