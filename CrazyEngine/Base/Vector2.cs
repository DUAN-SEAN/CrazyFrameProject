

using System;

[Serializable]
public struct Vector2
{

    public Vector2(float _x, float _y)
    {
        x = (float)Math.Round(_x, 6);
        y = (float)Math.Round(_y, 6);
    }

    public float x { private set; get; }
    public float y { private set; get; }

    public Vector2 normalized  //方向
    {
        get
        {
            if (Math.Abs(magnitude) < float.Epsilon) return new Vector2();
            return new Vector2(x / magnitude, y / magnitude);
        }
    }


    public float magnitude  //大小
    {
        get
        {
            return (float)Math.Sqrt(x * x + y * y);
        }
    }

    public void Set(float newX, float newY)
    {
        x = (float)Math.Round(newX, 6);
        y = (float)Math.Round(newY, 6);
    }

    /// <summary>
    /// 旋转（绕本体）
    /// </summary>
    public Vector2 Rotate(int degree)
    {
        float angle = (float)(degree / 360f * Math.PI * 2);
        float newX = (float)(x * Math.Cos(angle) - y * Math.Sin(angle));
        float newY = (float)(x * Math.Sin(angle) + y * Math.Cos(angle));
        return new Vector2(newX, newY);
    }

    public static float Distance(Vector2 v1,Vector2 v2)
    {
        return (float)Math.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y));
    }


    /// <summary>
    /// 方法重写和操作符重载
    /// </summary>
    /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Vector2"/>.</returns>
    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Vector2)) return false;
        return new Vector2(x, y) == (Vector2)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator ==(Vector2 vector1, Vector2 Vector2)
    {
        return Math.Abs(vector1.x - Vector2.x) < float.Epsilon && Math.Abs(vector1.y - Vector2.y) < float.Epsilon;
    }

    public static bool operator !=(Vector2 vector1, Vector2 Vector2)
    {
        return !(vector1 == Vector2);
    }

    public static Vector2 operator +(Vector2 vector1, Vector2 Vector2)
    {
        return new Vector2(vector1.x + Vector2.x, vector1.y + Vector2.y);
    }

    public static Vector2 operator -(Vector2 vector1, Vector2 Vector2)
    {
        return new Vector2(vector1.x - Vector2.x, vector1.y - Vector2.y);
    }

    public static Vector2 operator *(Vector2 vector, float num)
    {
        return new Vector2(vector.x * num, vector.y * num);
    }

    public static Vector2 operator *(float num, Vector2 vector)
    {
        return vector * num;
    }

    public static float operator *(Vector2 vector1, Vector2 vector2)
    {
        return vector1.x * vector2.y + vector2.x * vector1.y;
    }

    public static Vector2 operator /(Vector2 vector, float num)
    {
        if (Math.Abs(num) < float.Epsilon) return new Vector2();
        return new Vector2(vector.x / num, vector.y / num);
    }

}
