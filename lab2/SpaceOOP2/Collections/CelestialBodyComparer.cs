using SpaceOOP2.Models;

namespace SpaceOOP2.Collections;

/// <summary>
/// Сравнение космических тел по массе.
/// IComparer&lt;in T&gt; — контравариантный интерфейс из .NET.
/// </summary>
public class CelestialBodyComparer : IComparer<CelestialBody>
{
    public int Compare(CelestialBody? x, CelestialBody? y)
    {
        if (ReferenceEquals(x, y))
            return 0;

        if (x is null)
            return -1;

        if (y is null)
            return 1;

        return x.Mass.CompareTo(y.Mass);
    }
}
