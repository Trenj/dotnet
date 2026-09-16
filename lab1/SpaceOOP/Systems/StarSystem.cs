using SpaceOOP.Models;

namespace SpaceOOP.Systems;

/// <summary>
/// Звёздная система: хранит космические тела, позволяет добавлять,
/// удалять, искать объекты и получать их орбиты.
/// </summary>
public class StarSystem
{
    private readonly List<CelestialBody> bodies = new();

    public string Name { get; }

    public int ObjectCount => bodies.Count;

    /// <summary>Только для чтения: все тела системы.</summary>
    public IReadOnlyList<CelestialBody> Objects => bodies.AsReadOnly();

    public StarSystem(string name)
    {
        Name = name;
    }

    public void AddObject(CelestialBody body) => bodies.Add(body);

    public void RemoveObject(string name)
    {
        CelestialBody? body = FindObject(name);
        if (body != null)
            bodies.Remove(body);
    }

    /// <summary>Поиск по имени без учёта регистра.</summary>
    public CelestialBody? FindObject(string name) =>
        bodies.Find(body => body.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    /// <summary>Полиморфизм: у каждого тела вызывается своя реализация ShowInfo.</summary>
    public void ShowAllObjects()
    {
        Console.WriteLine($"\n=== {Name} ===\n");
        foreach (CelestialBody body in bodies)
        {
            body.ShowInfo();
            Console.WriteLine();
        }
    }

    public List<Planet> GetPlanets() => bodies.OfType<Planet>().ToList();

    public List<Satellite> GetSatellites() => bodies.OfType<Satellite>().ToList();

    /// <summary>Строит орбиты всех объектов, реализующих IOrbitalObject.</summary>
    public List<Orbit> GetOrbits() =>
        bodies.OfType<IOrbitalObject>()
              .Select(o => new Orbit((CelestialBody)o, o.OrbitRadius, o.OrbitalPeriod))
              .ToList();
}
