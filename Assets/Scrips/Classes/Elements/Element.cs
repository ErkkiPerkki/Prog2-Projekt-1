using Godot;

namespace ElementSandbox
{
    public abstract class Element
    {
        private ElementID _ID;

        public ElementID ID { get{return _ID;} }
        public Vector2I GridPosition = new Vector2I(0, 0);

        public Element(ElementID ID)
        {
            _ID = ID;
            //GD.Print($"New Element: [{ID}]");
        }

        public abstract void Evaluate();
    }
}


