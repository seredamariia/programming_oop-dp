using System.Text;

public interface ILibraryCard
{
    string GetInfo();
}

public class StudentCard : ILibraryCard
{
    public string GetInfo()
    {
        return "Це студентський квиток. Він дійсний протягом одного року.";
    }
}

public class SchoolCard : ILibraryCard
{
    public string GetInfo()
    {
        return "Це шкільний квиток. Він дійсний протягом шести місяців.";
    }
}

public class ResearchCard : ILibraryCard
{
    public string GetInfo()
    {
        return "Це науковий квиток. Він дійсний протягом трьох років і дає доступ до спеціальних ресурсів.";
    }
}

public class AcademicCard : ILibraryCard
{
    public string GetInfo()
    {
        return "Це академічний квиток. Він дійсний протягом п'яти років і дає безмежні можливості для читання.";
    }
}

public class CardMachine
{
    public ILibraryCard FactoryMethod(string cardType)
    {
        switch (cardType.ToLower())
        {
            case "student":
                return new StudentCard();
            case "school":
                return new SchoolCard();
            case "research":
                return new ResearchCard();
            case "academic":
                return new AcademicCard();
            default:
                throw new ArgumentException("Невідомий тип квитка");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        CardMachine cardMachine = new CardMachine();
        ILibraryCard libraryCard;

        string[] cards = ["school", "academic", "research", "student"];

        foreach (string card in cards)
        {
            libraryCard = cardMachine.FactoryMethod(card);
            Console.WriteLine(libraryCard.GetInfo());
        }
    }
}
