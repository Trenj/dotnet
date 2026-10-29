namespace SpaceOOP2.Collections;

/// <summary>
/// Ковариантный обобщённый интерфейс (out T).
/// T используется только в позиции результата, поэтому ISpaceReader&lt;Planet&gt;
/// можно использовать как ISpaceReader&lt;CelestialBody&gt;.
/// </summary>
public interface ISpaceReader<out T>
{
    int Count { get; }

    T this[int index] { get; }

    IEnumerable<T> Items { get; }
}
