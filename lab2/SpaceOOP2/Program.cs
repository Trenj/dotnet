using SpaceOOP2.Collections;
using SpaceOOP2.Models;
using SpaceOOP2.Services;
using SpaceOOP2.Systems;

// ================================
// ДАННЫЕ: СОЛНЕЧНАЯ СИСТЕМА
// ================================
// Это код верхнего уровня (top-level statements): точка входа программы.
// Здесь создаются объекты предметной области, с которыми работает меню.

StarSystem solarSystem = new("Солнечная система");

// Порядок добавления: звезда, планеты, спутник
solarSystem.AddObject(new Star("Солнце", 1.989e30, 696340, 5778, "G2V"));
solarSystem.AddObject(new Planet("Меркурий", 3.301e23, 2439.7, 57.9, 87.97, false));
solarSystem.AddObject(new Planet("Земля", 5.972e24, 6371, 149.6, 365.25, true));
solarSystem.AddObject(new Planet("Марс", 6.417e23, 3389.5, 227.9, 687, true));
solarSystem.AddObject(new Satellite("Луна", 7.342e22, 1737.4, "Земля", 0.384, 27.3));

// Каталог заполняется теми же телами, что есть в системе.
// solarSystem.Objects: доступ только для чтения, изменить систему
// через это свойство нельзя.
AstronomicalCatalog catalog = new();
foreach (CelestialBody body in solarSystem.Objects)
    catalog.Add(body);

DistanceCalculator calculator = new();
// Заранее созданная миссия Земля -> Марс (пункт меню 8)
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
        case "10": ManageGenerics(solarSystem); break; // демонстрация обобщений
        case "0": running = false; Console.WriteLine("Программа завершена."); continue;
        default: Console.WriteLine("Ошибка: такого пункта меню нет."); break;
    }

    Pause();
}

// ================================
// КОНСОЛЬНЫЙ ИНТЕРФЕЙС
// ================================
// Ниже: локальные статические функции (объявлены прямо в файле
// с кодом верхнего уровня). Каждая обслуживает один пункт меню.

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
    Console.WriteLine("10. Обобщённая коллекция и вариативность");
    Console.WriteLine("0. Выход\n");
}

static void FindObject(StarSystem system)
{
    CelestialBody? body = ReadBody(system, "Введите название объекта: ");
    // ?. : вызов ShowInfo() только если body не null
    body?.ShowInfo();
}

static void ShowPlanets(StarSystem system)
{
    // Обобщённый метод: из всех тел системы отбираются только планеты
    SpaceCollection<Planet> planets = system.GetPlanets();

    Console.WriteLine("=== ПЛАНЕТЫ ===\n");
    foreach (Planet planet in planets)
        Console.WriteLine($"- {planet.Name}");

    Console.WriteLine($"\nВсего планет: {planets.Count}");
}

static void ShowSatellites(StarSystem system)
{
    SpaceCollection<Satellite> satellites = system.GetSatellites();

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
        // Сюда попадём, если одно из тел: не планета (см. DistanceCalculator)
        Console.WriteLine($"Ошибка: {exception.Message}");
    }
}

static void ShowOrbitInfo(StarSystem system)
{
    CelestialBody? body = ReadBody(system, "Введите название объекта: ");
    if (body == null) return;

    // Орбита есть только у объектов, реализующих IOrbitalObject
    // (у планет и спутников; у звезды: нет).
    if (body is IOrbitalObject orbital)
        orbital.ShowOrbitInfo();
    else
        Console.WriteLine($"У объекта \"{body.Name}\" нет орбитальной информации.");
}

static void ShowAllOrbits(StarSystem system)
{
    // GetOrbits возвращает List<Orbit>: Orbit не является CelestialBody,
    // поэтому в SpaceCollection его положить нельзя (см. комментарий в StarSystem).
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
// ДЕМОНСТРАЦИЯ ОБОБЩЕНИЙ (предмет 2-й лабораторной)
// ================================

static void ManageGenerics(StarSystem system)
{
    // Работаем с КЛОНОМ коллекции тел, чтобы демонстрации
    // (например, удаление при клонировании) не портили саму систему.
    SpaceCollection<CelestialBody> collection = system.CloneBodies();
    bool inMenu = true;

    while (inMenu)
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("   ОБОБЩЁННАЯ КОЛЛЕКЦИЯ И ВАРИАТИВНОСТЬ");
        Console.WriteLine("========================================\n");
        Console.WriteLine("1. Показать обобщённую коллекцию");
        Console.WriteLine("2. Клонирование коллекции (ICloneable)");
        Console.WriteLine("3. Ковариантность (out T)");
        Console.WriteLine("4. Контравариантность (in T)");
        Console.WriteLine("5. Контравариантность IComparer<in T>");
        Console.WriteLine("0. Назад\n");

        switch (ReadInput("Выберите действие: "))
        {
            case "1": ShowGenericCollection(collection); break;
            case "2": ShowCloneDemo(collection); break;
            case "3": ShowCovarianceDemo(system); break;
            case "4": ShowContravarianceDemo(system); break;
            case "5": ShowComparerDemo(system); break;
            case "0": inMenu = false; continue;
            default: Console.WriteLine("Ошибка: такого пункта меню нет."); break;
        }

        Pause();
    }
}

// Показывает, что наша коллекция работает как обычная: Count, foreach,
// индексатор, CopyTo. Ограничение where T : CelestialBody проверяется
// компилятором: SpaceCollection<string> просто не соберётся.
static void ShowGenericCollection(SpaceCollection<CelestialBody> collection)
{
    Console.WriteLine("=== SpaceCollection<CelestialBody> ===\n");
    Console.WriteLine($"Тип коллекции: {collection.GetType().Name}");
    Console.WriteLine($"Количество элементов: {collection.Count}\n");

    // foreach возможен благодаря реализации IEnumerable<T>
    foreach (CelestialBody body in collection)
        Console.WriteLine($"- {body.GetType().Name}: {body.Name}");

    // CopyTo: метод из ICollection<T>
    CelestialBody[] copy = new CelestialBody[collection.Count];
    collection.CopyTo(copy, 0);

    Console.WriteLine($"\nICollection<T>.CopyTo: скопировано {copy.Length} элементов.");
    Console.WriteLine($"Ограничение where T : CelestialBody не позволяет хранить посторонние типы.");
}

// Демонстрация ICloneable: клон: независимая по составу копия коллекции.
static void ShowCloneDemo(SpaceCollection<CelestialBody> collection)
{
    // Ровно одно клонирование. Clone() возвращает object, нужно приведение.
    var clone = (SpaceCollection<CelestialBody>)collection.Clone();

    Console.WriteLine("=== ICloneable ===\n");
    Console.WriteLine($"Оригинал до удаления: {collection.Count} элементов.");
    Console.WriteLine($"Клон до удаления: {clone.Count} элементов.");

    // Удаляем первый элемент из ОРИГИНАЛА: в клоне он должен остаться
    if (collection.Count > 0)
        collection.RemoveAt(0);

    Console.WriteLine($"Оригинал после удаления: {collection.Count} элементов.");
    Console.WriteLine($"Клон после изменения оригинала: {clone.Count} элементов.");
    Console.WriteLine("\nКлон независим по составу: удаление из оригинала не изменило копию.");
    Console.WriteLine("Клон поверхностный: объекты-тела общие, независим только состав коллекции.");
}

// КОВАРИАНТНОСТЬ (out T): присваивание «от частного к общему».
static void ShowCovarianceDemo(StarSystem system)
{
    SpaceCollection<Planet> planets = system.GetPlanets();

    // 1) Наш ковариантный интерфейс ISpaceReader<out T>:
    ISpaceReader<Planet> planetReader = planets;
    // Planet: это CelestialBody, поэтому «читатель планет» можно
    // использовать как «читатель любых тел». Без out такая строка
    // не скомпилировалась бы.
    ISpaceReader<CelestialBody> bodyReader = planetReader;

    // 2) Стандартный ковариантный интерфейс IEnumerable<out T>:
    IEnumerable<Planet> planetSequence = planets;
    IEnumerable<CelestialBody> bodySequence = planetSequence;

    Console.WriteLine("=== КОВАРИАНТНОСТЬ (out T) ===\n");
    Console.WriteLine("ISpaceReader<Planet> -> ISpaceReader<CelestialBody>");
    Console.WriteLine($"Планет через ISpaceReader<Planet>: {planetReader.Count}");
    Console.WriteLine($"Тел через ISpaceReader<CelestialBody>: {bodyReader.Count}\n");

    Console.WriteLine("IEnumerable<Planet> -> IEnumerable<CelestialBody>");
    // Перебираем планет через переменную типа IEnumerable<CelestialBody>
    foreach (CelestialBody body in bodySequence)
        Console.WriteLine($"- {body.Name}");
}

// КОНТРАВАРИАНТНОСТЬ (in T): присваивание «от общего к частному».
static void ShowContravarianceDemo(StarSystem system)
{
    // 1) Наш контравариантный интерфейс ISpaceVisitor<in T>:
    ISpaceVisitor<CelestialBody> bodyVisitor = new SpaceInfoPrinter();
    // Посетитель умеет обрабатывать ЛЮБОЕ тело, значит, подойдёт и там,
    // где нужен обработчик именно планет. Без in: ошибка компиляции.
    ISpaceVisitor<Planet> planetVisitor = bodyVisitor;

    Console.WriteLine("=== КОНТРАВАРИАНТНОСТЬ (in T) ===\n");
    Console.WriteLine("ISpaceVisitor<CelestialBody> -> ISpaceVisitor<Planet>\n");

    SpaceCollection<Planet> planets = system.GetPlanets();
    if (planets.Count == 0)
    {
        Console.WriteLine("В системе нет планет.");
        return;
    }

    // Вызываем Visit через ссылку ISpaceVisitor<Planet>,
    // хотя объект создан как ISpaceVisitor<CelestialBody>
    planetVisitor.Visit(planets[0]);

    // 2) Контравариантность стандартного делегата Action<in T>:
    // делегат, принимающий CelestialBody, можно использовать как
    // делегат, принимающий Planet.
    Action<CelestialBody> bodyAction = body => Console.WriteLine($"Action<CelestialBody>: {body.Name}");
    Action<Planet> planetAction = bodyAction;

    Console.WriteLine();
    planetAction(planets[0]);
}

// Контравариантность стандартного IComparer<in T> на примере сортировки.
static void ShowComparerDemo(StarSystem system)
{
    SpaceCollection<Planet> planets = system.GetPlanets();
    List<Planet> sorted = planets.ToList(); // копия в List, т.к. Sort есть у List<T>

    // Сравниватель создан для CelestialBody...
    IComparer<CelestialBody> bodyComparer = new CelestialBodyComparer();
    // ...но благодаря in T подходит и для сортировки планет
    IComparer<Planet> planetComparer = bodyComparer;

    sorted.Sort(planetComparer); // сортировка по массе (см. CelestialBodyComparer)

    Console.WriteLine("=== IComparer<in T> ===\n");
    Console.WriteLine("IComparer<CelestialBody> -> IComparer<Planet>\n");
    Console.WriteLine("Планеты, отсортированные по массе:");

    foreach (Planet planet in sorted)
        Console.WriteLine($"- {planet.Name,-10} {planet.Mass:E2} кг");
}

// ================================
// ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
// ================================

// Читает строку из консоли и убирает пробелы по краям.
static string ReadInput(string prompt)
{
    Console.Write(prompt);
    return (Console.ReadLine() ?? string.Empty).Trim();
}

// Запрашивает название и ищет тело в системе.
// Возвращает null (с сообщением), если ввод пуст или объект не найден.
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

static void Pause()
{
    Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
    Console.ReadLine();
}
