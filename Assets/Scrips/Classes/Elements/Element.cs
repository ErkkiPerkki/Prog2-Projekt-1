using Godot;
using System.Collections.Generic;

namespace ElementSandbox
{
    public abstract class Element
    {
        private ElementID _ID;

        public ElementID ID { get{return _ID;} }
        public Vector2I GridPosition;

        public Element(ElementID ID)
        {
            _ID = ID;
        }

        public Dictionary<Vector2I, bool> GetNeighbors()
        {
            Dictionary<Vector2I, bool> neighbors = new();

            foreach (KeyValuePair<string, Vector2I> pair in Grid.Directions) {
                Vector2I pos = GridPosition + pair.Value;
                bool isOnGrid = Grid.IsOnGrid(pos);
                bool neighborExists = isOnGrid ? Grid.grid[pos.X, pos.Y] == null : false;

                neighbors.Add(pos, neighborExists);
            }

            return neighbors;
        }

        public abstract Vector2I? Evaluate();
    }
}


