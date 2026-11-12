using SpaceOOP2.Models;

namespace SpaceOOP2.Systems;

/// <summary>
/// Орбита конкретного космического тела: радиус и период обращения.
/// </summary>
public class Orbit
{
    public CelestialBody Body { get; } // тело, которому принадлежит орбита

    public double Radius { get; } // радиус орбиты, млн км

    public double Period { get; } // период обращения, дни

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
