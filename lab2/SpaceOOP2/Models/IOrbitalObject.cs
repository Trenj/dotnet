namespace SpaceOOP2.Models;

/// <summary>
/// Интерфейс «объект, у которого есть орбита».
/// Реализуют Planet и Satellite; Star - нет.
/// Позволяет работать с орбитальными характеристиками единообразно,
/// не зная конкретного класса объекта.
/// </summary>
public interface IOrbitalObject
{
    double OrbitalPeriod { get; } // период обращения, дни

    double OrbitRadius { get; } // радиус орбиты, млн км

    void ShowOrbitInfo();
}
