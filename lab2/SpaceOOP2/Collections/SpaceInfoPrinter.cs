using SpaceOOP2.Models;

namespace SpaceOOP2.Collections;

public class SpaceInfoPrinter : ISpaceVisitor<CelestialBody>
{
    public void Visit(CelestialBody item) => item.ShowInfo();
}
