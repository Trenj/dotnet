using System.Collections;
using SpaceOOP2.Models;

namespace SpaceOOP2.Collections;

/// <summary>
/// Собственная обобщённая коллекция космических тел.
/// Ограничение: where T : CelestialBody — хранить можно только тела предметной области.
/// Поддерживает ICollection&lt;T&gt;, IReadOnlyList&lt;T&gt;, IEnumerable&lt;T&gt;,
/// IEnumerator&lt;T&gt; и ICloneable.
/// </summary>
public class SpaceCollection<T> : ICollection<T>, IReadOnlyList<T>, ICloneable, ISpaceReader<T>
    where T : CelestialBody
{
    private T[] items;
    private int count;

    public SpaceCollection()
    {
        items = Array.Empty<T>();
    }

    public SpaceCollection(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        items = capacity == 0 ? Array.Empty<T>() : new T[capacity];
    }

    public SpaceCollection(IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        items = source.ToArray();
        count = items.Length;
    }

    public int Count => count;

    public bool IsReadOnly => false;

    public IEnumerable<T> Items => this;

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

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        EnsureCapacity(count + 1);
        items[count] = item;
        count++;
    }

    public void Clear()
    {
        Array.Clear(items, 0, count);
        count = 0;
    }

    public bool Contains(T item) => IndexOf(item) >= 0;

    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);

        if (arrayIndex < 0 || arrayIndex > array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));

        if (array.Length - arrayIndex < count)
            throw new ArgumentException("Недостаточно места в целевом массиве.");

        Array.Copy(items, 0, array, arrayIndex, count);
    }

    public bool Remove(T item)
    {
        int index = IndexOf(item);
        if (index < 0)
            return false;

        RemoveAt(index);
        return true;
    }

    public void RemoveAt(int index)
    {
        ValidateIndex(index);

        int moved = count - index - 1;
        if (moved > 0)
            Array.Copy(items, index + 1, items, index, moved);

        count--;
        items[count] = default!;
    }

    public int IndexOf(T item) => Array.IndexOf(items, item, 0, count);

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < count; i++)
            yield return items[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public object Clone()
    {
        SpaceCollection<T> clone = new(count);
        Array.Copy(items, clone.items, count);
        clone.count = count;
        return clone;
    }

    private void EnsureCapacity(int minimum)
    {
        if (items.Length >= minimum)
            return;

        int newCapacity = items.Length == 0 ? 4 : items.Length * 2;
        if (newCapacity < minimum)
            newCapacity = minimum;

        Array.Resize(ref items, newCapacity);
    }

    private void ValidateIndex(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index));
    }
}
