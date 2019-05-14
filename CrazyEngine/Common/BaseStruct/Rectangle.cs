using System;
public struct Rectangle : IBaseStruct
{
    /// <summary>
    /// 矩形小值点
    /// </summary>
    /// <value>The minimum.</value>
    public Vector2 Min { set; get; }
    /// <summary>
    /// 矩形大值点
    /// </summary>
    /// <value>The max.</value>
    public Vector2 Max { set; get; }
    /// <summary>
    /// 矩形的宽
    /// </summary>
    /// <value>The length.</value>
    public float Width
    {
        get
        {
            return Max.x - Min.x;
        }
    }
    /// <summary>
    /// 矩形的高
    /// </summary>
    /// <value>The width.</value>
    public float Height
    {
        get
        {
            return Max.y - Min.y;
        }
    }

    public Rectangle(Vector2 min, Vector2 max)
    {
        if (max.x > min.x)
        {
            Min = min;
            Max = max;
        }else
        {
            Min = max;
            Max = min;
        }
    }

    public Vector2 Center
    {
        get
        {
            return new Vector2((Max.x + Min.x) / 2, (Max.y + Min.y) / 2);
        }
        set
        {
            Max = value + new Vector2(Width / 2, Height / 2);
            Min = value - new Vector2(Width / 2, Height / 2);
        }
    }
}
