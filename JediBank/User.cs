using JediBank.CurrencyFolder;

namespace JediBank
{
    internal class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalLoans { get; set; }
        public decimal MaxLoan { get; set; }
        public List<Loan> Loans { get; set; } = new();
        public List<Account> Accounts { get; set; } = new();
        public bool IsLocked { get; internal set; }

        /*public User(string name, string password, bool isAdmin)
        {
            Name = name;
            Password = password;
            IsAdmin = isAdmin;
        }*/
        public async Task UnlockUser()
        {
            await Task.Delay(5000);
            IsLocked = false;
        }
        public void CreateLoan(Account toAccount, decimal amount, decimal interest)
        {
            if (amount <= CalculateMaxLoan())
            {
                Loan newLoan = new Loan
                {
                    LoanAmount = amount,
                    LoanId = Loans.Count() + 1,
                };
                newLoan.CalculateInterest();
                newLoan.CalculateTotal();
                Loans.Add(newLoan);
                toAccount.Add(amount);
            }

        }

        public void CalculateBalance()
        {
            foreach (var account in Accounts)
            {
                TotalBalance += account.Balance;
            }
        }

        public void CalculateLoans()
        {
            foreach (var loan in Loans)
            {
                TotalLoans += loan.LoanAmount;
            }
        }

        public decimal CalculateMaxLoan()
        {
            CalculateBalance();
            CalculateLoans();
            MaxLoan = (TotalBalance - TotalLoans) * 5 - TotalLoans;
            return MaxLoan;
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
        public string[] GetAccountNames()
        {
            return Accounts.Select(obj => obj.Name).ToList().ToArray();
        }

        public bool Exchange(decimal amount, Currency currency, Account account1, Account account2)

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
            Console.Write("Choose name: ");
            string name = Console.ReadLine();
            Accounts.Add(new Account
            {
                Name = name,
                Balance = 0,
                Currency = new SEK()
            });
        }
    }
}
