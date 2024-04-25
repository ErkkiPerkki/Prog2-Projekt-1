using Godot;
using System.Collections.Generic;

namespace ElementSandbox
{
    public class Sand: Element
    {
        public Sand(): base(ElementID.SAND)
        {

        }

        public override Vector2I? Evaluate()
        {
            //Dictionary<Vector2I, bool> neighbors = GetNeighbors();

            //bool downOccupied = neighbors[Grid.Directions["DOWN"]];
            //if (downOccupied) return null;

            return new (0, -1);
        }
    }
}

