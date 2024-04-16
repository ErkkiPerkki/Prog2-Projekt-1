using Godot;

namespace ElementSandbox
{
    public abstract partial class Element: Resource
    {
        private ElementID _ID = ElementID.NONE;

        public ElementID ID { get { return _ID; } }
        public Vector2I GridPosition = new Vector2I(0, 0);

        public Element(ElementID ID = ElementID.NONE)
        {
            _ID = ID;
            //GD.Print($"New Element: [{ID}]");
        }

        public abstract void Evaluate();
    }
}


