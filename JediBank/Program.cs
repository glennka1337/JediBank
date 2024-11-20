namespace JediBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UI x = new UI();
            x.ReadUserName();
            x.ReadPassword();
            x.Menu(new string[] { "a", "b", "c" });
        }
    }
}
