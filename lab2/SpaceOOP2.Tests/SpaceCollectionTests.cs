using SpaceOOP2.Collections;
using SpaceOOP2.Models;

namespace SpaceOOP2.Tests;

// Тесты поведения собственной обобщённой коллекции SpaceCollection<T>.
// Используются реальные типы предметной области (Planet, Star, Satellite),
// т.к. ограничение where T : CelestialBody не даёт взять что-то проще.
public class SpaceCollectionTests
{
    // Вспомогательные фабрики: у планет важна только различимость по имени.
    private static Planet NewPlanet(string name) =>
        new(name, mass: 5.0e24, radius: 6371, distanceFromStar: 150, orbitalPeriod: 365, hasAtmosphere: true);

    private static Star NewStar(string name) =>
        new(name, mass: 2.0e30, radius: 696340, temperature: 5778, spectralClass: "G2V");

    [Fact]
    public void Add_IncreasesCount_AndContainsFindsItem()
    {
        var collection = new SpaceCollection<Planet>();
        Planet earth = NewPlanet("Земля");

        Assert.Empty(collection);

        collection.Add(earth);

        Assert.Single(collection); // Single проверяет, что элемент ровно один
        Assert.Contains(earth, collection);
    }

    [Fact]
    public void Remove_ReturnsTrueForExisting_AndFalseForMissing()
    {
        var collection = new SpaceCollection<Planet>();
        Planet earth = NewPlanet("Земля");
        Planet mars = NewPlanet("Марс");
        collection.Add(earth);

        Assert.True(collection.Remove(earth));
        Assert.Empty(collection);

        Assert.False(collection.Remove(mars)); // элемента нет в коллекции
        Assert.False(collection.Remove(earth)); // уже удалён
    }

    [Fact]
    public void Indexer_AndRemoveAt_ThrowOnInvalidIndex()
    {
        var collection = new SpaceCollection<Planet>();
        collection.Add(NewPlanet("Земля"));

        // Допустимые индексы: 0..Count-1, всё остальное: исключение
        Assert.Throws<ArgumentOutOfRangeException>(() => collection[-1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => collection[1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => collection.RemoveAt(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => collection.RemoveAt(5));
    }

    [Fact]
    public void Add_GrowsBeyondInitialCapacity()
    {
        // Начальная ёмкость пустой коллекции равна 0, далее массив растёт
        // с удвоением (4, 8, 16...). Добавляем заметно больше элементов.
        var collection = new SpaceCollection<CelestialBody>();

        for (int i = 0; i < 20; i++)
            collection.Add(NewPlanet($"Планета {i}"));

        Assert.Equal(20, collection.Count);
        for (int i = 0; i < 20; i++)
            Assert.Equal($"Планета {i}", collection[i].Name);
    }

    [Fact]
    public void Clone_KeepsCount_AfterOriginalRemoved()
    {
        var original = new SpaceCollection<CelestialBody>();
        original.Add(NewStar("Солнце"));
        original.Add(NewPlanet("Земля"));

        var clone = (SpaceCollection<CelestialBody>)original.Clone();

        original.RemoveAt(0);

        Assert.Single(original);
        Assert.Equal(2, clone.Count); // состав клона не зависит от оригинала
    }

    [Fact]
    public void Foreach_AndCopyTo_ReturnItemsInInsertionOrder()
    {
        var collection = new SpaceCollection<CelestialBody>();
        collection.Add(NewStar("Солнце"));
        collection.Add(NewPlanet("Земля"));
        collection.Add(NewPlanet("Марс"));

        // Порядок через foreach
        var names = new List<string>();
        foreach (CelestialBody body in collection)
            names.Add(body.Name);

        Assert.Equal(new[] { "Солнце", "Земля", "Марс" }, names);

        // Тот же порядок через CopyTo в массив
        var copy = new CelestialBody[collection.Count];
        collection.CopyTo(copy, 0);

        Assert.Equal(new[] { "Солнце", "Земля", "Марс" }, copy.Select(b => b.Name).ToArray());
    }

    [Fact]
    public void Covariance_SpaceReaderOfDerived_AssignsToBase()
    {
        // Ковариантность ISpaceReader<out T>: «читатель планет» подходит
        // как «читатель любых тел». Проверяем, что это работает в рантайме
        // (компилируемость самой строки уже доказана сборкой теста).
        var planets = new SpaceCollection<Planet>();
        planets.Add(NewPlanet("Земля"));
        planets.Add(NewPlanet("Марс"));

        ISpaceReader<Planet> planetReader = planets;
        ISpaceReader<CelestialBody> bodyReader = planetReader;

        Assert.Equal(planetReader.Count, bodyReader.Count);
        Assert.Equal(2, bodyReader.Count);
        Assert.All(bodyReader.Items, item => Assert.IsType<Planet>(item));
    }
}
