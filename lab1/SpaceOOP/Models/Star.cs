namespace SpaceOOP.Models;

public class Star : CelestialBody
{
    public double Temperature { get; }

    public string SpectralClass { get; }

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
