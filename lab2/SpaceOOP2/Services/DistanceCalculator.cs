using SpaceOOP2.Models;

namespace SpaceOOP2.Services;

/// <summary>
/// Расчёт расстояния между планетами (упрощённая модель:
/// разница расстояний от звезды, т.е. «в одну линию»).
/// </summary>
public class DistanceCalculator
{
    public double CalculateDistance(CelestialBody first, CelestialBody second)
    {
        // Паттерн «is not Planet firstPlanet»: если хотя бы одно тело
        // не планета — расчёт не имеет смысла, бросаем исключение.
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
