namespace SpaceOOP2.Collections;

/// <summary>
/// Контравариантный обобщённый интерфейс (in T).
/// T используется только в позиции параметра, поэтому посетитель,
/// умеющий обрабатывать CelestialBody, подходит и для Planet.
/// </summary>
public interface ISpaceVisitor<in T>
{
    void Visit(T item);
}
