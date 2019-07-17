using System;
abstract public class ShipBase : RectangleEntity
{
    public int HP;
    public int Armor;
    public float IntervalTime;
    public int maxArmor;

    public bool isLeft;
    public bool isRight;
    public ShipInWorld shipinworld;

    public ShipBase()
    {
        HP = 10;
        Armor = 10;
        maxArmor = 10;
        IntervalTime = 5;
    }
    public ShipBase(Vector2 min, Vector2 max):base(min,max)
    {
        HP = 10;
        Armor = 10;
        maxArmor = 10;
        IntervalTime = 5;

    }

    public override void OnCollisionStay(Collider collider)
    {
        if (collider.body.Label.HasFlag(Label) || Label.HasFlag(collider.body.Label)) return;
        LogUI.Log(collider.body.Label + " " +Label);
        //LogUI.Log(Position + " " + collider.body.Position);
        if (Armor > 0) Armor--;
        else if (Armor == 0) HP--;

        if(HP == 0)
        Dispose();
    }

    public override void Dispose()
    {
        HP = 0;
        if(shipinworld!=null)
        shipinworld.Destroy();
        base.Dispose();
    }
}
