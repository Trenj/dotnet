using SpaceOOP.Models;

namespace SpaceOOP.Systems;

/// <summary>Орбитальные характеристики космического объекта.</summary>
public class Orbit
{
    public CelestialBody Body { get; }

    public double Radius { get; }

    public double Period { get; }

    public Orbit(CelestialBody body, double radius, double period)
    {
        Body = body;
        Radius = radius;
        Period = period;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Орбита объекта: {Body.Name}");
        Console.WriteLine($"Радиус орбиты: {Radius:N1} млн км");
        Console.WriteLine($"Период обращения: {Period:N2} дней");
    }
}
