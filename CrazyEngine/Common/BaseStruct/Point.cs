using System;
public struct Point : IBaseStruct
{
    private Vector2 _postion;

    public Vector2 Center
    { 
        set { _postion = value; }
        get { return _postion; }
    }

    public Point(Vector2 pos)
    {
        _postion = pos;
    }

    public Point(float x, float y)
    {
        _postion = new Vector2(x, y);
    }
}
