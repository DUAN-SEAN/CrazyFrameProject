using System;
public struct Circle : IBaseStruct
{

    private Vector2 _center;

    /// <summary>
    /// 圆的中心
    /// </summary>
    /// <value>The center.</value>
    public Vector2 Center
    {
        get { return _center; }
        set { _center = value; }
    }

    /// <summary>
    /// 圆的半径
    /// </summary>
    /// <value>The radius.</value>
    public float Radius { get; set; }

    public Circle(Vector2 center, float radius)
    {
        if (radius < 0) radius = 0;
        _center = center;
        Radius = radius;
    }

}
