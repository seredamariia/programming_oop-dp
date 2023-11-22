using System.Text;

interface IBankingModule
{
    void UseInternetBanking(User user);
}

public class User
{
    public string Name { get; set; }
    public Boolean IsServiceEnabled;

    public User (string name, bool isServiceEnabled)
    {
        Name = name;
        IsServiceEnabled = isServiceEnabled;
    }
}

class RealBankingModule : IBankingModule
{
    public void UseInternetBanking(User user)
    {
        Console.WriteLine("{0}, Ви користуєтеся послугами інтернет-банкінгу.\n", user.Name);
    }
}

class BankingModuleProxy : IBankingModule
{
    private RealBankingModule realModule;

    public void UseInternetBanking(User user)
    {
        if (user.IsServiceEnabled)
        {
            if (realModule == null)
                realModule = new RealBankingModule();

            realModule.UseInternetBanking(user);
        }
        else
        {
            Console.WriteLine("{0}, послуга інтернет-банкінгу вимагає реєстрації.\n", user.Name);
        }
    }

    public void EnableService(User user)
    {
        user.IsServiceEnabled = true;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        BankingModuleProxy bm = new BankingModuleProxy();

        User user1 = new User("Георгій", true);
        bm.UseInternetBanking (user1);

        User user2 = new User("Ольга", false);
        bm.UseInternetBanking(user2);
        bm.EnableService(user2);
        bm.UseInternetBanking(user2);
    }
}
