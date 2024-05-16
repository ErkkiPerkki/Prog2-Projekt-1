namespace ElementSandbox;

// The concrete element is a fundemental building block for ElementSandbox since it is the only solid
// It can be used to build static structures
public partial class Concrete: Solid
{
    public Concrete(): base(ElementID.CONCRETE, Main.random.Next(0, 3))
    {

    }
}
