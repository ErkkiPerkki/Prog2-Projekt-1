using Godot;
using System;
using System.Collections.Generic;

namespace ElementSandbox
{
    public enum ElementID {
        NONE,
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

        static void DrawGrid()
        {
            for (int x = 0; x <= GridSize.X; x++) {
                for (int y = 0; y <= GridSize.Y; y++) {
                    Vector2I pos = new(x, y);
                    Element element = grid[x, y];
                    int currentID = TileMap.GetCellSourceId(0, pos);
                    int nextID = (int)element.ID;

                    if (currentID == nextID) continue;
                    TileMap.SetCell(0, pos, nextID);
                }
            }
        }

        void HandleTileAction(TileAction action)
        {
            if (TileMap == null) return;

            Vector2I mousePos = (Vector2I)GetGlobalMousePosition();
            Vector2I gridPos = mousePos / TileSize;

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
        }

        public static void _OnGridUpdate()
        {
            DrawGrid();
            //Element?[,] nextGrid = new Element[GridSize.X, GridSize.Y];

            //for (int x=0; x <= GridSize.X; x++) {
            //    for (int y=0; y <= GridSize.Y; y++) {
            //        nextGrid[x, y] = Elements[ElementID.SAND].Invoke();
            //    }
            //}

            //grid = nextGrid;
        }
    }
}
