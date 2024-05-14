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

    public override void OnContact(List<Element> contactingNeighbors)
    {
        float neighborWetness = _Wetness;
        int wetNeighbors = 1;

        foreach (Element neighbor in contactingNeighbors) {

        }
    }
}
