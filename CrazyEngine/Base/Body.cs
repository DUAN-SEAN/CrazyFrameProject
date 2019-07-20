using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace CrazyEngine
{
    
    
    public class Body : ObjectBase
    {
        protected bool _static = false;
        protected Vector2 _position = new Vector2();
        protected Vector2 _velocity = new Vector2();
        protected float _mass = 1f;
        protected Vector2 _forward = new Vector2(1,0);
        protected Collider _collider = new Collider();
        protected float _maxVelocity = 100;
        protected List<Vector2> _forceList = new List<Vector2>();
        protected Body m_ownerbody;
        protected string userid;

        protected IGetWorld currentWorld;

        //每当body创建时候加入world的bodies
        public Body()
        {
        }
        public virtual void InitWorld(IGetWorld getWorld)
        {
            currentWorld = getWorld;
            getWorld.GetCurrentWorld().Bodies.Add(this);

        }
       
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        public Body Owner
        {
            get { return m_ownerbody; }
            set { m_ownerbody = value; }
        }

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
            get { return _forward; }
            set
            {
                _forward = value;
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

        public float MaxVelocity
        {
            set
            {
                _maxVelocity = value;
            }
            get
            {
                return _maxVelocity;
            }
        }

        /// <summary>
        /// 碰撞机
        /// </summary>
        /// <value>The collider.</value>
        public Collider Collider
        {
            get
            {
                _collider.collider.Center = _position;  //collider始终附在物体上
                _collider.body = this;
                return _collider;
            }
            set
            {
                _collider = value;
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

        public bool StayIn;


        /// <summary>
        /// 碰撞后一直调用
        /// </summary>
        /// <param name="collider">Collider.</param>
        public virtual void OnCollisionStay(Collider collider)
        {
       
        }

        /// <summary>
        /// 碰撞第一次被调用一次
        /// </summary>
        /// <param name="collider"></param>
        public virtual void OnCollisionStayIn(Collider collider)
        {

        }

        /// <summary>
        /// 碰撞结束被调用一次
        /// </summary>
        /// <param name="collider"></param>
        public virtual void OnCollisionStayOut(Collider collider)
        {

        }
        public virtual void Dispose()
        {
           currentWorld.GetCurrentWorld().Bodies.Remove(this);
        }
    }

}