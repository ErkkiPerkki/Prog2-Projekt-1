using Godot;
using System;
using System.Collections.Generic;

namespace ElementSandbox;

public partial class Grid: Node2D
{
    public static TileMap TileMap;

    public static Vector2I TileSize = new(8, 8); // The grid cell size in pixels
    public static Vector2I ScreenSize = new (
        (int)ProjectSettings.GetSetting("display/window/size/viewport_width"),
        (int)ProjectSettings.GetSetting("display/window/size/viewport_height")
    );
    public static Vector2I GridSize = ScreenSize / TileSize; // The grid size in grid cells

    public static Element?[,] grid = new Element[GridSize.X, GridSize.Y]; // Main array holding the data for each grid cell
    public static ElementID selectedElement = ElementID.SAND; // The element which the player is placing

    enum TileAction {
        Place,
        Remove
    }

    // Stores all neighbor directions
    public static Dictionary<string, Vector2I> Directions = new() {
        {"TOP_LEFT", -Vector2I.One},
        {"UP", Vector2I.Up},
        {"TOP_RIGHT", new(1, -1)},
        {"LEFT", Vector2I.Left},
        {"RIGHT", Vector2I.Right},
        {"BOTTOM_LEFT", new(-1, 1)},
        {"DOWN", Vector2I.Down},
        {"BOTTOM_RIGHT", Vector2I.One}
    };

    // This dictionary holds a function which when invoked, returns a new element of given ElementID
    static Dictionary<ElementID, Func<Element>> Elements = new()
    {
        {ElementID.SAND, () => new Sand()},
        {ElementID.CONCRETE, () => new Concrete()},
        {ElementID.WATER, () => new Water()},
        {ElementID.COFFEEPOWDER, () => new CoffeePowder()}
    };

    public override async void _Ready()
    {
        TileMap = GetNode<TileMap>("TileMap");
    }

    public override void _Input(InputEvent Event)
    {
        if (Input.IsActionPressed("PlaceTile"))
            HandleTileAction(TileAction.Place);
        if (Input.IsActionPressed("RemoveTile"))
            HandleTileAction(TileAction.Remove);
    }

    /// <summary>
    /// Returns a boolean indicating whether the given grid position in on the grid or not
    /// </summary>
    /// <param name="IsOnGrid"></param>
    public static bool IsOnGrid(Vector2I gridPosition)
    {
        bool withinXBounds = gridPosition.X >= 0 && gridPosition.X < GridSize.X;
        bool withinYBounds = gridPosition.Y >= 0 && gridPosition.Y < GridSize.Y;
        return withinXBounds && withinYBounds;
    }
    
    /// <summary>
    /// Handles tile placement and removal
    /// </summary>
    /// <param name="HandleTileAction"></param>
    void HandleTileAction(TileAction action)
    {
        if (TileMap == null) return;

        Vector2I mousePos = (Vector2I)GetGlobalMousePosition();
        Vector2I gridPos = mousePos / TileSize;
        if (!IsOnGrid(gridPos)) return;

        // Checks whether the selected element has a valid texture atlas in the tileset
        int selectedElementID = (int)selectedElement;
        bool tileExists = TileMap.TileSet.HasSource(selectedElementID);
        if (!tileExists) return;

        // If left click is pressed (TileAction.Place), it inserts a new
        // element of type selectedElement into the grid array
        // If right click is pressed (TileAction.Remove), it removes the
        // element at given position from the grid array
        if (action == TileAction.Place) {
            Element newElement = Elements[selectedElement]();
            newElement.GridPosition = gridPos;
            grid[gridPos.X, gridPos.Y] = newElement;
        }
        else if (action == TileAction.Remove) {
            grid[gridPos.X, gridPos.Y] = null;
        }
    }

    /// <summary>
    /// Renders the grid by looping through the grid array and setting
    /// each value to the corresponding element in the tilemap
    /// </summary>
    static void DrawGrid()
    {
        for (int x = 0; x < GridSize.X; x++) {
            for (int y = 0; y < GridSize.Y; y++) {
                Vector2I pos = new(x, y);
                Element? element = grid[x, y];

                int currentID = TileMap.GetCellSourceId(0, pos);
                int nextID = element == null ? -1: (int)element.ID; 
                if (currentID == nextID) continue;

                int atlasID = element == null ? 0 : element.AtlasID;
                TileMap.SetCell(0, pos, nextID, new (atlasID,0));
            }
        }
    }

    public override void _Process(double delta)
    {
        // nextGrid is an empty copy of the grid array which is used to store the updated values
        Element?[,] nextGrid = new Element[GridSize.X, GridSize.Y];

        for (int x = 0; x < GridSize.X; x++) {
            for (int y = 0; y < GridSize.Y; y++) {
                Element? element = grid[x, y];
                if (element == null) continue;

                // Evaluates the next position for each element using the logic stored in the Evaluate function
                Vector2I nextMove = element.Evaluate();
                Vector2I nextPosition = element.GridPosition + nextMove;

                // Handles the case where two cells might want to write to the same grid position
                bool spotOccupied = nextGrid[nextPosition.X, nextPosition.Y] == null;
                if (!spotOccupied) {
                    nextGrid[element.GridPosition.X, element.GridPosition.Y] = element;
                    continue;
                }

                element.GridPosition += nextMove;
                nextGrid[nextPosition.X, nextPosition.Y] = element;

                // Contact stuff
                // *Should* fill an array of elements which are laying next to the element in the current iteration
                // This array is later passed on to the OnContact function which each element type handles differently
                List<Element> contactingNeighbors = new List<Element>();

                foreach (KeyValuePair<Vector2I, bool> pair in element.GetNeighbors()) {
                    if (!pair.Value) continue;
               
                    Vector2I gridPos = new(pair.Key.X, pair.Key.Y);
                    gridPos += element.GridPosition;
                    if (!IsOnGrid(gridPos)) continue;

                    Element? neighbor = grid[gridPos.X, gridPos.Y];
                    if (neighbor == null) continue;

                    contactingNeighbors.Add(neighbor);
                }
                if (contactingNeighbors.Count == 0) continue;

                element.OnContact(contactingNeighbors);
            }
        }

        // Updates the grid with the new grid values
        grid = nextGrid;
        DrawGrid();
    }
}
