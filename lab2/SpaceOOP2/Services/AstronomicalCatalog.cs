using SpaceOOP2.Models;

namespace SpaceOOP2.Services;

/// <summary>
/// Отдельный каталог космических тел с поиском по названию.
/// (Остался с первой лабораторной; внутри использует стандартный List&lt;T&gt;.)
/// </summary>
public class AstronomicalCatalog
{
    private readonly List<CelestialBody> objects = new();

    public int Count => objects.Count;

    public void Add(CelestialBody body) => objects.Add(body);

    // Поиск без учёта регистра; null, если объект не найден.
    public CelestialBody? FindByName(string name) =>
        objects.Find(body => body.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public void ShowCatalog()
    {
        Console.WriteLine("\n=== АСТРОНОМИЧЕСКИЙ КАТАЛОГ ===");
        foreach (CelestialBody body in objects)
            Console.WriteLine($"{body.GetType().Name}: {body.Name}");
    }
}
