using Godot;
using System;
using System.Collections.Generic;

namespace ElementSandbox;

public class CoffeePowder: Powder
{
    private float _Wetness = 0;
    public float Wetness { get{return _Wetness;} }

    public CoffeePowder(): base(ElementID.COFFEEPOWDER, 0)
    {
        
    }

    public override void OnContact()
    {
        Dictionary<Vector2I, bool> neighbors = GetNeighbors();

        float neighborWetness = _Wetness;
        int wetNeighbors = 1;

        foreach (KeyValuePair<Vector2I, bool> pair in neighbors) {
            if (!pair.Value) continue;

            Vector2I pos = new(GridPosition.X + pair.Key.X, GridPosition.Y + pair.Key.Y);
            if (!Grid.IsOnGrid(pos)) continue;

            Element neighbor = Grid.grid[pos.X, pos.Y];
            if (neighbor == null) continue;

            switch (neighbor.ID) {
                case ElementID.WATER:
                    neighborWetness++;
                    wetNeighbors++;
                    break;

                case ElementID.COFFEEPOWDER:
                    if (((CoffeePowder)neighbor).Wetness == 0) break;

                    neighborWetness += ((CoffeePowder)neighbor)._Wetness;
                    wetNeighbors++;
                    break;

                default:
                    break;
            }
        }

        //if (wetNeighbors == 1) return;

        float averageWetness = neighborWetness / wetNeighbors;
        _Wetness = averageWetness;
        AtlasID = Math.Min((int)(_Wetness * 10), 9);

        foreach (KeyValuePair<Vector2I, bool> pair in neighbors) {
            if (!pair.Value) continue;

            Vector2I pos = new(GridPosition.X + pair.Key.X, GridPosition.Y + pair.Key.Y);
            if (!Grid.IsOnGrid(pos)) continue;

            Element neighbor = Grid.grid[pos.X, pos.Y];
            if (neighbor == null) continue;

            if (neighbor.ID == ElementID.COFFEEPOWDER) {
                ((CoffeePowder)neighbor)._Wetness = averageWetness;
                neighbor.AtlasID = Math.Min((int)(((CoffeePowder)neighbor)._Wetness * 10), 9);
                GD.Print(neighbor.AtlasID);
            }
        }

    }
}
