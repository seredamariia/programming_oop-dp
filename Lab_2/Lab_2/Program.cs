using System;
using System.Diagnostics.Metrics;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

interface IDevelopable
{
    public abstract void AssignTask();
    public abstract void Deploy();
    public abstract void Maintain();
}

interface ITestable
{
    public abstract void AssignTask();
}

public delegate void TestingHandler(Developer developer);

public abstract class Employee
{
    protected string name;
    protected int age;
    protected string phoneNumber;
    protected double salary;

    protected string sdlc_phase = "analysis";

    public Employee()
    {
        if (Program.access_level == "user")
            Console.WriteLine("You do not have rights to create new Employee");
        else
        {
            name = "Undefined employee";
            phoneNumber = "+380XXXXXXXXX";
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

    public abstract void DisplayMainSkill();

    protected double CalculateYearlyBonus()
    {
        return Salary * 0.05;
    }
}

public static class EmployeeExtension
{
    public static void DisplayFullInfo(this Employee employee)
    {
        if (Program.access_level == "admin")
            Console.WriteLine($"{employee.Name,-30}   {employee.Age,2} y.o.   {employee.PhoneNumber,13}   {employee.Salary,5} UAH");
        else
            Console.WriteLine("You do not have rights to see Full Info");
    }
}

public class Developer : Employee, IDevelopable, ITestable
{
    public Boolean isUnitTestingFinished { get; private set; } = false;
    //public event TestingHandler TestingEvent;
    //public event Action<Developer> TestingEvent;
    public event Func<Developer, object> TestingEvent;
    public Developer(string name, int age, string phoneNumber, double salary) : base(name, age, phoneNumber, salary) { }

    public override void DisplayMainSkill()
    {
        Console.WriteLine("Developer's main skill is writting code");
    }

    void IDevelopable.AssignTask()
    {
        Console.WriteLine("Analysis phase finished. Development phase now in progress...");
        sdlc_phase = "development";
        Console.WriteLine("Task assigned: system development");
    }

    void IDevelopable.Deploy()
    {
        Console.WriteLine("Deployment is in progress...");
        sdlc_phase = "deployment";
    }

    void IDevelopable.Maintain()
    {
        Console.WriteLine("Operations and maintenance are being performed...");
        sdlc_phase = "maintenance";
    }

    void ITestable.AssignTask()
    {
        Console.WriteLine("Task assigned: unit testing" + "\n");
        isUnitTestingFinished = true;
        Console.WriteLine("Unit testing finished" + "\n");

        TestingEvent?.Invoke((Developer)this);
    }

    public double CalculateDeveloperBonus()
    {
        double yearlyBonus = CalculateYearlyBonus();
        double developerSpecificBonus = Salary * 0.1;
        return yearlyBonus + developerSpecificBonus;
    }

}

public class Designer : Employee
{
    public Designer(string name, int age, string phoneNumber, double salary) : base(name, age, phoneNumber, salary) { }

    public override void DisplayMainSkill()
    {
        Console.WriteLine("Designer's main skill is creating UX/UI design");
    }

    public double CalculateDesignerBonus()
    {
        double yearlyBonus = CalculateYearlyBonus();
        double developerSpecificBonus = Salary * 0.05;
        return yearlyBonus + developerSpecificBonus;
    }
    public void Design()
    {
        Console.WriteLine("Design is in progress...");
        sdlc_phase = "design";
    }
}

public class Tester : Employee, ITestable
{
    public Tester(string name, int age, string phoneNumber, double salary) : base(name, age, phoneNumber, salary) { }

    public override void DisplayMainSkill()
    {
        Console.WriteLine("Tester's main skill is testing software");
    }

    void ITestable.AssignTask()
    {
        Console.WriteLine("Task assigned: test execution");
        sdlc_phase = "testing";
    }

    public double CalculateTesterBonus()
    {
        double yearlyBonus = CalculateYearlyBonus();
        double developerSpecificBonus = Salary * 0.025;
        return yearlyBonus + developerSpecificBonus;
    }

    public void startTesting(Developer developer)
    {
        if (developer.isUnitTestingFinished == true)
        {
            Console.WriteLine("Development phase finished. Testing phase now in progress...");
            ((ITestable)(Tester)this).AssignTask();
        }
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

public delegate void ExceptionHandler(object sender, InvalidAnswerEventArgs e);

public class InvalidAnswerException : Exception
{
    public InvalidAnswerException(string message) : base(message) { }
}


public class InvalidAnswerEventArgs : EventArgs
{
    public string InvalidAnswer { get; }

    public InvalidAnswerEventArgs(string invalidAnswer)
    {
        InvalidAnswer = invalidAnswer;
    }
}

public class Error
{
    public event ExceptionHandler InvalidAnswerProvided;

    public void HandleUserAnswer(string answer, ref Authorization authorization)
    {

        if (answer.ToUpper() == "NO")
        {
            authorization = Authorization.GetInstance();
        }
        else if (answer.ToUpper() == "YES")
        {
            Console.WriteLine("Enter login:");
            string login = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            authorization = Authorization.GetInstance(login, password);
        }
        else
        {
            InvalidAnswerProvided?.Invoke(null, new InvalidAnswerEventArgs(answer));
        }
    }

    public void InvalidAnswerHandler(object sender, InvalidAnswerEventArgs e)
    {
        throw new InvalidAnswerException($"Invalid answer: {e.InvalidAnswer}. Please enter [YES] or [NO].");
    }
}

class Program
{
    public static string access_level;

    static void Main(string[] args)
    {
        Error error = new Error();
        error.InvalidAnswerProvided += new ExceptionHandler(error.InvalidAnswerHandler);

        Authorization authorization = null;
        Console.WriteLine("Your current access level is [user] and you have limited rights. Do you want to change access level to [admin] and have unlimited rights?");
        Console.WriteLine("Enter [YES] or [NO]");

        do
        {
            string answer = Console.ReadLine();

            try
            {
                error.HandleUserAnswer(answer, ref authorization);
            }
            catch (InvalidAnswerException ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (authorization == null);

        access_level = authorization.GetAccessLevel();

        Developer developer = new Developer("Matvii", 18, "+380509695043", 37000);

        Tester tester = new Tester("Mariia", 18, "+380996625909", 72000);


        Console.WriteLine();

        ////Дефолтна реалізація
        //developer.TestingEvent += new TestingHandler(tester.startTesting);

        ////Реалізація через анонімний метод
        //developer.TestingEvent += delegate (Developer developer)
        //{
        //    if (developer.isUnitTestingFinished == true)
        //    {
        //        Console.WriteLine("Development phase finished. Testing phase now in progress...");
        //        ((ITestable)(tester)).AssignTask();
        //    }
        //};

        ////Реалізація через лямбда вираз
        //developer.TestingEvent += (Developer developer) =>
        //{
        //    if (developer.isUnitTestingFinished == true)
        //    {
        //        Console.WriteLine("Development phase finished. Testing phase now in progress...");
        //        ((ITestable)(tester)).AssignTask();
        //    }
        //};

        ////Реалізація через Action
        //Action<Developer> TestingEvent = (developer) =>
        //{
        //    if (developer.isUnitTestingFinished == true)
        //    {
        //        Console.WriteLine("Development phase finished. Testing phase now in progress...");
        //        ((ITestable)(tester)).AssignTask();
        //    }
        //};
        //developer.TestingEvent += TestingEvent;

        //Реалізація через Func
        Func<Developer, object> TestingEvent = (developer) =>
        {
            if (developer.isUnitTestingFinished == true)
            {
                Console.WriteLine("Development phase finished. Testing phase now in progress...");
                ((ITestable)(tester)).AssignTask();
            }
            return "The test was done by " + developer.Name;
        };
        developer.TestingEvent += TestingEvent;

        ((IDevelopable)developer).AssignTask();
        Console.WriteLine();

        ((ITestable)developer).AssignTask();
    }
}