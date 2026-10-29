# SpaceOOP — моделирование Солнечной системы

Консольное приложение на C# (.NET 8) — учебный проект по ООП: моделирование
звёздной системы и работа с космическими телами через консольное меню.

## Возможности

- Просмотр всех объектов системы (звёзды, планеты, спутники)
- Поиск объекта по названию
- Списки планет и спутников
- Расчёт расстояния между планетами
- Орбитальная информация: по объекту или по всем объектам сразу
- Управление космической миссией (запуск / завершение / статус)
- Астрономический каталог с поиском по названию

## Запуск

Требуется [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
dotnet run --project lab1/SpaceOOP
```

## Структура проекта

```
lab1/
└── SpaceOOP/                  # Проект
    ├── Models/                # Иерархия космических тел
    │   ├── CelestialBody.cs   # Абстрактный базовый класс
    │   ├── Star.cs            # Звезда
    │   ├── Planet.cs          # Планета
    │   ├── Satellite.cs       # Спутник
    │   └── IOrbitalObject.cs  # Интерфейс орбитальных объектов
    ├── Systems/               # Логика предметной области
    │   ├── StarSystem.cs      # Звёздная система (список тел, поиск, орбиты)
    │   ├── Orbit.cs           # Орбитальные характеристики
    │   └── SpaceMission.cs    # Космическая миссия
    ├── Services/              # Сервисные классы
    │   ├── AstronomicalCatalog.cs # Каталог объектов
    │   └── DistanceCalculator.cs  # Расчёт расстояний
    ├── Program.cs             # Консольное меню и точка входа
    └── description.txt        # Описание предметной области
```

## Архитектура и ООП

### Иерархия наследования

```
CelestialBody (abstract)
├── Star
├── Planet
└── Satellite
```

`CelestialBody` хранит общие характеристики (название, масса, радиус) и
объявляет абстрактный метод `ShowInfo()`, который каждый наследник
реализует по-своему.

### Инкапсуляция

- Поле `mass` закрыто (`private`), доступ — через свойство `Mass`,
  которое проверяет корректность (`value <= 0` → `ArgumentException`).
- У большинства свойств только `get` — состояние меняется через конструктор.
- `SpaceMission.Status` имеет `private set`: статус меняется только
  методами `Start()` и `Complete()`.

### Полиморфизм

Все тела хранятся в `List<CelestialBody>`; при вызове `ShowInfo()`
выполняется реализация конкретного класса (`Star`, `Planet` или `Satellite`).
Метод `StarSystem.ShowAllObjects()` работает с базовым типом и не знает
о конкретных классах.

### Интерфейс

`IOrbitalObject` (`OrbitalPeriod`, `OrbitRadius`, `ShowOrbitInfo()`)
реализуют `Planet` и `Satellite`. Метод `StarSystem.GetOrbits()` строит
орбиты всех объектов системы через этот интерфейс.

### Взаимодействие классов

- `StarSystem` объединяет тела, ищет и фильтрует их (`GetPlanets`,
  `GetSatellites`, `GetOrbits`)
- `AstronomicalCatalog` ведёт отдельный каталог с поиском по имени
- `DistanceCalculator` считает расстояние между планетами
- `Orbit` описывает орбитальные характеристики объекта
- `SpaceMission` моделирует миссию между двумя объектами

## Лабораторная 2 — обобщения (generics)

Вторая лабораторная развивает ту же предметную область: добавлена
собственная обобщённая коллекция и продемонстрирована вариативность
обобщённых интерфейсов.

```bash
dotnet run --project lab2/SpaceOOP2
```

### Что реализовано

- `SpaceCollection<T>` — собственная коллекция на массиве, реализует
  `ICollection<T>`, `IReadOnlyList<T>`, `IEnumerable<T>`, `IEnumerator<T>`
  и `ICloneable`
- ограничение обобщения: `where T : CelestialBody`
- ковариантность: `ISpaceReader<out T>`, а также стандартный
  `IEnumerable<out T>`
- контравариантность: `ISpaceVisitor<in T>`, стандартные
  `IComparer<in T>` и `Action<in T>`
- `StarSystem` хранит тела в `SpaceCollection<CelestialBody>` и использует
  обобщённый метод `GetObjectsOfType<T>() where T : CelestialBody`
- пункт меню `10` демонстрирует коллекцию, клонирование, ковариантность,
  контравариантность и сортировку через `IComparer<in T>`

```
lab2/
└── SpaceOOP2/
    ├── Collections/
    │   ├── SpaceCollection.cs        # Обобщённая коллекция
    │   ├── ISpaceReader.cs           # Ковариантный интерфейс (out T)
    │   ├── ISpaceVisitor.cs          # Контравариантный интерфейс (in T)
    │   ├── SpaceInfoPrinter.cs       # Реализация ISpaceVisitor
    │   └── CelestialBodyComparer.cs  # IComparer<in T>
    ├── Models/                       # Иерархия космических тел
    ├── Systems/                      # Звёздная система и орбиты
    ├── Services/                     # Каталог и расчёт расстояний
    ├── Program.cs                    # Консольное меню и демонстрации
    └── description.txt               # Описание проекта и обобщений
```
