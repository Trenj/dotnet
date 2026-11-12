namespace SpaceOOP2.Models;

/// <summary>
/// Абстрактный базовый класс космического тела — корень иерархии:
///     CelestialBody
///     ├── Star
///     ├── Planet
///     └── Satellite
///
/// Именно он используется как ограничение where T : CelestialBody
/// в обобщённой коллекции SpaceCollection&lt;T&gt;.
///
/// Инкапсуляция: поле mass закрыто, доступ — через свойство Mass с проверкой.
/// </summary>
public abstract class CelestialBody
{
    // Закрытое поле: снаружи класса изменить массу напрямую нельзя.
    private double mass;

    public string Name { get; }

    public double Radius { get; }

    public double Mass
    {
        get => mass;
        set
        {
            // Проверка корректности — пример инкапсуляции:
            // класс не даёт записать в себя некорректные данные.
            if (value <= 0)
                throw new ArgumentException("Масса должна быть больше нуля.");

            mass = value;
        }
    }

    // protected — конструктор доступен только наследникам,
    // создать «просто CelestialBody» нельзя (класс абстрактный).
    protected CelestialBody(string name, double mass, double radius)
    {
        Name = name;
        Mass = mass; // присваиваем через свойство — сработает проверка
        Radius = radius;
    }

    // Общий вывод характеристик, общий для всех наследников.
    protected void ShowCommonInfo(string title)
    {
        Console.WriteLine($"{title}: {Name}");
        Console.WriteLine($"Масса: {Mass:E2} кг");
        Console.WriteLine($"Радиус: {Radius:N0} км");
    }

    // Абстрактный метод: тела нет, каждый наследник ОБЯЗАН
    // реализовать его по-своему (полиморфизм).
    public abstract void ShowInfo();
}
