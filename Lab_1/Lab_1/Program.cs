using System;

public class Employee : IDisposable
{
    // Для перевірки, чи викликався вже метод Dispose().
    private bool disposed = false;

    private string name;
    private int age;
    private string phoneNumber;
    private double salary;

    public Employee()
    {
        if (Program.access_level == "user")
            Console.WriteLine("You do not have rights to create new Employee");
        else
        {
            name = "Undefined employee";
            phoneNumber = "+380XXXXXXXXX";
            Console.WriteLine("Employee initial constructor worked successfully");
        }
    }

    public Employee(string name, int age, string phoneNumber, double salary)
    {
        if (Program.access_level == "user")
            Console.WriteLine("You do not have rights to create new Employee");
        else
        {
            Name = name;
            Age = age;
            PhoneNumber = phoneNumber;
            Salary = salary;
            Console.WriteLine("Employee parametric constructor worked successfully");
        }
    }

    public string Name
    {
        get { return name; }
        set
        {
            if (Program.access_level == "user")
                Console.WriteLine("You do not have rights to set Name");
            else if (value == null)
                Console.WriteLine("Name cannot be empty");
            else
                name = value;
        }
    }

    public int Age
    {
        get
        {
            if (Program.access_level == "user")
            {
                Console.WriteLine("You do not have rights to get Age");
                return 0;
            }
            else
                return age;
        }
        set
        {
            if (Program.access_level == "user")
                Console.WriteLine("You do not have rights to set Age");
            else if (value < 16 || value > 99)
                Console.WriteLine("Age must be in between 16 and 99");
            else
                age = value;
        }
    }

    public string PhoneNumber
    {
        get { return phoneNumber; }
        set
        {
            if (Program.access_level == "user")
                Console.WriteLine("You do not have rights to set Phone Number");
            else if (value == null)
                Console.WriteLine("Phone Number cannot be empty");
            else if (!value.Contains("+380"))
                Console.WriteLine("Phone Number must start with +380");
            else
                phoneNumber = value;
        }
    }

    public double Salary
    {
        get
        {
            if (Program.access_level == "user")
            {
                Console.WriteLine("You do not have rights to get Salary");
                return 0;
            }
            else
                return salary;
        }
        set
        {
            if (Program.access_level == "user")
                Console.WriteLine("You do not have rights to set Salary");
            else if (value < 6700.00)
                Console.WriteLine("Salary cannot be less than 6700 UAH");
            else
                salary = value;
        }
    }

    public virtual void DisplayMainSkill()
    {
        Console.WriteLine("Employee's main skill depends on theirs position");
    }

    public void DisplayFullInfo()
    {
        if (Program.access_level == "admin")
            Console.WriteLine($"{name,-30}   {age,2} y.o.   {phoneNumber,13}   {salary,5} UAH");
        else
            Console.WriteLine("You do not have rights to see Full Info");
    }

    public void Dispose()
    {
        // Виклик допоміжного методу. "true" - якщо очищення було викликане користувачем об'єкта. 
        CleanUp(true);

        // Подавлення фіналізації.
        GC.SuppressFinalize(this);

        GC.ReRegisterForFinalize(this);
    }

    protected virtual void CleanUp(bool disposing)
    {
        // Перевірка, чи було виконано очищення.
        if (!disposed)
        {
            if (disposing)
            {
                Console.WriteLine("Disposing managed resourses");
            }
            Console.WriteLine("Disposing unmanaged resourses");
            disposed = true;
        }
    }

    ~Employee()
    {
        // Виклик допоміжного методу.
        CleanUp(false);
        Console.WriteLine("Employee destroyed");
    }
}

public class Developer : Employee
{
    public Developer(string name, int age, string phoneNumber, double salary) : base(name, age, phoneNumber, salary)
    {
        Console.WriteLine("Developer parametric constructor worked successfully");
    }

    public override void DisplayMainSkill()
    {
        Console.WriteLine("Developer's main skill is writting code");
    }

    ~Developer()
    {
        Console.WriteLine("Developer destroyed");
    }
}

public class Designer : Employee
{
    public Designer(string name, int age, string phoneNumber, double salary) : base(name, age, phoneNumber, salary)
    {
        Console.WriteLine("Designer parametric constructor worked successfully");
    }

    public override void DisplayMainSkill()
    {
        Console.WriteLine("Designer's main skill is creating UX/UI design");
    }

    ~Designer()
    {
        Console.WriteLine("Designer destroyed");
    }
}

public class Tester : Employee
{
    public Tester(string name, int age, string phoneNumber, double salary) : base(name, age, phoneNumber, salary)
    {
        Console.WriteLine("Tester parametric constructor worked successfully");
    }

    public override void DisplayMainSkill()
    {
        Console.WriteLine("Tester's main skill is testing software");
    }

    ~Tester()
    {
        Console.WriteLine("Tester destroyed");
    }
}

public class Authorization
{
    private static Authorization instance;

    private string login;
    private string password;
    private static string correctLogin = "admin";
    private static string correctPassword = "PtahaFred";
    private string access_level;

    private Authorization()
    {
        login = "user";
        access_level = "user";
    }

    private Authorization(string login, string password)
    {
        if (login == correctLogin && password == correctPassword)
            access_level = "admin";
        else
        {
            Console.WriteLine("Login or password is invalid. Your current access level is [user]");
            access_level = "user";
        }

    }

    public static Authorization GetInstance()
    {
        if (instance == null)
        {
            instance = new Authorization();
        }
        return instance;
    }

    public static Authorization GetInstance(string login, string password)
    {
        if (instance == null)
        {
            instance = new Authorization(login, password);
        }
        return instance;
    }

    public string GetAccessLevel()
    {
        return access_level;
    }
}

public class CurrentTasks
{
    private decimal number;

    public CurrentTasks(decimal count)
    {
        Number = number;
    }

    decimal Number
    {
        get { return number; }
        set
        {
            if (Program.access_level == "user")
                Console.WriteLine("You do not have rights to set Number of Current Tasks");
            else if (value >= 0)
                number = value;
        }
    }
}

class Program
{
    public static string access_level;

    static void Main(string[] args)
    {
        Authorization authorization = null;
        Console.WriteLine("Your current access level is [user] and you have limited rights. Do you want to change access level to [admin] and have unlimited rights?");
        Console.WriteLine("Enter [YES] or [NO]");
        string answer = Console.ReadLine();
        if (answer.ToUpper() == "NO")
            authorization = Authorization.GetInstance();
        else if (answer.ToUpper() == "YES")
        {
            Console.WriteLine("Enter login:");
            string login = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            authorization = Authorization.GetInstance(login, password);
        }
        access_level = authorization.GetAccessLevel();

        Console.WriteLine();

        Employee employee = new Employee();
        Console.WriteLine("Memory before collect: " + GC.GetTotalMemory(false));
        Console.WriteLine($"Generation: {GC.GetGeneration(employee)}");
        GC.Collect();
        Console.WriteLine("Memory after collect: " + GC.GetTotalMemory(false));
        Console.WriteLine($"Generation: {GC.GetGeneration(employee)}");
        employee.Dispose();

        Console.WriteLine();

        CurrentTasks currentTasks;
        for (int i = 0; i < 1000; i++)
        {
            currentTasks = new CurrentTasks(1000);
        }
        Console.WriteLine("Memory used before collection: {0:N0}",
        GC.GetTotalMemory(false));
        // Collect all generations of memory.
        GC.Collect();
        Console.WriteLine("Memory used after full collection: {0:N0}",
        GC.GetTotalMemory(true));
    }
}