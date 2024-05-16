using Godot;
using System.Collections.Generic;

namespace ElementSandbox;

// Each value in ElementID corresponds to a tilset id which is used
// to display the corresponding texture for each element
public enum ElementID {
    SAND,
    CONCRETE,
    WATER,
    COFFEEPOWDER
}


// The Element class is the base class for all elements.
// It stores common data for all elements like ElementID and GridPosition
public abstract class Element
{
    private ElementID _ID;
    private int _AtlasID;

    public int AtlasID {
        get {return _AtlasID;}
        set {_AtlasID = value;}
    }

    public ElementID ID { get { return _ID; } set { _ID = value; } }
    public Vector2I GridPosition;

    public Element(ElementID ID, int atlasID)
    {
        _ID = ID;
        _AtlasID = atlasID;
    }

    /// <summary>
    /// Returns a dictionary containing a Vector2I direction as key and a bool as value indicating if a
    /// neighbor exists in that direction
    /// </summary>
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


