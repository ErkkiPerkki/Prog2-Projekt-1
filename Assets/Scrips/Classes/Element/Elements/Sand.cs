using Godot;
using System.Collections.Generic;

namespace ElementSandbox;

public class Sand: Powder
{
    public Sand(): base(ElementID.SAND, Main.random.Next(0, 4))
    {
        
    }

    public override void OnContact(List<Element> contactingNeighbors)
    {

    }
}

