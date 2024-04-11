using Godot;

namespace ElementSandbox
{
    public enum ElementID {
        NONE,
        SAND
    }

    public partial class Grid: Node2D
    {
        static Vector2I TileSize = new(8, 8);
        static TileMap TileMap;
        static object ScreenSize = ProjectSettings.GetSetting("display");

        //static Vector2I[][] Grid = new Vector2I[][];

        static ElementID selectedElement = ElementID.NONE;

        enum TileAction {
            Place,
            Remove
        }

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

        void HandleTileAction(TileAction action)
        {
            if (TileMap == null) return;

            Vector2I mousePos = (Vector2I)GetGlobalMousePosition();
            Vector2I gridPos = mousePos / TileSize;

            int selectedElementID = (int)selectedElement;
            bool tileExists = TileMap.TileSet.HasSource(selectedElementID);
            if (!tileExists) return;

            if (action == TileAction.Place) {
                TileMap.SetCell(0, gridPos, selectedElementID, new(0, 0));
            }
            else if (action == TileAction.Remove) {
                TileMap.SetCell(0, gridPos, -1, new(0, 0));
            }
        }

        public static void Update()
        {
            GD.Print(ProjectSettings.GetSetting("display/window/size/viewport_width"));
        }
    }
}
