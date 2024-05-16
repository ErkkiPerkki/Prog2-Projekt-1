using Godot;
using System.Collections.Generic;

namespace ElementSandbox;

// The Solid class inherits from Element and handles the default logic for all solid elements
public class Solid: Element
{
    public Solid(ElementID elementID, int atlasID = 0): base(elementID, atlasID)
    {

    }

    // Solid logic
    public override Vector2I Evaluate()
    {
        return Vector2I.Zero;
    }

    public override void OnContact(List<Element> contactingNeighbors)
    {
        
    }
}
