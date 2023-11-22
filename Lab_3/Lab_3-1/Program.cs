using System.Text;

public abstract class AbstractFormatter
{
    protected abstract string FormatTime(DateTime time);
    protected abstract string FormatDate(DateTime date);
    protected abstract string FormatCurrency(decimal amount);
    protected abstract string FormatProductName(string productName);
    protected string Message;

    public virtual string FormatReceipt(Receipt receipt)
    {
        return $"{FormatTime(receipt.Time)}\n{FormatDate(receipt.Date)}\n{FormatProductName(receipt.ProductName)}\n{FormatCurrency(receipt.Amount)}\n{receipt.Message}";
    }
}

public class UkrainianFormatter : AbstractFormatter
{
    protected override string FormatTime(DateTime time) => $"{time.Hour}:{time.Minute}:{time.Second}";
    protected override string FormatDate(DateTime date) => $"{date.Day}.{date.Month}.{date.Year}";
    protected override string FormatCurrency(decimal amount) => $"{amount * 36} грн";
    protected override string FormatProductName(string productName) => productName;
    protected string Message = "Дякуємо за покупку!";
}

public class USAFormatter : AbstractFormatter
{
    protected override string FormatTime(DateTime time) => $"{time.Hour}:{time.Minute}:{time.Second}";
    protected override string FormatDate(DateTime date) => $"{date.Month}.{date.Day}.{date.Year}";
    protected override string FormatCurrency(decimal amount) => $"${amount}";
    protected override string FormatProductName(string productName) => productName;
    protected string Message = "Thank you for shopping!";
}

public class JapaneseFormatter : AbstractFormatter
{
    protected override string FormatTime(DateTime time) => $"{time.Hour}時{time.Minute}分{time.Second}秒";
    protected override string FormatDate(DateTime date) => $"{date.Year}年{date.Month}月{date.Day}日";
    protected override string FormatCurrency(decimal amount) => $"円{amount * 150}";
    protected override string FormatProductName(string productName) => productName;
    protected string Message = "ご購入ありがとうございます！";
}

public class Receipt
{
    public DateTime Time { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string ProductName { get; set; }
    public string Message { get; set; }
}

public interface IShoppingCart
{
    void Buy(string productName, decimal amount);
}

public class ShoppingCart : IShoppingCart
{
    private AbstractFormatter _formatter;

    public ShoppingCart(AbstractFormatter formatter)
    {
        _formatter = formatter;
    }

    public void Buy(string productName, decimal amount)
    {
        Receipt receipt = new Receipt
        {
            Time = DateTime.Now,
            Date = DateTime.Now,
            Amount = amount,
            ProductName = productName
        };

        Console.WriteLine(_formatter.FormatReceipt(receipt));
    }
}


class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        List<ShoppingCart> passengers = new List<ShoppingCart>{
                new ShoppingCart(new UkrainianFormatter()),
                new ShoppingCart(new USAFormatter()),
                new ShoppingCart(new JapaneseFormatter())
            };
        foreach (ShoppingCart sc in passengers)
        {
            sc.Buy("JUJUTSU KAISEN manga: all volumes", 100);
        }
    }
}