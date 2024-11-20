namespace JediBank
{
    public class Bank
    {
        public List<User> Users { get; set; }

        public Bank()
        {
            Users = new List<User>();
        }

        public void RunProgram()
        {

        }
        public void AddUser(User user)
        {
            Users.Add(user);
            Console.WriteLine($"Användare {user.Username} har lagts till");
        }

    public void CreateUser()
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

        }
    public void LoadUsers()
    {

    }

    public void ArchiveUsers()
    {
            Console.WriteLine("Arkiverar användare. ");
            foreach (User user in Users)
            {
                Console.WriteLine($"Användarinfo: {user.Username}, {user.Password}");
            }
        }

        public bool LoanRequirement(User user)
        {
            Console.WriteLine("Checkar om användare uppfyller krav: ");
        decimal totalBalance = 0;

            foreach (var account in user.Accounts)
        {
            totalBalance += account.Balance;
        }
    decimal maxLoanAmount = totalBalance * 5;
        return user.LoanAmount <= maxLoanAmount;
    }
}
