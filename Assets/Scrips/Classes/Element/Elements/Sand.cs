using Godot;
using System.Collections.Generic;

namespace ElementSandbox;

// The sand element is a basic powder with no special features
public class Sand: Powder
{
    public Sand(): base(ElementID.SAND, Main.random.Next(0, 4))
    {
        
    }
}

