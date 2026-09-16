namespace SpaceOOP.Models;

public class Planet : CelestialBody, IOrbitalObject
{
    public double DistanceFromStar { get; }

    public double OrbitalPeriod { get; }

    public bool HasAtmosphere { get; }

    /// <summary>Радиус орбиты планеты — расстояние от звезды.</summary>
    public double OrbitRadius => DistanceFromStar;

    public Planet(
        string name,
        double mass,
        double radius,
        double distanceFromStar,
        double orbitalPeriod,
        bool hasAtmosphere)
        : base(name, mass, radius)
    {
        DistanceFromStar = distanceFromStar;
        OrbitalPeriod = orbitalPeriod;
        HasAtmosphere = hasAtmosphere;
    }

    public override void ShowInfo()
    {
        ShowCommonInfo("Планета");
        Console.WriteLine($"Расстояние от звезды: {DistanceFromStar:N1} млн км");
        Console.WriteLine($"Период обращения: {OrbitalPeriod:N2} дней");
        Console.WriteLine($"Атмосфера: {(HasAtmosphere ? "есть" : "нет")}");
    }

    public void ShowOrbitInfo()
    {
        Console.WriteLine($"Планета {Name} совершает полный оборот за {OrbitalPeriod:N2} дней.");
    }
}
