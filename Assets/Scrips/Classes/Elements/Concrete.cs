using ElementSandbox;
using Godot;

public partial class Concrete: Element
{
    public Concrete(): base(ElementID.CONCRETE, Main.random.Next(0, 3))
    {

    }

    public override Vector2I Evaluate()
    {
        return Vector2I.Zero;
    }
}
