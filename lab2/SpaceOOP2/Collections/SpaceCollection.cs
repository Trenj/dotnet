using System.Collections;
using SpaceOOP2.Models;

namespace SpaceOOP2.Collections;

/// <summary>
/// Собственная обобщённая коллекция космических тел (аналог List&lt;T&gt;,
/// написанный вручную на основе массива).
///
/// Что здесь демонстрируется:
///   1. Обобщённый класс: SpaceCollection&lt;T&gt; — один и тот же код работает
///      с любым типом T (Planet, Satellite, CelestialBody и т.д.).
///   2. Ограничение обобщения: where T : CelestialBody — коллекция может
///      хранить только космические тела, попытка написать SpaceCollection&lt;string&gt;
///      не скомпилируется.
///   3. Реализация стандартных интерфейсов .NET:
///        ICollection&lt;T&gt;    — Add / Remove / Clear / Contains / CopyTo / Count;
///        IReadOnlyList&lt;T&gt;  — Count + индексатор (доступ по номеру элемента);
///        IEnumerable&lt;T&gt;    — позволяет перебирать коллекцию в foreach;
///        ICloneable        — метод Clone() создаёт копию коллекции;
///        ISpaceReader&lt;T&gt;  — наш собственный ковариантный интерфейс.
/// </summary>
public class SpaceCollection<T> : ICollection<T>, IReadOnlyList<T>, ICloneable, ISpaceReader<T>
    where T : CelestialBody
{
    // Внутреннее хранилище — обычный массив элементов типа T.
    // Массив почти всегда больше, чем реальное число элементов (запас на рост).
    private T[] items;

    // Сколько элементов реально лежит в массиве (items.Length >= count).
    private int count;

    public SpaceCollection()
    {
        items = Array.Empty<T>(); // пустой массив без выделения лишней памяти
    }

    public SpaceCollection(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        items = capacity == 0 ? Array.Empty<T>() : new T[capacity];
    }

    // Конструктор из любой последовательности (массива, List<T> и т.п.)
    public SpaceCollection(IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        items = source.ToArray();
        count = items.Length;
    }

    // Количество элементов в коллекции (требование ICollection<T>).
    public int Count => count;

    // false — коллекцию можно изменять (требование ICollection<T>).
    public bool IsReadOnly => false;

    // Свойство из нашего интерфейса ISpaceReader<T>: отдаём коллекцию
    // как перечисление. this подходит, т.к. мы реализуем IEnumerable<T>.
    public IEnumerable<T> Items => this;

    // Индексатор — доступ к элементу по номеру: collection[0], collection[1]...
    // Геттер требуется IReadOnlyList<T> и ISpaceReader<T>, сеттер добавлен для удобства.
    public T this[int index]
    {
        get
        {
            ValidateIndex(index);
            return items[index];
        }
        set
        {
            ValidateIndex(index);
            items[index] = value;
        }
    }

    // Добавление элемента в конец коллекции.
    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        EnsureCapacity(count + 1); // при необходимости расширяем массив
        items[count] = item;
        count++;
    }

    // Полная очистка коллекции.
    public void Clear()
    {
        Array.Clear(items, 0, count); // затираем ссылки, чтобы сборщик мусора мог их освободить
        count = 0;
    }

    public bool Contains(T item) => IndexOf(item) >= 0;

    // Копирует все элементы в переданный массив, начиная с позиции arrayIndex
    // (требование ICollection<T>).
    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);

        if (arrayIndex < 0 || arrayIndex > array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));

        if (array.Length - arrayIndex < count)
            throw new ArgumentException("Недостаточно места в целевом массиве.");

        Array.Copy(items, 0, array, arrayIndex, count);
    }

    // Удаляет первое вхождение элемента. Возвращает true, если элемент найден.
    public bool Remove(T item)
    {
        int index = IndexOf(item);
        if (index < 0)
            return false;

        RemoveAt(index);
        return true;
    }

    // Удаление по индексу: все элементы правее сдвигаются на одну позицию влево.
    public void RemoveAt(int index)
    {
        ValidateIndex(index);

        int moved = count - index - 1; // сколько элементов нужно сдвинуть
        if (moved > 0)
            Array.Copy(items, index + 1, items, index, moved);

        count--;
        items[count] = default!; // очищаем освободившуюся ячейку
    }

    // Линейный поиск элемента (по ссылке/Equals) среди первых count элементов.
    public int IndexOf(T item) => Array.IndexOf(items, item, 0, count);

    // Этот метод вызывается, когда коллекцию перебирают в foreach.
    // yield return отдаёт элементы по одному, не создавая промежуточный массив.
    // Компилятор сам генерирует класс-энумератор, реализующий IEnumerator<T>.
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < count; i++)
            yield return items[i];
    }

    // Необобщённая версия GetEnumerator — требование старого интерфейса IEnumerable.
    // Просто перенаправляем на обобщённую версию.
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Создаёт копию коллекции (требование ICloneable).
    // Важно: это ПОВЕРХНОСТНОЕ копирование — копируется массив ссылок,
    // а сами объекты CelestialBody остаются общими для оригинала и копии.
    // Независимость копии в том, что Add/Remove в одной коллекции
    // не меняют другую.
    public object Clone()
    {
        SpaceCollection<T> clone = new(count);
        Array.Copy(items, clone.items, count);
        clone.count = count;
        return clone;
    }

    // Увеличивает массив, если в нём не хватает места под minimum элементов.
    // Стратегия как у List<T>: ёмкость удваивается (амортизированное O(1) на Add).
    private void EnsureCapacity(int minimum)
    {
        if (items.Length >= minimum)
            return;

        int newCapacity = items.Length == 0 ? 4 : items.Length * 2;
        if (newCapacity < minimum)
            newCapacity = minimum;

        Array.Resize(ref items, newCapacity);
    }

    // Проверка, что индекс указывает на существующий элемент (0..count-1).
    private void ValidateIndex(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index));
    }
}
