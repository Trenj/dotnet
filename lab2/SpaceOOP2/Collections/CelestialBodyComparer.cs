using SpaceOOP2.Models;

namespace SpaceOOP2.Collections;

/// <summary>
/// Сравнение космических тел по массе (от лёгких к тяжёлым).
///
/// IComparer&lt;in T&gt; - контравариантный интерфейс из стандартной библиотеки .NET.
/// Поэтому этот сравниватель (IComparer&lt;CelestialBody&gt;) можно передать
/// в List&lt;Planet&gt;.Sort(), который ждёт IComparer&lt;Planet&gt;:
/// сравниватель умеет сравнивать любые тела, значит, и планеты в частности.
/// </summary>
public class CelestialBodyComparer : IComparer<CelestialBody>
{
    // Возвращает: < 0 если x легче, 0 если равны, > 0 если x тяжелее.
    public int Compare(CelestialBody? x, CelestialBody? y)
    {
        if (ReferenceEquals(x, y))
            return 0; // одна и та же ссылка (в т.ч. оба null)

        if (x is null)
            return -1; // null считается «меньше» любого объекта

        if (y is null)
            return 1;

        // CompareTo у double сравнивает числа - используем его для массы
        return x.Mass.CompareTo(y.Mass);
    }
}
