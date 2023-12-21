using System.Text;
using System.Xml.Linq;

// Абстрактний клас посередника
abstract class Mediator
{
    // Метод, який використовують колеги для відправки повідомлень
    public abstract void Send(string message, Colleague colleague);
}

// Конкретний посередник - секретар компанії
class Secretary : Mediator
{
    private List<Colleague> colleagues = new List<Colleague>();
    private List<Colleague> confirmed = new List<Colleague>();
    private Boss boss;

    public void AddColleague(Colleague colleague)
    {
        colleagues.Add(colleague);
    }

    public void SetBoss(Boss boss)
    {
        this.boss = boss;
    }

    // Переадресація повідомлень від колег
    public override void Send(string message, Colleague colleague)
    {
        // Якщо повідомлення від керівника, то він організовує мітинг
        if (colleague == boss)
        {
            // Сповіщаємо всіх учасників про мітинг
            foreach (var c in colleagues)
            {
                c.Notify(message);
            }
        }
        // Якщо повідомлення від учасника, то він підтверджує або відмовляється від участі
        else
        {
            // Якщо підтвердження, то додаємо до списку підтверджених
            if (message == "Так")
            {
                confirmed.Add(colleague);
                Console.WriteLine("Робітник {0} підтверджує участь", colleague.Name);
            }
            // Якщо відмова, то видаляємо зі списку учасників
            else if (message == "Ні")
            {
                colleagues.Remove(colleague);
                Console.WriteLine("Робітник {0} відмовляється від участі", colleague.Name);
            }
            // Якщо всі учасники відповіли, то передаємо список підтверджених керівнику
            if (confirmed.Count == colleagues.Count)
            {
                boss.Notify("Список підтверджених учасників мітингу: ");
                foreach (var c in confirmed)
                {
                    Console.Write(c.Name + " ");
                }
            }
        }
    }
}

abstract class Colleague
{
    protected Mediator mediator;
    public string Name { get; set; }

    public Colleague(string name, Mediator mediator)
    {
        this.Name = name;
        this.mediator = mediator;
    }

    public virtual void Send(string message)
    {
        mediator.Send(message, this);
    }

    public abstract void Notify(string message);
}

class Boss : Colleague
{
    public Boss(string name, Mediator mediator) : base(name, mediator)
    {
    }

    public override void Notify(string message)
    {
        Console.Write("Керівник {0} отримав повідомлення: {1}", Name, message);
    }
}

class Worker : Colleague
{
    public Worker(string name, Mediator mediator) : base(name, mediator)
    {
    }

    public override void Notify(string message)
    {
        Console.WriteLine("Робітник {0} отримав повідомлення: {1}", Name, message);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Secretary secretary = new Secretary();

        Boss boss = new Boss("Олександр", secretary);
        secretary.SetBoss(boss);

        Worker worker1 = new Worker("Іван", secretary);
        Worker worker2 = new Worker("Марія", secretary);
        Worker worker3 = new Worker("Петро", secretary);
        secretary.AddColleague(worker1);
        secretary.AddColleague(worker2);
        secretary.AddColleague(worker3);

        boss.Send("Завтра о 10:00 відбудеться мітинг. Підтвердіть свою участь.");

        worker1.Send("Так");
        worker2.Send("Так");
        worker3.Send("Ні");
    }
}
