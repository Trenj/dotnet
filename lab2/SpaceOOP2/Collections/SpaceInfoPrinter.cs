using SpaceOOP2.Models;

namespace SpaceOOP2.Collections;

/// <summary>
/// Контравариантный «посетитель»: умеет обрабатывать любое CelestialBody
/// (выводит его информацию). Благодаря in T этот же объект можно
/// использовать и как ISpaceVisitor&lt;Planet&gt;, и как ISpaceVisitor&lt;Satellite&gt;.
/// </summary>
public class SpaceInfoPrinter : ISpaceVisitor<CelestialBody>
{
    // Полиморфизм: ShowInfo() вызовет реализацию конкретного класса
    // (Star, Planet или Satellite).
    public void Visit(CelestialBody item) => item.ShowInfo();
}
