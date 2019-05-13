using System;
using System.Collections.Generic;

public class Body : ObjectBase
{

    private bool _static = false;
    private Vector2 _position = new Vector2();
    private Vector2 _velocity = new Vector2();
    private float _mass = 1f;
    private Vector2 _forword = new Vector2(1,0);
    private ColliderType _collierType = ColliderType.Point;
    private IBaseStruct _collier = new Point(new Vector2(0,0));
    private List<Vector2> _forceList = new List<Vector2>();

    //每当body创建时候加入world的bodies
    //public Body()
    //{
    //    World.Instanse.Bodies.Add(this);
    //}

    /// <summary>
    /// 位置
    /// </summary>
    public Vector2 Position
    {
        get { return _position; }
        set
        {
            _position = value;
        }
    }

    /// <summary>
    /// 速度
    /// </summary>
    /// <value>The velocity.</value>
    public Vector2 Velocity
    {
        get { return _velocity; }
        set
        {
            _velocity = value;
        }
    }

    /// <summary>
    /// 是否属于静态
    /// </summary>
    /// <value><c>true</c> if static; otherwise, <c>false</c>.</value>
    public bool Static
    {
        get { return _static; }
        set
        {
            _static = value;
        }
    }

    /// <summary>
    /// 质量
    /// </summary>
    /// <value>The mass.</value>
    public float Mass
    {
        get { return _mass; }
        set
        {
            _mass = value;
        }
    }

    /// <summary>
    /// 质量的倒数
    /// </summary>
    /// <value>The inverse mass.</value>
    public float InverseMass
    {
        get
        {
            return 1 / _mass;
        }
    }

    /// <summary>
    /// 朝向
    /// </summary>
    /// <value>The angle.</value>
    public Vector2 Forward
    {
        get { return _forword; }
        set
        {
            _forword = value;
        }
    }

    /// <summary>
    /// 受力
    /// </summary>
    /// <value>The force.</value>
    public Vector2 Force
    {
        get
        {
            Vector2 _force = new Vector2();
            foreach(var force in _forceList)
            {
                _force += force;
            }
            return _force;
        }
    }

    /// <summary>
    /// 碰撞机类型
    /// </summary>
    /// <value>The type of the collider.</value>
    public ColliderType ColliderType
    {
        get
        {
            return _collierType;
        }
    }

    /// <summary>
    /// 碰撞机
    /// </summary>
    /// <value>The collider.</value>
    public IBaseStruct Collider
    {
        get
        {
            //_collier.Center = _position;
            return _collier;
        }
        set
        {
            if(value is Point)
            {
                _collierType = ColliderType.Point;
            }else if(value is Circle)
            {
                _collierType = ColliderType.Circle;
            }else if(value is Rectangle)
            {
                _collierType = ColliderType.Rectangle;
            }

            _collier = value;
        }
    }

    /// <summary>
    /// 加速度
    /// </summary>
    /// <value>The acceleration.</value>
    public Vector2 Acceleration
    {
        get { return Force*InverseMass; }
    }

    //给物体施加力的方法
    public void AddForce(Vector2 force)
    {
        if(!_forceList.Contains(force))
            _forceList.Add(force);
    }

    //清空力的方法
    public void ClearForce()
    {
        _forceList.Clear();
    }

}
