namespace SpaceOOP2.Models;

public interface IOrbitalObject
{
    double OrbitalPeriod { get; }

    double OrbitRadius { get; }

    void ShowOrbitInfo();
}
