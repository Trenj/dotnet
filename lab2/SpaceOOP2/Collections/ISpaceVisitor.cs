namespace SpaceOOP2.Collections;

/// <summary>
/// Контравариантный обобщённый интерфейс (КЛЮЧЕВОЕ СЛОВО in).
///
/// in означает: тип T используется ТОЛЬКО как «входной» —
/// принимается параметром в методы, но никогда не возвращается.
///
/// Благодаря этому разрешено присваивание «от общего к частному»:
///     ISpaceVisitor&lt;CelestialBody&gt; visitor = new SpaceInfoPrinter();
///     ISpaceVisitor&lt;Planet&gt; planetVisitor = visitor;  // OK!
///
/// Логика: если объект умеет обрабатывать ЛЮБОЕ космическое тело,
/// то он тем более умеет обработать планету.
/// </summary>
public interface ISpaceVisitor<in T>
{
    void Visit(T item); // T принимается параметром — позиция аргумента, in разрешён
}
