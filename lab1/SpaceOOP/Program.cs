using SpaceOOP.Models;
using SpaceOOP.Services;
using SpaceOOP.Systems;

// ================================
// ДАННЫЕ: СОЛНЕЧНАЯ СИСТЕМА
// ================================

StarSystem solarSystem = new("Солнечная система");

solarSystem.AddObject(new Star("Солнце", 1.989e30, 696340, 5778, "G2V"));
solarSystem.AddObject(new Planet("Меркурий", 3.301e23, 2439.7, 57.9, 87.97, false));
solarSystem.AddObject(new Planet("Земля", 5.972e24, 6371, 149.6, 365.25, true));
solarSystem.AddObject(new Planet("Марс", 6.417e23, 3389.5, 227.9, 687, true));
solarSystem.AddObject(new Satellite("Луна", 7.342e22, 1737.4, "Земля", 0.384, 27.3));

AstronomicalCatalog catalog = new();
foreach (CelestialBody body in solarSystem.Objects)
    catalog.Add(body);

DistanceCalculator calculator = new();
SpaceMission mission = new("Mars Exploration", solarSystem.FindObject("Земля")!, solarSystem.FindObject("Марс")!);

// ================================
// ГЛАВНЫЙ ЦИКЛ МЕНЮ
// ================================

bool running = true;

while (running)
{
    ShowMenu();

    switch (ReadInput("Выберите действие: "))
    {
        case "1": solarSystem.ShowAllObjects(); break;
        case "2": FindObject(solarSystem); break;
        case "3": ShowPlanets(solarSystem); break;
        case "4": ShowSatellites(solarSystem); break;
        case "5": ShowDistance(solarSystem, calculator); break;
        case "6": ShowOrbitInfo(solarSystem); break;
        case "7": ShowAllOrbits(solarSystem); break;
        case "8": ManageMission(mission); break;
        case "9": ShowCatalog(catalog); break;
        case "0": running = false; Console.WriteLine("Программа завершена."); continue;
        default: Console.WriteLine("Ошибка: такого пункта меню нет."); break;
    }

    Pause();
}

// ================================
// КОНСОЛЬНЫЙ ИНТЕРФЕЙС
// ================================

static void ShowMenu()
{
    Console.Clear();
    Console.WriteLine("========================================");
    Console.WriteLine("          СОЛНЕЧНАЯ СИСТЕМА");
    Console.WriteLine("========================================\n");
    Console.WriteLine("1. Показать все объекты");
    Console.WriteLine("2. Найти космический объект");
    Console.WriteLine("3. Показать планеты");
    Console.WriteLine("4. Показать спутники");
    Console.WriteLine("5. Рассчитать расстояние между планетами");
    Console.WriteLine("6. Показать орбитальную информацию объекта");
    Console.WriteLine("7. Показать орбиты всех объектов");
    Console.WriteLine("8. Управление космической миссией");
    Console.WriteLine("9. Астрономический каталог");
    Console.WriteLine("0. Выход\n");
}

static void FindObject(StarSystem system)
{
    CelestialBody? body = ReadBody(system, "Введите название объекта: ");
    body?.ShowInfo();
}

static void ShowPlanets(StarSystem system)
{
    List<Planet> planets = system.GetPlanets();

    Console.WriteLine("=== ПЛАНЕТЫ ===\n");
    foreach (Planet planet in planets)
        Console.WriteLine($"- {planet.Name}");

    Console.WriteLine($"\nВсего планет: {planets.Count}");
}

static void ShowSatellites(StarSystem system)
{
    List<Satellite> satellites = system.GetSatellites();

    Console.WriteLine("=== СПУТНИКИ ===\n");
    foreach (Satellite satellite in satellites)
        Console.WriteLine($"- {satellite.Name} (спутник {satellite.ParentObject})");

    Console.WriteLine($"\nВсего спутников: {satellites.Count}");
}

static void ShowDistance(StarSystem system, DistanceCalculator calculator)
{
    CelestialBody? first = ReadBody(system, "Введите первую планету: ");
    if (first == null) return;

    CelestialBody? second = ReadBody(system, "Введите вторую планету: ");
    if (second == null) return;

    try
    {
        calculator.ShowDistance(first, second);
    }
    catch (ArgumentException exception)
    {
        Console.WriteLine($"Ошибка: {exception.Message}");
    }
}

static void ShowOrbitInfo(StarSystem system)
{
    CelestialBody? body = ReadBody(system, "Введите название объекта: ");
    if (body == null) return;

    if (body is IOrbitalObject orbital)
        orbital.ShowOrbitInfo();
    else
        Console.WriteLine($"У объекта \"{body.Name}\" нет орбитальной информации.");
}

static void ShowAllOrbits(StarSystem system)
{
    List<Orbit> orbits = system.GetOrbits();

    Console.WriteLine("=== ОРБИТЫ ОБЪЕКТОВ ===\n");
    foreach (Orbit orbit in orbits)
    {
        orbit.ShowInfo();
        Console.WriteLine();
    }

    Console.WriteLine($"Всего орбит: {orbits.Count}");
}

static void ShowCatalog(AstronomicalCatalog catalog)
{
    catalog.ShowCatalog();
    Console.WriteLine($"\nВсего объектов в каталоге: {catalog.Count}");
}

static void ManageMission(SpaceMission mission)
{
    bool inMenu = true;

    while (inMenu)
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("        УПРАВЛЕНИЕ МИССИЕЙ");
        Console.WriteLine("========================================\n");
        Console.WriteLine($"Миссия: {mission.Name}");
        Console.WriteLine($"Отправление: {mission.StartPoint.Name}");
        Console.WriteLine($"Назначение: {mission.Destination.Name}");
        Console.WriteLine($"Статус: {mission.Status}\n");
        Console.WriteLine("1. Запустить миссию");
        Console.WriteLine("2. Завершить миссию");
        Console.WriteLine("3. Показать информацию");
        Console.WriteLine("0. Назад\n");

        switch (ReadInput("Выберите действие: "))
        {
            case "1": mission.Start(); Console.WriteLine("Миссия запущена."); break;
            case "2": mission.Complete(); Console.WriteLine("Миссия завершена."); break;
            case "3": mission.ShowInfo(); break;
            case "0": inMenu = false; continue;
            default: Console.WriteLine("Ошибка: такого пункта меню нет."); break;
        }

        Pause();
    }
}

// ================================
// ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
// ================================

/// <summary>Читает строку от пользователя (без завершающих пробелов).</summary>
static string ReadInput(string prompt)
{
    Console.Write(prompt);
    return (Console.ReadLine() ?? string.Empty).Trim();
}

/// <summary>
/// Просит ввести название, находит объект в системе.
/// Если ввод пустой или объект не найден — печатает ошибку и возвращает null.
/// </summary>
static CelestialBody? ReadBody(StarSystem system, string prompt)
{
    string name = ReadInput(prompt);

    if (name.Length == 0)
    {
        Console.WriteLine("Название не может быть пустым.");
        return null;
    }

    CelestialBody? body = system.FindObject(name);

    if (body == null)
        Console.WriteLine($"Объект \"{name}\" не найден.");

    return body;
}

/// <summary>Пауза между пунктами меню.</summary>
static void Pause()
{
    Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
    Console.ReadLine();
}
