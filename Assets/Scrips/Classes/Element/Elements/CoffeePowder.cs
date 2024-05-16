using Godot;
using System;
using System.Collections.Generic;

namespace ElementSandbox;

// CoffeePowder is an element which when combined with water WAS supposed to slowly convert into
// liquid coffee
public class CoffeePowder: Powder
{
    private float _Wetness = 0;
    public float Wetness { get{return _Wetness;} }

    public CoffeePowder(): base(ElementID.COFFEEPOWDER, 0)
    {
        
    }

    // CoffeePowder logic
    public override void OnContact(List<Element> contactingNeighbors)
    {
        float neighborWetness = _Wetness;
        int wetNeighbors = 1;

        foreach (Element neighbor in contactingNeighbors) {

        }
    }
}
