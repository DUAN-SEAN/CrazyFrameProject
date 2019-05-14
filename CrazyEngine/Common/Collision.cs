using System;

public enum ColliderType
{
    Point,
    Circle,
    Rectangle
}

public class Collision
{
    /// <summary>
    /// 点与点之间的碰撞检测
    /// </summary>
    /// <returns><c>true</c>,碰撞到了, <c>false</c> 没有碰撞到.</returns>
    /// <param name="point1">Point1.</param>
    /// <param name="point2">Point2.</param>
    public static bool isCollision(Point point1, Point point2)
    {
        return point1.Center == point2.Center;
    }

    /// <summary>
    /// 点与圆之间的碰撞检测
    /// </summary>
    /// <returns><c>true</c>,碰撞到了, <c>false</c> 没有碰撞到.</returns>
    /// <param name="point">Point.</param>
    /// <param name="circle">Circle.</param>
    public static bool isCollision(Point point, Circle circle)
    {
        float distance = Vector2.Distance(point.Center, circle.Center);
        return distance < circle.Radius;
    }

    /// <summary>
    /// 点与矩阵之间的碰撞检测
    /// </summary>
    /// <returns><c>true</c>,碰撞到了, <c>false</c> 没有碰撞到.</returns>
    /// <param name="point">Point.</param>
    /// <param name="rect">Rect.</param>
    public static bool isCollision(Point point, Rectangle rect)
    {
        return point.Center.x > rect.Min.x && point.Center.x < rect.Max.x &&
            point.Center.y > rect.Min.y && point.Center.x < rect.Max.y;
    }

    /// <summary>
    /// 圆与圆之间的碰撞检测
    /// </summary>
    /// <returns><c>true</c>,碰撞到了, <c>false</c> 没有碰撞到.</returns>
    /// <param name="circle1">Circle1.</param>
    /// <param name="circle2">Circle2.</param>
    public static bool isCollision(Circle circle1, Circle circle2)
    {
        float distance = Vector2.Distance(circle1.Center, circle2.Center);
        return distance < (circle1.Radius + circle2.Radius);
    }

    /// <summary>
    /// 圆和矩形之间的碰撞检测
    /// </summary>
    /// <returns><c>true</c>,碰撞到了, <c>false</c> 没有碰撞到.</returns>
    /// <param name="circle">Circle.</param>
    /// <param name="rect">Rect.</param>
    public static bool isCollision(Circle circle, Rectangle rect)
    {
        Vector2 vector = circle.Center - rect.Center;   //圆形中心与矩形中心的相对坐标

        float minX = Math.Min(vector.x, rect.Width / 2);
        float maxX = Math.Max(minX, -rect.Width / 2);
        float minY = Math.Min(vector.y, rect.Height / 2);
        float maxY = Math.Max(minY, -rect.Height / 2);

        return ((maxX - vector.x) * (maxX - vector.x) + (maxY - vector.y) * (maxY - vector.y)) <= circle.Radius * circle.Radius;
    }

    /// <summary>
    /// 矩形和矩形之间的碰撞检测
    /// </summary>
    /// <returns><c>true</c>,碰撞到了, <c>false</c> 没有碰撞到.</returns>
    /// <param name="rect1">Rect1.</param>
    /// <param name="rect2">Rect2.</param>
    public static bool isCollision(Rectangle rect1, Rectangle rect2)
    {
        if (rect1.Center.x >= rect2.Center.x && rect2.Center.x <= rect1.Center.x - rect1.Width / 2 - rect2.Width / 2) return false;
        if (rect1.Center.x <= rect2.Center.x && rect2.Center.x >= rect1.Center.x + rect1.Width / 2 + rect2.Width / 2) return false;
        if (rect1.Center.y >= rect2.Center.y && rect2.Center.y <= rect1.Center.y - rect1.Height / 2 - rect2.Height / 2) return false;
        if (rect1.Center.y <= rect2.Center.y && rect2.Center.y >= rect1.Center.y + rect1.Height / 2 + rect2.Height / 2) return false;
        return true;
    }

    /// <summary>
    /// 圆和矩形之间的碰撞检测
    /// </summary>
    /// <returns><c>true</c>,碰撞到了, <c>false</c> 没有碰撞到.</returns>
    /// <param name="rect">Circle.</param>
    /// <param name="circle">Rect.</param>
    public static bool isCollision(Rectangle rect, Circle circle)
    {
        return isCollision(circle, rect);
    }

    /// <summary>
    /// 圆和点之间的碰撞检测
    /// </summary>
    /// <returns><c>true</c>,碰撞到了, <c>false</c> 没有碰撞到.</returns>
    /// <param name="circle">Circle.</param>
    /// <param name="point">Point.</param>
    public static bool isCollision(Circle circle, Point point)
    {
        return isCollision(point, circle);
    }

    /// <summary>
    /// 矩阵和圆之间的碰撞检测
    /// </summary>
    /// <returns><c>true</c>,碰撞到了, <c>false</c> 没有碰撞到.</returns>
    /// <param name="rect">Rect.</param>
    /// <param name="point">Point.</param>
    public static bool isCollision(Rectangle rect, Point point)
    {
        return isCollision(point, rect);
    }
}
