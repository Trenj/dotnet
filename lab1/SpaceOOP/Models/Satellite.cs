namespace SpaceOOP.Models;

public class Satellite : CelestialBody, IOrbitalObject
{
    public string ParentObject { get; }

    public double DistanceFromParent { get; }

    public double OrbitalPeriod { get; }

    /// <summary>Радиус орбиты спутника — расстояние до родительского объекта.</summary>
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
