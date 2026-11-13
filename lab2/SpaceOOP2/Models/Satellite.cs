namespace SpaceOOP2.Models;

/// <summary>
/// Спутник - наследник CelestialBody, реализует IOrbitalObject,
/// т.к. спутник обращается вокруг родительского тела (планеты).
/// </summary>
public class Satellite : CelestialBody, IOrbitalObject
{
    public string ParentObject { get; } // название тела, вокруг которого обращается

    public double DistanceFromParent { get; } // расстояние до родителя, млн км

    public double OrbitalPeriod { get; } // период обращения, дни (из IOrbitalObject)

    // Радиус орбиты для интерфейса - расстояние до родительского тела.
    public double OrbitRadius => DistanceFromParent;

    public Satellite(
        string name,
        double mass,
        double radius,
        string parentObject,
        double distanceFromParent,
        double orbitalPeriod)
        : base(name, mass, radius)
    {
        ParentObject = parentObject;
        DistanceFromParent = distanceFromParent;
        OrbitalPeriod = orbitalPeriod;
    }

    public override void ShowInfo()
    {
        ShowCommonInfo("Спутник");
        Console.WriteLine($"Родительский объект: {ParentObject}");
        Console.WriteLine($"Период обращения: {OrbitalPeriod:N2} дней");
    }

    public void ShowOrbitInfo()
    {
        Console.WriteLine($"Спутник {Name} обращается вокруг {ParentObject} за {OrbitalPeriod:N2} дней.");
    }
}
