using SpaceOOP.Models;

namespace SpaceOOP.Services;

/// <summary>Каталог космических объектов с поиском по названию.</summary>
public class AstronomicalCatalog
{
    private readonly List<CelestialBody> objects = new();

    public int Count => objects.Count;

    public void Add(CelestialBody body) => objects.Add(body);

    public CelestialBody? FindByName(string name) =>
        objects.Find(body => body.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public void ShowCatalog()
    {
        Console.WriteLine("\n=== АСТРОНОМИЧЕСКИЙ КАТАЛОГ ===");
        foreach (CelestialBody body in objects)
            Console.WriteLine($"{body.GetType().Name}: {body.Name}");
    }
}
