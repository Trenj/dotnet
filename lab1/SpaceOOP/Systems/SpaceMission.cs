using SpaceOOP.Models;

namespace SpaceOOP.Systems;

/// <summary>Космическая миссия между двумя объектами.</summary>
public class SpaceMission
{
    public string Name { get; }

    public CelestialBody StartPoint { get; }

    public CelestialBody Destination { get; }

    /// <summary>Статус меняется только через Start/Complete.</summary>
    public string Status { get; private set; }

    public SpaceMission(string name, CelestialBody startPoint, CelestialBody destination)
    {
        Name = name;
        StartPoint = startPoint;
        Destination = destination;
        Status = "Запланирована";
    }

    public void Start() => Status = "Выполняется";

    public void Complete() => Status = "Завершена";

    public void ShowInfo()
    {
        Console.WriteLine($"\n=== КОСМИЧЕСКАЯ МИССИЯ ===");
        Console.WriteLine($"Название: {Name}");
        Console.WriteLine($"Отправление: {StartPoint.Name}");
        Console.WriteLine($"Назначение: {Destination.Name}");
        Console.WriteLine($"Статус: {Status}");
    }
}
