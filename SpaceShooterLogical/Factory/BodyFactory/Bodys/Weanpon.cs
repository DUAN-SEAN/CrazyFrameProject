using System;
using CrazyEngine;
using SpaceShip.Base;
using SpaceShip.System;

namespace SpaceShip.Factory
{

    /// <summary>
    /// 导弹在物理世界的描述
    /// </summary>
    public class MissileInBody : PointEntity, ITickable, IAliveable
    {
        public ISBSeanDuan iSBSean;
        public virtual void Init(Level seanD)
        {
            base.InitWorld(seanD);
            iSBSean = seanD;
            iSBSean.GetWeanponList().Add(this);

        }
        public MissileInBody()
        {
            //LogUI.Log("isWeapon");
            birthtime = DateTime.Now.Ticks;
            lifetime = 50000000;
            isAlive = true;
        }
        public MissileInBody(Vector2 vector) : base(vector)
        {
            birthtime = DateTime.Now.Ticks;
            lifetime = 50000000;
            isAlive = true;
        }

        public void Tick()
        {
            if (Enable == false)
            {
                stop_dely = DateTime.Now.Ticks - currenttime;
                return;
            }
            currenttime = DateTime.Now.Ticks;
            if (DateTime.Now.Ticks - birthtime - stop_dely > lifetime)
                this.Dispose();
            //TODO 完成导弹自己的持续运行
            this.AddForce(Forward * 0.5f);
            //LogUI.Log(Forward);

        }

        public override void OnCollisionStay(Collider collider)
        {

            if (collider.body.Label.HasFlag(Label) || Label.HasFlag(collider.body.Label)) return;
            //LogUI.Log(collider.body.Label + " " + Label);
            //LogUI.Log(Position + " " + collider.body.Position);
            isAlive = false;
            Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();
            iSBSean.GetWeanponList().Remove(this);
            isAlive = false;
        }

        public bool GetAliveState()
        {
            return isAlive;
        }

        public bool isAlive;

        //单位：ms / 10000
        public long birthtime;
        public long lifetime;

        private long stop_dely;
        private long currenttime;

    }

    /// <summary>
    /// 激光导弹在物理世界的描述
    /// </summary>
    public class BoltInBody : PointEntity, ITickable, IAliveable
    {
        public ISBSeanDuan iSBSean;
        public virtual void Init(Level seanD)
        {
            base.InitWorld(seanD);
            iSBSean = seanD;
            iSBSean.GetWeanponList().Add(this);

        }
        public BoltInBody()
        {
            //LogUI.Log("isWeapon");
           
            birthtime = DateTime.Now.Ticks;
            lifetime = 50000000;
            isAlive = true;
        }
        public BoltInBody(Vector2 vector) : base(vector)
        {
           
            birthtime = DateTime.Now.Ticks;
            lifetime = 50000000;
            isAlive = true;
        }


        public void Tick()
        {

            if (Enable == false)
            {
                stop_dely = DateTime.Now.Ticks - currenttime;
                return;
            }
            currenttime = DateTime.Now.Ticks;
            if (DateTime.Now.Ticks - birthtime - stop_dely > lifetime)
                this.Dispose();
            //TODO 完成导弹自己的持续运行
            this.AddForce(Forward * 5);
            //LogUI.Log(Forward);

        }

        public override void OnCollisionStay(Collider collider)
        {
            if (collider.body.Label.HasFlag(Label) || Label.HasFlag(collider.body.Label)) return;
            //LogUI.Log(collider.body.Label + " " + Label);
            isAlive = false;
            Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();
           iSBSean.GetWeanponList().Remove(this);
            isAlive = false;
        }

        public bool GetAliveState()
        {
            return isAlive;
        }

        public bool isAlive;

        //单位：ms / 10000
        public long birthtime;
        public long lifetime;

        private long stop_dely;
        private long currenttime;

    }


    /// <summary>
    /// 地雷在物理世界的描述
    /// </summary>
    public class MineInBody : CircleEntity, ITickable, IAliveable
    {
        public ISBSeanDuan iSBSean;
        public virtual void Init(Level seanD)
        {
            base.InitWorld(seanD);
            iSBSean = seanD;
            iSBSean.GetWeanponList().Add(this);

        }
        public MineInBody()
        {
            //LogUI.Log("isWeapon");
            birthtime = DateTime.Now.Ticks;
            //lifetime = 50000000;
            isAlive = true;
        }
        public MineInBody(Vector2 vector, float r) : base(vector, r)
        {
            birthtime = DateTime.Now.Ticks;
            //lifetime = 50000000;
            isAlive = true;
        }

        public void Tick()
        {
            if (Enable == false)
            {
                stop_dely = DateTime.Now.Ticks - currenttime;
                return;
            }
            currenttime = DateTime.Now.Ticks;

            //if (DateTime.Now.Ticks - birthtime > lifetime)
            //this.Dispose();
            //TODO 完成的持续运行
            //this.AddForce(Forward * 5);
            //LogUI.Log(Forward);

        }

        public override void OnCollisionStay(Collider collider)
        {
            if (collider.body.Label.HasFlag(Label) || Label.HasFlag(collider.body.Label)) return;
            //LogUI.Log(collider.body.Label + " " + Label);
            isAlive = false;
            Dispose();
        }
        public override void Dispose()
        {
            base.Dispose();
            iSBSean.GetWeanponList().Remove(this);
            isAlive = false;
        }

        public bool GetAliveState()
        {
            return isAlive;
        }

        public bool isAlive;

        //单位：ms / 10000
        public long birthtime;
        public long lifetime;

        private long stop_dely;
        private long currenttime;

        
    }


    /// <summary>
    /// 射线激光在物理世界的描述
    /// TODO射线激光在物理世界的描述未完成
    /// </summary>
    public class LightInBody : LineEntity, ITickable, IAliveable, IWeanpon
    {

        public ISBSeanDuan iSBSean;
        public virtual void Init(Level seanD)
        {
            base.InitWorld(seanD);
            iSBSean = seanD;
            iSBSean.GetWeanponList().Add(this);

        }
        public LightInBody()
        {
            //LogUI.Log("isWeapon");
            
            birthtime = DateTime.Now.Ticks;
            lifetime = 50000000;
            isAlive = true;
        }
        public LightInBody(Vector2 start, Vector2 end) : base(start, end)
        {
            StartPoint = start;
            Length = (float)Math.Sqrt((end.x - start.x) * (end.x - start.x) + (end.y - start.y) * (end.y - start.y));
            birthtime = DateTime.Now.Ticks;
            lifetime = 50000000;
            isAlive = true;
        }

        public void Tick()
        {
            if (Enable == false)
            {
                stop_dely = DateTime.Now.Ticks - currenttime;
                return;
            }
            currenttime = DateTime.Now.Ticks;

            //if (DateTime.Now.Ticks - birthtime > lifetime)
            //this.Dispose();
            //TODO 完成自己的持续运行
            //this.AddForce(Forward * 5);
            //LogUI.Log(Forward);
            StartPoint = m_ownerbody.Position;
            this.Forward = m_ownerbody.Forward;
            //TODO
            Collider.collider = ((Line)Collider.collider).Rotate(m_ownerbody.Forward);
            //LogUI.Log(StartPoint + " " + this.Forward.normalized + " " + Length);
            //LogUI.Log(StartPoint + this.Forward.normalized * Length / 2);
            this.Position = StartPoint + this.Forward.normalized * Length / 2;
            //LogUI.Log(this.Collider.collider.Center);



        }

        public override void OnCollisionStay(Collider collider)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
            iSBSean.GetWeanponList().Remove(this);
            isAlive = false;
        }

        public bool GetAliveState()
        {
            return isAlive;
        }

        public Body GetOwnerBody()
        {
            return m_ownerbody;
        }



        public bool isAlive;

        //单位：ms / 10000
        public long birthtime;
        public long lifetime;

        private long stop_dely;
        private long currenttime;

        
    }


public interface IAliveable
    {
        bool GetAliveState();
    }
   
public interface IWeanpon
    {
        Body GetOwnerBody();
    }

}
