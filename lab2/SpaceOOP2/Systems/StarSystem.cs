using SpaceOOP2.Collections;
using SpaceOOP2.Models;

namespace SpaceOOP2.Systems;

/// <summary>
/// Звёздная система. В отличие от первой лабораторной, тела хранятся
/// в собственной обобщённой коллекции SpaceCollection&lt;T&gt;.
/// </summary>
public class StarSystem
{
    private readonly SpaceCollection<CelestialBody> bodies = new();

    public string Name { get; }

    public int ObjectCount => bodies.Count;

    public IReadOnlyList<CelestialBody> Objects => bodies;

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

    public CelestialBody? FindObject(string name)
    {
        foreach (CelestialBody body in bodies)
        {
            if (body.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return body;
        }

        return null;
    }

    public void ShowAllObjects()
    {
        Console.WriteLine($"\n=== {Name} ===\n");
        foreach (CelestialBody body in bodies)
        {
            body.ShowInfo();
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Обобщённый метод с ограничением: TResult должен быть космическим телом.
    /// </summary>
    public SpaceCollection<TResult> GetObjectsOfType<TResult>()
        where TResult : CelestialBody
    {
        SpaceCollection<TResult> result = new();

        foreach (CelestialBody body in bodies)
        {
            if (body is TResult typed)
                result.Add(typed);
        }

        return result;
    }

    public SpaceCollection<Planet> GetPlanets() => GetObjectsOfType<Planet>();

    public SpaceCollection<Satellite> GetSatellites() => GetObjectsOfType<Satellite>();

    public List<Orbit> GetOrbits() =>
        bodies.OfType<IOrbitalObject>()
              .Select(o => new Orbit((CelestialBody)o, o.OrbitRadius, o.OrbitalPeriod))
              .ToList();

    public SpaceCollection<CelestialBody> CloneBodies() =>
        (SpaceCollection<CelestialBody>)bodies.Clone();
}
