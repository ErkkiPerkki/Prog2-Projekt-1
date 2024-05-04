namespace ElementSandbox;

public class Water: Liquid
{
    public Water(): base(ElementID.WATER, Main.random.Next(0, 3))
    {

    }

    public override void OnContact()
    {
        
    }
}
