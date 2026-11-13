namespace SpaceOOP2.Collections;

/// <summary>
/// Ковариантный обобщённый интерфейс (КЛЮЧЕВОЕ СЛОВО out).
///
/// out означает: тип T используется ТОЛЬКО как «выходной» -
/// возвращается из свойств и методов, но никогда не принимается как параметр.
///
/// Благодаря этому разрешено присваивание «от частного к общему»:
///     ISpaceReader&lt;Planet&gt; reader = new SpaceCollection&lt;Planet&gt;();
///     ISpaceReader&lt;CelestialBody&gt; bodies = reader;  // OK: Planet - это CelestialBody
///
/// Логика: если из reader можно прочитать Planet, то через него
/// заведомо можно прочитать и CelestialBody (любая планета - космическое тело).
/// </summary>
public interface ISpaceReader<out T>
{
    int Count { get; }

    T this[int index] { get; } // T возвращается - позиция результата, out разрешён

    IEnumerable<T> Items { get; } // IEnumerable<T> сам ковариантен, поэтому тоже допустимо
}
