using System;
public class Id
{
    private static int _id;

    public int Value { get; private set; }
    public Id()
    {
        Value = _id++;
    }
    private Id(int id)
    {
        Value = id;
    }

    public static Id Create()
    {
        return new Id(_id++);
    }

    public static bool operator <(Id a, Id b)
    {
        return a.Value < b.Value;
    }

    public static bool operator >(Id a, Id b)
    {
        return a.Value > b.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
