using SpaceOOP2.Collections;
using SpaceOOP2.Models;

namespace SpaceOOP2.Systems;

/// <summary>
/// Звёздная система: центральный класс предметной области.
///
/// ГЛАВНОЕ ОТЛИЧИЕ ОТ ПЕРВОЙ ЛАБОРАТОРНОЙ: тела хранятся не в
/// List&lt;CelestialBody&gt;, а в нашей собственной обобщённой коллекции
/// SpaceCollection&lt;CelestialBody&gt;: так коллекция «внедрена в проект».
/// </summary>
public class StarSystem
{
    // Собственная обобщённая коллекция вместо стандартного List<T>.
    // T = CelestialBody: можно хранить звёзды, планеты и спутники вместе.
    private readonly SpaceCollection<CelestialBody> bodies = new();

    public string Name { get; }

    public int ObjectCount => bodies.Count;

    // Наружу отдаём не саму коллекцию, а защитную копию только для чтения:
    // bodies.ToArray() создаёт новый массив, Array.AsReadOnly оборачивает
    // его в ReadOnlyCollection. Привести результат обратно к SpaceCollection
    // и изменить систему снаружи уже нельзя.
    public IReadOnlyList<CelestialBody> Objects => Array.AsReadOnly(bodies.ToArray());

    public StarSystem(string name)
    {
        Name = name;
    }

    public void AddObject(CelestialBody body) => bodies.Add(body);

    public void RemoveObject(string name)
    {
        CelestialBody? body = FindObject(name);
        if (body != null)
            bodies.Remove(body);
    }

    // Поиск по названию без учёта регистра. Возвращает null, если не найдено.
    public CelestialBody? FindObject(string name)
    {
        // foreach работает, потому что SpaceCollection<T> реализует IEnumerable<T>
        foreach (CelestialBody body in bodies)
        {
            if (body.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return body;
        }

        return null;
    }

    // Полиморфизм: вызываем ShowInfo() у базового типа CelestialBody,
    // а выполняется версия конкретного класса (Star/Planet/Satellite).
    public void ShowAllObjects()
    {
        Console.WriteLine($"\n=== {Name} ===\n");
        foreach (CelestialBody body in bodies)
        {
            body.ShowInfo();
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Обобщённый метод с ограничением where TResult : CelestialBody.
    /// Возвращает коллекцию только тех тел, которые имеют тип TResult.
    /// Ограничение нужно, чтобы результат можно было положить в
    /// SpaceCollection&lt;TResult&gt; (у неё то же самое ограничение).
    /// </summary>
    public SpaceCollection<TResult> GetObjectsOfType<TResult>()
        where TResult : CelestialBody
    {
        SpaceCollection<TResult> result = new();

        foreach (CelestialBody body in bodies)
        {
            // Проверка типа «is» + приведение: если тело нужного типа, добавляем
            if (body is TResult typed)
                result.Add(typed);
        }

        return result;
    }

    // Примеры вызова обобщённого метода: тип подставляется в <...>
    public SpaceCollection<Planet> GetPlanets() => GetObjectsOfType<Planet>();

    public SpaceCollection<Satellite> GetSatellites() => GetObjectsOfType<Satellite>();

    // Возвращает List<Orbit>, а не SpaceCollection<Orbit>, потому что Orbit
    // не является космическим телом и не проходит ограничение
    // where T : CelestialBody нашей коллекции.
    // OfType<T>() здесь: стандартный обобщённый LINQ-метод, оставляет только
    // элементы указанного типа (только «орбитальные» объекты).
    public List<Orbit> GetOrbits() =>
        bodies.OfType<IOrbitalObject>()
              .Select(o => new Orbit((CelestialBody)o, o.OrbitRadius, o.OrbitalPeriod))
              .ToList();

    // Клон коллекции тел: используется в демонстрации ICloneable (пункт меню 10).
    public SpaceCollection<CelestialBody> CloneBodies() =>
        (SpaceCollection<CelestialBody>)bodies.Clone();
}
