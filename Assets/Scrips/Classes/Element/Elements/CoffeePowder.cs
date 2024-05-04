using Godot;
using System;
using System.Collections.Generic;

namespace ElementSandbox;

public class CoffeePowder: Powder
{
    private float _Wetness = 0;
    public float Wetness {get{return _Wetness;}}

    public CoffeePowder(): base(ElementID.COFFEEPOWDER, 0)
    {
        
    }

    public override void OnContact()
    {
        Dictionary<Vector2I, bool> neighbors = GetNeighbors();

        float neighborWetness = _Wetness;
        int wetNeighbors = 1;

        foreach (KeyValuePair<Vector2I, bool> pair in neighbors){
            if (!pair.Value) continue;

            Element neighbor = Grid.grid[GridPosition.X + pair.Key.X, GridPosition.Y + pair.Key.Y];
            if (neighbor == null) continue;

            switch (neighbor.ID) {
                case ElementID.WATER:
                    neighborWetness++;
                    wetNeighbors++;
                    break;

                case ElementID.COFFEEPOWDER:
                    if ( ((CoffeePowder)neighbor).Wetness == 0 ) break;

                    neighborWetness += ((CoffeePowder)neighbor)._Wetness;
                    wetNeighbors++;
                    break;

                default:
                    break;
            }
        }

        if (wetNeighbors == 1) return;

        float averageWetness = neighborWetness / wetNeighbors;
        if (averageWetness != 0) GD.Print(averageWetness);
        _Wetness = averageWetness;
        AtlasID = Math.Max((int)_Wetness, 2);
    }
}
