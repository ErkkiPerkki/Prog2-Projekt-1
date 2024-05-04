using Godot;
using System.Collections.Generic;

namespace ElementSandbox;

public class Liquid: Element
{
    public Liquid(ElementID elementID, int atlasID = 0): base(elementID, atlasID)
    {

    }

    public override Vector2I Evaluate()
    {
        Dictionary<Vector2I, bool> neighbors = GetNeighbors();

        bool downOccupied = neighbors[Grid.Directions["DOWN"]];
        if (!downOccupied) return Vector2I.Down;

        bool bottomLeftOccupied = neighbors[Grid.Directions["BOTTOM_LEFT"]];
        bool bottomRightOccupied = neighbors[Grid.Directions["BOTTOM_RIGHT"]];
        bool leftOccupied = neighbors[Vector2I.Left];
        bool rightOccupied = neighbors[Vector2I.Right];

        if (!bottomLeftOccupied && !bottomRightOccupied && (!leftOccupied && !rightOccupied)) {
            return Main.random.Next(0, 2) == 1 ? Grid.Directions["BOTTOM_LEFT"] : Grid.Directions["BOTTOM_RIGHT"];
        }
        else if (!bottomLeftOccupied && !leftOccupied) {
            return Grid.Directions["BOTTOM_LEFT"];
        }
        else if (!bottomRightOccupied && !rightOccupied) {
            return Grid.Directions["BOTTOM_RIGHT"];
        }

        else if (!leftOccupied && !rightOccupied) {
            return Main.random.Next(0, 2) == 1 ? Vector2I.Left : Vector2I.Right;
        }
        else if (!leftOccupied) {
            return Vector2I.Left;
        }
        else if (!rightOccupied) {
            return Vector2I.Right;
        }

        return Vector2I.Zero;
    }

    public override void OnContact()
    {
        
    }
}
