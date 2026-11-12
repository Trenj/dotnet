namespace SpaceOOP2.Models;

/// <summary>
/// Планета — наследник CelestialBody, реализует IOrbitalObject,
/// т.к. у планеты есть орбита вокруг звезды.
/// </summary>
public class Planet : CelestialBody, IOrbitalObject
{
    public double DistanceFromStar { get; } // расстояние до звезды, млн км

    public double OrbitalPeriod { get; } // период обращения, дни (из IOrbitalObject)

    public bool HasAtmosphere { get; }

    // Радиус орбиты для интерфейса — это и есть расстояние до звезды.
    public double OrbitRadius => DistanceFromStar;

    // base(...) передаёт общие характеристики в конструктор CelestialBody.
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

    // override — своя реализация абстрактного метода базового класса.
    public override void ShowInfo()
    {
        ShowCommonInfo("Планета");
        Console.WriteLine($"Расстояние от звезды: {DistanceFromStar:N1} млн км");
        Console.WriteLine($"Период обращения: {OrbitalPeriod:N2} дней");
        Console.WriteLine($"Атмосфера: {(HasAtmosphere ? "есть" : "нет")}");
    }

    // Реализация интерфейса IOrbitalObject.
    public void ShowOrbitInfo()
    {
        Console.WriteLine($"Планета {Name} совершает полный оборот за {OrbitalPeriod:N2} дней.");
    }
}
