using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Клас, що представляє студента
class Student
{
    public string Name { get; set; } // прізвище студента
    public int DebtCount { get; set; } // кількість предметів, які складають заборгованість
    public double DebtAmount { get; set; } // сума фінансового боргу
}

// Інтерфейс, що описує алгоритм відбору студентів
interface ISelectionStrategy
{
    List<Student> Select(List<Student> students); // метод, що повертає список відібраних студентів
}

// Клас, що реалізує алгоритм відбору студентів за наявністю навчальної заборгованості
class AcademicDebtSelection : ISelectionStrategy
{
    public List<Student> Select(List<Student> students)
    {
        return students.Where(s => s.DebtCount > 0).ToList();
    }
}

// Клас, що реалізує алгоритм відбору студентів за наявністю фінансової заборгованості
class FinancialDebtSelection : ISelectionStrategy
{
    public List<Student> Select(List<Student> students)
    {
        return students.Where(s => s.DebtAmount > 0).ToList();
    }
}

// Інтерфейс, що описує алгоритм сортування студентів
interface ISortStrategy
{
    List<Student> Sort(List<Student> students);
}

// Клас, що реалізує алгоритм сортування студентів за прізвищем в алфавітному порядку
class NameSort : ISortStrategy
{
    public List<Student> Sort(List<Student> students)
    {
        return students.OrderBy(s => s.Name).ToList();
    }
}

// Клас, що реалізує алгоритм сортування студентів за кількістю предметів, які складають заборгованість
class DebtCountSort : ISortStrategy
{
    public List<Student> Sort(List<Student> students)
    {
        return students.OrderBy(s => s.DebtCount).ToList();
    }
}

// Клас, що реалізує алгоритм сортування студентів за сумою фінансового боргу
class DebtAmountSort : ISortStrategy
{
    public List<Student> Sort(List<Student> students)
    {
        return students.OrderBy(s => s.DebtAmount).ToList();
    }
}

// Інтерфейс, що описує алгоритм друку списку студентів
interface IPrintStrategy
{
    void Print(List<Student> students);
}

// Клас, що реалізує алгоритм друку списку студентів у консоль
class ConsolePrint : IPrintStrategy
{
    public void Print(List<Student> students)
    {
        foreach (var s in students)
        {
            Console.WriteLine($"{s.Name}: {s.DebtCount}, {s.DebtAmount}");
        }
    }
}

// Клас, що представляє контекст, у якому використовуються різні стратегії
class StudentListContext
{
    private ISelectionStrategy selectionStrategy;
    private ISortStrategy sortStrategy;
    private IPrintStrategy printStrategy;

    public StudentListContext(ISelectionStrategy selectionStrategy, ISortStrategy sortStrategy, IPrintStrategy printStrategy)
    {
        this.selectionStrategy = selectionStrategy;
        this.sortStrategy = sortStrategy;
        this.printStrategy = printStrategy;
    }

    public void GetStudentList(List<Student> students)
    {
        var selectedStudents = selectionStrategy.Select(students);
        var sortedStudents = sortStrategy.Sort(selectedStudents);
        printStrategy.Print(sortedStudents);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var students = new List<Student>
        {
            new Student { Name = "Іваненко Олександр", DebtCount = 2, DebtAmount = 1000 },
            new Student { Name = "Петренко Юлія", DebtCount = 0, DebtAmount = 500 },
            new Student { Name = "Ковальчук Віталій", DebtCount = 1, DebtAmount = 0 },
            new Student { Name = "Сидоренко Анна", DebtCount = 0, DebtAmount = 0 },
            new Student { Name = "Мельник Дмитро", DebtCount = 3, DebtAmount = 1500 }
        };

        // створюємо різні варіанти списків за допомогою різних комбінацій стратегій
        var list1 = new StudentListContext(new AcademicDebtSelection(), new NameSort(), new ConsolePrint());
        var list2 = new StudentListContext(new FinancialDebtSelection(), new DebtAmountSort(), new ConsolePrint());
        var list3 = new StudentListContext(new AcademicDebtSelection(), new DebtCountSort(), new ConsolePrint());

        Console.WriteLine("Список студентів з навчальною заборгованістю, відсортований за прізвищем:");
        list1.GetStudentList(students);
        Console.WriteLine();

        Console.WriteLine("Список студентів з фінансовою заборгованістю, відсортований за сумою фінансового боргу:");
        list2.GetStudentList(students);
        Console.WriteLine();

        Console.WriteLine("Список студентів з навчальною заборгованістю, відсортований за кількістю предметів, які складають заборгованість:");
        list3.GetStudentList(students);
        Console.WriteLine();
    }
}