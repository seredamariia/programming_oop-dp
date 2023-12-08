// Абстрактні класи предметів
abstract class Equipment
{
    public abstract string Name { get; }
    public abstract int Durability { get; }
}

abstract class Potion
{
    public abstract string Name { get; }
    public abstract int Duration { get; }
}

abstract class Weapon
{
    public abstract string Name { get; }
    public abstract int Power { get; }
}

// Абстрактні фабрики
abstract class PlayerFactory
{
    public abstract Equipment CreateEquipment();
    public abstract Potion CreatePotion();
    public abstract Weapon CreateWeapon();
}

// Конкретні класи предметів
class FreePlayerEquipment : Equipment
{
    public override string Name => "Basic Armor";
    public override int Durability => 50;
}

class FreePlayerPotion : Potion
{
    public override string Name => "Healing Potion";
    public override int Duration => 3;
}

class FreePlayerWeapon : Weapon
{
    public override string Name => "Wooden Sword";
    public override int Power => 10;
}

class PremiumPlayerEquipment : Equipment
{
    public override string Name => "Advanced Armor";
    public override int Durability => 100;
}

class PremiumPlayerPotion : Potion
{
    public override string Name => "Greater Healing Potion";
    public override int Duration => 5;
}

class PremiumPlayerWeapon : Weapon
{
    public override string Name => "Steel Sword";
    public override int Power => 20;
}

// Конкретні фабрики
class FreePlayerFactory : PlayerFactory
{
    public override Equipment CreateEquipment() => new FreePlayerEquipment();
    public override Potion CreatePotion() => new FreePlayerPotion();
    public override Weapon CreateWeapon() => new FreePlayerWeapon();
}

class PremiumPlayerFactory : PlayerFactory
{
    public override Equipment CreateEquipment() => new PremiumPlayerEquipment();
    public override Potion CreatePotion() => new PremiumPlayerPotion();
    public override Weapon CreateWeapon() => new PremiumPlayerWeapon();
}

// Використання фабрик
class Player
{
    private readonly PlayerFactory _factory;
    public Equipment Equipment { get; private set; }
    public Potion Potion { get; private set; }
    public Weapon Weapon { get; private set; }

    public Player(PlayerFactory factory)
    {
        _factory = factory;
        Equipment = _factory.CreateEquipment();
        Potion = _factory.CreatePotion();
        Weapon = _factory.CreateWeapon();
    }

    public void DisplayItems()
    {
        Console.WriteLine($"Equipment: {Equipment.Name}, Durability: {Equipment.Durability}");
        Console.WriteLine($"Potion: {Potion.Name}, Duration: {Potion.Duration}");
        Console.WriteLine($"Weapon: {Weapon.Name}, Power: {Weapon.Power}");
    }
}

class Program
{
    static void Main()
    {
        PlayerFactory freePlayerFactory = new FreePlayerFactory();
        Player freePlayer = new Player(freePlayerFactory);
        Console.WriteLine("Free Player Items:");
        freePlayer.DisplayItems();

        Console.WriteLine();

        PlayerFactory premiumPlayerFactory = new PremiumPlayerFactory();
        Player premiumPlayer = new Player(premiumPlayerFactory);
        Console.WriteLine("Premium Player Items:");
        premiumPlayer.DisplayItems();
    }
}
