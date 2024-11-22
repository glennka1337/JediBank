namespace JediBank
{
    internal class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public List<Account> Accounts { get;} = new();

        public User(string name, string password, bool isAdmin)
        {
            Name = name;
            Password = password;
            IsAdmin = isAdmin;
        }

        public void ShowAccounts()
        {   
            foreach (var account in Accounts)
            {
                Console.WriteLine($"{account.Name} - {account.Balance:C}");
            }

        }

        public bool TransferFunds(decimal amount, Account account1, Account account2)
        {
            if (account1.Balance >= amount)
            {
                account1.Subtract(amount);
                account2.Add(amount);
                return true;
            }
            else return false;
        }

        public bool Withdraw(decimal amount, Account account)
        {
            if (account.Balance >= amount)
            {
                account.Subtract(amount);
                return true;
            }
            else return false;
        }

/*        public bool AddFunds(decimal amount)
        {
            return true;
        }*/

        public bool Exchange(decimal amount, string currency, Account account1, Account account2)
        {
            if (account2.Currency == currency && account1.Balance >= amount)
            {
                account1.Subtract(amount);
                account2.Add(amount);
                return true;
            }
            else return false;
        }

        public bool Login()
        {
            UI ui = new UI();
            return (Password == ui.ReadPassword() ? true : false) ;
        }
        public void AddAccount()
        {
            Accounts.Add(new Account
            {
                Name = "Test",
                Balance = 9999,
                Currency = "SEK",
            });
        }
    }
}
