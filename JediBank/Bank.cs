﻿using System.Linq;

namespace JediBank
{
    class Bank
    {
        public List<User> Users = new List<User>();
        public User? currentUser { get; set; } = null;
        public Account? currentAccount { get; set; } = null;
        //public string action { get; set;} 
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
                            string action = currentUser.IsAdmin ? uI.MainMenu(AdminMenuOptions(currentUser), currentUser.Name) : uI.MainMenu(MainMenuOptions(currentUser), currentUser.Name);
                            currentAccount = currentUser.Accounts.Find(x => x.Name == action);
                            action = currentUser.Accounts.Contains(currentAccount) ? "Account" : action;
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

        public Dictionary<string, string[]> AdminMenuOptions(User user)
        {
            Dictionary<string, string[]> alt = new Dictionary<string, string[]>
             {
                 { "⚙️ Manage users", ["Create user", "Remove user"] },
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
                 { "Account", AccountShow },
                 { "Create user", CreateUser },
                 { "Remove user", RemoveUser }

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

        }
        public void Transfer()
        {
            UI uI = new UI();
            Account[] transferInfo = uI.TransferMenu(currentUser);

        }

        public void CreateUser()
        {
            Console.WriteLine("Select name: ");
            string username = Console.ReadLine();
            Console.WriteLine("Select password: ");
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
        public void LogOut()
        {
            currentUser = null;
        }
        private User Login()
        {
            Console.Clear();
            UI uI = new UI();
            //User? currentUser = null;
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
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
            } while (currentUser == null);
            return null;
        }
    }
}
