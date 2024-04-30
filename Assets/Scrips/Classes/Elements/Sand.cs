using Godot;
using System.Collections.Generic;

namespace ElementSandbox
{
    public class Sand: Element
    {
        public Sand(): base(ElementID.SAND, Main.random.Next(0, 4))
        {
            
        }

        public override Vector2I Evaluate()
        {
            Dictionary<Vector2I, bool> neighbors = GetNeighbors();

            bool downOccupied = neighbors[Vector2I.Down];
            if (!downOccupied) return Vector2I.Down;

            bool bottomLeftOccupied = neighbors[Grid.Directions["BOTTOM_LEFT"]];
            bool bottomRightOccupied = neighbors[Grid.Directions["BOTTOM_RIGHT"]];
            bool leftOccupied = neighbors[Vector2I.Left];
            bool rightOccupied = neighbors[Vector2I.Right];

            if ((!bottomLeftOccupied && !bottomRightOccupied) && (!leftOccupied && !rightOccupied)) {
                return Main.random.Next(0, 2) == 1 ? Grid.Directions["BOTTOM_LEFT"] : Grid.Directions["BOTTOM_RIGHT"];
            }
            else if (!bottomLeftOccupied && !leftOccupied) {
                return Grid.Directions["BOTTOM_LEFT"];
            }
            else if (!bottomRightOccupied && !rightOccupied) {
                return Grid.Directions["BOTTOM_RIGHT"];
            }

            return Vector2I.Zero;
        }
    }
}

