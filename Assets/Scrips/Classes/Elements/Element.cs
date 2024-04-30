using Godot;
using System.Collections.Generic;

namespace ElementSandbox
{
    public enum ElementID {
        SAND,
        CONCRETE,
        WATER
    }

    public abstract class Element
    {
        private ElementID _ID;
        private int _AtlasID;

        public int AtlasID { get{return _AtlasID;} }
        public ElementID ID { get{return _ID;} }
        public Vector2I GridPosition;

        public Element(ElementID ID, int atlasID = 0)
        {
            _ID = ID;
            _AtlasID = atlasID;
        }

        public Dictionary<Vector2I, bool> GetNeighbors()
        {
            Dictionary<Vector2I, bool> neighbors = new();

            foreach (KeyValuePair<string, Vector2I> pair in Grid.Directions) {
                Vector2I pos = GridPosition + pair.Value;
                bool isOnGrid = Grid.IsOnGrid(pos);
                bool neighborExists = isOnGrid ? Grid.grid[pos.X, pos.Y] != null : true;

                neighbors.Add(pair.Value, neighborExists);
            }

            return neighbors;
        }

        public abstract Vector2I Evaluate();
    }
}


