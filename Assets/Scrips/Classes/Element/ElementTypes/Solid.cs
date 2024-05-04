using Godot;

namespace ElementSandbox;

public class Solid: Element
{
    public Solid(ElementID elementID, int atlasID = 0): base(elementID, atlasID)
    {

    }

    public override Vector2I Evaluate()
    {
        return Vector2I.Zero;
    }

    public override void OnContact()
    {
        
    }
}
