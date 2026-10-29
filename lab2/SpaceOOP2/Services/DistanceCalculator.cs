using SpaceOOP2.Models;

namespace SpaceOOP2.Services;

public class DistanceCalculator
{
    public double CalculateDistance(CelestialBody first, CelestialBody second)
    {
        if (first is not Planet firstPlanet || second is not Planet secondPlanet)
            throw new ArgumentException("Расстояние можно рассчитать только между планетами.");

        return Math.Abs(firstPlanet.DistanceFromStar - secondPlanet.DistanceFromStar);
    }

    public void ShowDistance(CelestialBody first, CelestialBody second)
    {
        double distance = CalculateDistance(first, second);
        Console.WriteLine($"Расстояние между {first.Name} и {second.Name}: {distance:N1} млн км");
    }
}
