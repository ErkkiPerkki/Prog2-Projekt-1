using Godot;
using System;
using System.Collections.Generic;

namespace ElementSandbox
{
    public enum ElementID {
        SAND,
        CONCRETE
    }

    public partial class Grid: Node2D
    {
        public static TileMap TileMap;

        public static Vector2I TileSize = new(8, 8);
        public static Vector2I ScreenSize = new (
            (int)ProjectSettings.GetSetting("display/window/size/viewport_width"),
            (int)ProjectSettings.GetSetting("display/window/size/viewport_height")
        );
        public static Vector2I GridSize = ScreenSize / TileSize;

        public static Element?[,] grid = new Element[GridSize.X, GridSize.Y];
        public static ElementID selectedElement = ElementID.CONCRETE;

        enum TileAction {
            Place,
            Remove
        }

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

        static Dictionary<ElementID, Func<Element>> Elements = new()
        {
            {ElementID.SAND, () => new Sand()},
            {ElementID.CONCRETE, () => new Concrete()}
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

        public static bool IsOnGrid(Vector2I gridPosition)
        {
            bool withinXBounds = gridPosition.X >= 0 && gridPosition.X < GridSize.X;
            bool withinYBounds = gridPosition.Y >= 0 && gridPosition.Y < GridSize.Y;
            return withinXBounds && withinYBounds;
        }

        void HandleTileAction(TileAction action)
        {
            if (TileMap == null) return;

            Vector2I mousePos = (Vector2I)GetGlobalMousePosition();
            Vector2I gridPos = mousePos / TileSize;
            if (!IsOnGrid(gridPos)) return;

            int selectedElementID = (int)selectedElement;
            bool tileExists = TileMap.TileSet.HasSource(selectedElementID);
            if (!tileExists) return;

            if (action == TileAction.Place) {
                Element newElement = Elements[selectedElement].Invoke();
                newElement.GridPosition = gridPos;
                grid[gridPos.X, gridPos.Y] = newElement;
            }
            else if (action == TileAction.Remove) {
                grid[gridPos.X, gridPos.Y] = null;
            }

            DrawGrid();
        }

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
            Element?[,] nextGrid = new Element[GridSize.X, GridSize.Y];

            for (int x = 0; x < GridSize.X; x++) {
                for (int y = 0; y < GridSize.Y; y++) {
                    Element? element = grid[x, y];
                    if (element == null) continue;

                    Vector2I nextMove = element.Evaluate();
                    Vector2I nextPosition = element.GridPosition + nextMove;

                    bool spotOccupied = nextGrid[nextPosition.X, nextPosition.Y] != null;
                    if (spotOccupied) {
                        nextGrid[element.GridPosition.X, element.GridPosition.Y] = element;
                        continue;
                    }

                    element.GridPosition += nextMove;
                    nextGrid[nextPosition.X, nextPosition.Y] = element;
                }
            }

            grid = nextGrid;
            DrawGrid();
        }
    }
}
