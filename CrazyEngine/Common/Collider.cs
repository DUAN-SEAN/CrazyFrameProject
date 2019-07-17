using System;
public class Collider
{
    /// <summary>
    /// 碰撞机的类型
    /// </summary>
    /// <value>The type of the collider.</value>
    public ColliderType ColliderType { set; get; }
    /// <summary>
    /// 碰撞机的数据本体
    /// </summary>
    /// <value>The collider.</value>
    public IBaseStruct collider { set; get; }
    /// <summary>
    /// 包含碰撞机的物体
    /// </summary>
    public Body body;

    /// <summary>
    /// 事件：碰撞过程中一直调用
    /// </summary>
    public void OnCollisionStay(Collider collision)
    {
       

        
        body.OnCollisionStay(collision);
    }

    protected Collider StayInC;
    protected bool StayIn;

    /* 以下均为构造方法的重载 */
    public Collider(IBaseStruct baseStruct)
    {
        if (baseStruct is Point)
        {
            ColliderType = ColliderType.Point;
        }
        else if (baseStruct is Circle)
        {
            ColliderType = ColliderType.Circle;
        }
        else if (baseStruct is Rectangle)
        {
            ColliderType = ColliderType.Rectangle;
        }
        else if (baseStruct is Line)
        {
            ColliderType = ColliderType.Line;
        }

        collider = baseStruct;
    }

    public Collider()
    {
        ColliderType = ColliderType.Point;

        collider = new Point(0, 0);
    }

    public Collider(Vector2 vector)
    {
        ColliderType = ColliderType.Point;

        collider = new Point(vector);
    }

    public Collider(Vector2 vector, float radius)
    {
        ColliderType = ColliderType.Circle;

        collider = new Circle(vector, radius);
    }

    public Collider(Vector2 min, Vector2 max)
    {
        ColliderType = ColliderType.Rectangle;

        collider = new Rectangle(min, max);
    }

}
