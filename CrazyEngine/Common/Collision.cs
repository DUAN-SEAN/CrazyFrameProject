using System;

public enum ColliderType
{
    Point,
    Line,
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
            point.Center.y > rect.Min.y && point.Center.y < rect.Max.y;
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


    /// <summary>
    /// 线和线之间的碰撞检测
    /// </summary>
    /// <param name="line1"></param>
    /// <param name="line2"></param>
    /// <returns></returns>
    public static bool isCollision(Line line1, Line line2)
    {
        if (Math.Min(line1.Start.x, line1.End.x) <= Math.Max(line2.Start.x, line2.End.x) &&
            Math.Max(line1.Start.x, line1.End.x) >= Math.Min(line2.Start.x, line2.End.x) &&
            Math.Min(line1.Start.y, line1.End.y) <= Math.Max(line2.Start.y, line2.End.y) &&
            Math.Max(line1.Start.y, line1.End.y) >= Math.Max(line2.Start.y, line2.End.y) &&
            Vector2.Mul(line2.Start, line1.Start, line2.End) * Vector2.Mul(line2.Start, line2.End, line1.End) >= 0 &&
            Vector2.Mul(line1.Start, line2.Start, line1.End) * Vector2.Mul(line1.Start, line1.End, line2.End) >= 0)
            return true;
        return false;
    }

    /// <summary>
    /// 线和点之间的碰撞检测
    /// </summary>
    /// <param name="line"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static bool isCollision(Line line, Point point)
    {
        //TODO 2
        return false;
    }

    /// <summary>
    /// 线和圆之间的碰撞检测
    /// </summary>
    /// <param name="line"></param>
    /// <param name="circle"></param>
    /// <returns></returns>
    public static bool isCollision(Line line, Circle circle)
    {
        float d1 = Vector2.Distance(line.Start, circle.Center);
        float d2 = Vector2.Distance(line.End, circle.Center);
        if (d1 < circle.Radius && d2 < circle.Radius)    //两点都在圆内
            return false;

        if (d1 < circle.Radius && d2 > circle.Radius || d1 > circle.Radius && d2 < circle.Radius)   //一点在圆内 一点在圆外 ，相交
            return true;

        float A, B, C, dist1, dist2, angle1, angle2;
        if (Math.Abs(line.Start.x - line.End.x) < float.Epsilon)
        {
            A = 1; B = 0; C = -line.Start.x;
        }
        else if (Math.Abs(line.Start.y - line.End.y) < float.Epsilon)
        {
            A = 0; B = 1; C = -line.Start.y;
        }
        else
        {
            A = line.Start.y - line.End.y;
            B = line.End.x - line.Start.x;
            C = line.Start.x * line.End.y - line.Start.y * line.End.x;
        }
        dist1 = A * circle.Center.x + B * circle.Center.y + C;
        dist1 *= dist1;
        dist2 = (A * A + B * B) * circle.Radius * circle.Radius;

        if (dist1 > dist2) return false;    //点到直线的距离大于半径R
        angle1 = (circle.Center.x - line.Start.x) * (line.End.x - line.Start.x) + (circle.Center.y - line.Start.y) * (line.End.y - line.Start.y);
        angle2 = (circle.Center.x - line.End.x) * (line.Start.x - line.End.x) + (circle.Center.y - line.End.y) * (line.Start.y - line.End.y);

        if (angle1 > 0 && angle2 > 0) return true;  //余弦都为正，则是锐角， 相交.

        return false;   //其他情况为不相交
    }

    /// <summary>
    /// 线和矩形之间的碰撞检测
    /// </summary>
    /// <param name="line"></param>
    /// <param name="rect"></param>
    /// <returns></returns>
    public static bool isCollision(Line line, Rectangle rect)
    {
        if (isCollision(line, new Line(new Vector2(rect.Min.x, rect.Min.y), new Vector2(rect.Max.x, rect.Min.y)))) return true;
        if (isCollision(line, new Line(new Vector2(rect.Min.x, rect.Max.y), new Vector2(rect.Max.x, rect.Max.y)))) return true;
        if (isCollision(line, new Line(new Vector2(rect.Min.x, rect.Min.y), new Vector2(rect.Max.x, rect.Max.y)))) return true;
        if (isCollision(line, new Line(new Vector2(rect.Min.x, rect.Max.y), new Vector2(rect.Max.x, rect.Max.y)))) return true;
        return false;
    }

    /// <summary>
    /// 点和线之间的碰撞检测
    /// </summary>
    /// <param name="point"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static bool isCollision(Point point, Line line)
    {
        return isCollision(line, point);
    }

    /// <summary>
    /// 圆和线之间的碰撞检测
    /// </summary>
    /// <param name="circle"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static bool isCollision(Circle circle, Line line)
    {
        return isCollision(line, circle);
    }

    /// <summary>
    /// 矩形和线之间的碰撞检测
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static bool isCollision(Rectangle rect, Line line)
    {
        return isCollision(line, rect);
    }
}
