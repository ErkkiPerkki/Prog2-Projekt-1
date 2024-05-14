using Godot;
using System.Collections.Generic;

namespace ElementSandbox;

public enum ElementID {
    SAND,
    CONCRETE,
    WATER,
    COFFEEPOWDER
}

public abstract class Element
{
    private ElementID _ID;
    private int _AtlasID;

    public int AtlasID {
        get {return _AtlasID;}
        set {
            // Add safety checks
            _AtlasID = value;
        }
    }

    public ElementID ID { get { return _ID; } set { _ID = value; } }
    public Vector2I GridPosition;

    public Element(ElementID ID, int atlasID)
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

    // Evaluates the next position of given cell
    public abstract Vector2I Evaluate();

    // Handles behavior where a certain tile would react
    // differently if in contact with another cell
    public abstract void OnContact(List<Element> contactingNeighbors); 
}


