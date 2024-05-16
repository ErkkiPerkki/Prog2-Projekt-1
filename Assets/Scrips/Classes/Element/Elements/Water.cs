using System.Collections.Generic;

namespace ElementSandbox;

// The Water element is a basic liquid with no special features
public class Water: Liquid
{
    public Water(): base(ElementID.WATER, Main.random.Next(0, 3))
    {

    }
}
