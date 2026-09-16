namespace SpaceOOP.Models;

/// <summary>
/// Абстрактный базовый класс космического тела.
/// Инкапсуляция: поле mass закрыто, доступ через свойство Mass с проверкой.
/// </summary>
public abstract class CelestialBody
{
    private double mass;

    public string Name { get; }

    public double Radius { get; }

    public double Mass
    {
        get => mass;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Масса должна быть больше нуля.");

            mass = value;
        }
    }

    protected CelestialBody(string name, double mass, double radius)
    {
        Name = name;
        Mass = mass;
        Radius = radius;
    }

    /// <summary>Общие характеристики тела (используется наследниками).</summary>
    protected void ShowCommonInfo(string title)
    {
        Console.WriteLine($"{title}: {Name}");
        Console.WriteLine($"Масса: {Mass:E2} кг");
        Console.WriteLine($"Радиус: {Radius:N0} км");
    }

    /// <summary>Полиморфизм: каждый класс описывает себя по-своему.</summary>
    public abstract void ShowInfo();
}
