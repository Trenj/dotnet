namespace SpaceOOP2.Models;

/// <summary>
/// Звезда - наследник CelestialBody. Интерфейс IOrbitalObject не реализует,
/// т.к. звезда - центр системы и ни вокруг чего не обращается.
/// </summary>
public class Star : CelestialBody
{
    public double Temperature { get; } // температура поверхности, К

    public string SpectralClass { get; } // спектральный класс (например, G2V у Солнца)

    public Star(string name, double mass, double radius, double temperature, string spectralClass)
        : base(name, mass, radius)
    {
        Temperature = temperature;
        SpectralClass = spectralClass;
    }

    public override void ShowInfo()
    {
        ShowCommonInfo("Звезда");
        Console.WriteLine($"Температура: {Temperature:N0} K");
        Console.WriteLine($"Спектральный класс: {SpectralClass}");
    }
}
