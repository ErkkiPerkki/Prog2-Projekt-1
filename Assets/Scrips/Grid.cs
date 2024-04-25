using Godot;
using System;
using System.Collections.Generic;

namespace ElementSandbox
{
    public enum ElementID {
        SAND
    }

    public partial class Grid: Node2D
    {
        private static TileMap TileMap;

        private static Vector2I TileSize = new(8, 8);
        private static Vector2I ScreenSize = new (
            (int)ProjectSettings.GetSetting("display/window/size/viewport_width"),
            (int)ProjectSettings.GetSetting("display/window/size/viewport_height")
        );
        private static Vector2I GridSize = ScreenSize / TileSize;

        private static Element?[,] grid = new Element[GridSize.X, GridSize.Y];
        private static ElementID selectedElement = ElementID.SAND;

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
            {ElementID.SAND, () => new Sand()}
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
            bool withinXBounds = gridPosition.X >= 0 && gridPosition.X <= GridSize.X;
            bool withinYBounds = gridPosition.Y >= 0 && gridPosition.Y <= GridSize.Y;
            return withinXBounds && withinYBounds;
        }

        void HandleTileAction(TileAction action)
        {
            if (TileMap == null) return;

            Vector2I mousePos = (Vector2I)GetGlobalMousePosition();
            Vector2I gridPos = mousePos / TileSize;
            if (gridPos.X < 0 || gridPos.X > GridSize.X) return;
            if (gridPos.Y < 0 || gridPos.Y > GridSize.Y) return;

            int selectedElementID = (int)selectedElement;
            bool tileExists = TileMap.TileSet.HasSource(selectedElementID);
            if (!tileExists) return;

            if (action == TileAction.Place) {
                //TileMap.SetCell(0, gridPos, selectedElementID, new(0, 0));
                Element newElement = Elements[selectedElement].Invoke();
                grid[gridPos.X, gridPos.Y] = newElement;
            }
            else if (action == TileAction.Remove) {
                //TileMap.SetCell(0, gridPos, -1, new(0, 0));
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
                    TileMap.SetCell(0, pos, nextID, new (0,0));
                }
            }
        }

        public static void _OnGridUpdate()
        {
            Element?[,] nextGrid = new Element[GridSize.X, GridSize.Y];

            for (int x = 0; x < GridSize.X; x++) {
                for (int y = 0; y < GridSize.Y; y++) {
                    if (grid[x, y] == null) continue;

                    foreach (KeyValuePair<string, Vector2I> pair in Directions) {
                        Vector2I pos = new Vector2I(x, y) + pair.Value;
                        nextGrid[pos.X, pos.Y] = Elements[ElementID.SAND].Invoke();
                    }
                    
                }
            }

            grid = nextGrid;
            DrawGrid();
        }
    }
}
