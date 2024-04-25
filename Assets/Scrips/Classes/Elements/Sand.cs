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

            //foreach (KeyValuePair<Vector2I, bool> pair in neighbors) {
            //    GD.Print(pair);
            //}
            bool downOccupied = neighbors[Grid.Directions["DOWN"]];
            if (!downOccupied) return Vector2I.Down;
            bool bottomLeftOccupied = neighbors[Grid.Directions["BOTTOM_LEFT"]];
            bool bottomRightOccupied = neighbors[Grid.Directions["BOTTOM_RIGHT"]];

            if (!bottomLeftOccupied && !bottomRightOccupied) {
                return Main.random.Next(0, 2) == 1 ? Grid.Directions["BOTTOM_LEFT"] : Grid.Directions["BOTTOM_RIGHT"];
            }
            else if (!bottomLeftOccupied) {
                return Grid.Directions["BOTTOM_LEFT"];
            }
            else if (!bottomRightOccupied) {
                return Grid.Directions["BOTTOM_RIGHT"];
            }

            return Vector2I.Zero;
        }
    }
}

