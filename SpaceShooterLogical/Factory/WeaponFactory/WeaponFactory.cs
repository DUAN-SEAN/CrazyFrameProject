using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class WeaponFactory : IDisposable
{
    
    private WeaponFactory()
    {

        ResourcesComponent.Instance.LoadBundle(AssetBundleName.WeaponAssetBundle);
    }

    public void Dispose()
    {
        ResourcesComponent.Instance.UnloadBundle(AssetBundleName.WeaponAssetBundle);

    }






    public Body LoadWeaponFromAssetBundle(string prefab,Body body,Vector2 position = default,Vector2 forward = default)
    {
        if (!LoadedWeaponDict.TryGetValue(prefab, out GameObject gameObject))
        {
            gameObject = ResourcesComponent.Instance.GetAsset(AssetBundleName.WeaponAssetBundle, prefab) as GameObject;
            gameObject.SetActive(false);
            LoadedWeaponDict[prefab] = gameObject;
        }
        if (forward == Vector2.Zero) forward = body.Forward;
        if (position == Vector2.Zero) position = body.Position;

        Body body_weanpon;
        switch (prefab)
        {
            case WeaponsName.Missile:

                body_weanpon = BodyFactory.Instance.LoadMissileWeaponByType<MissileInBody>(body, position, forward);

                GameObject game1 = UnityEngine.Object.Instantiate(gameObject);
                ((MissileInBody)body_weanpon).missileinworld = game1.GetComponent<MissileInWorld>();
                game1.GetComponent<MissileInWorld>().m_body = body_weanpon;
                game1.GetComponent<MissileInWorld>().Update();

                //LogUI.Log(game.GetComponent<MissileInWorld>().m_body);
                break;


            case WeaponsName.Bolt:
                //LogUI.Log("bolt:" + body);
                body_weanpon = BodyFactory.Instance.LoadBoltWeaponByType<BoltInBody>(body,position,forward);
                
                GameObject game2 = UnityEngine.Object.Instantiate(gameObject);
                ((BoltInBody)body_weanpon).boltinworld = game2.GetComponent<BoltInWorld>();
                game2.GetComponent<BoltInWorld>().m_body = body_weanpon;
                game2.GetComponent<BoltInWorld>().Update();
                break;

            case WeaponsName.Mine:

                body_weanpon = BodyFactory.Instance.LoadMineWeaponByType<MineInBody>(body, position, forward);

                GameObject game3 = UnityEngine.Object.Instantiate(gameObject);
                ((MineInBody)body_weanpon).mineinworld = game3.GetComponent<MineInWorld>();
                game3.GetComponent<MineInWorld>().m_body = body_weanpon;
                game3.GetComponent<MineInWorld>().Update();
                break;

            case WeaponsName.Light:

                body_weanpon = BodyFactory.Instance.LoadLightWeaponByType<LightInBody>(body, position, forward);

                GameObject game4 = UnityEngine.Object.Instantiate(gameObject);
                ((LightInBody)body_weanpon).lightinworld = game4.GetComponent<LightInWorld>();
                game4.GetComponent<LightInWorld>().m_body = body_weanpon;
                game4.GetComponent<LightInWorld>().Update();
                break;

            default:
                body_weanpon = new Body();
                break;
        }
        body_weanpon.Label = body.Label | Label.WEAPON;
        
        return body_weanpon;
    }

  

    public static WeaponFactory Instance
    {
        get
        {
            if (m_weaponfactory == null) m_weaponfactory = new WeaponFactory();
            return m_weaponfactory;
        }
    }


    private Dictionary<string, GameObject> LoadedWeaponDict = new Dictionary<string, GameObject>();
    private static WeaponFactory m_weaponfactory;
}


public static class WeaponsName
{
    public const string Missile = "Missile";
    public const string Mine = "Mine";
    public const string Light = "Light";
    public const string Bolt = "Bolt";
}

/// <summary>
/// 导弹在物理世界的描述
/// </summary>
public class MissileInBody : PointEntity,ITickable,IAliveable
{
    public MissileInBody()
    {
        //LogUI.Log("isWeapon");
        WeaponGameLogic.Instance.moveWeaponsList.Add(this);
        birthtime = DateTime.Now.Ticks;
        lifetime = 50000000;
        isAlive = true;
    }
    public MissileInBody(Vector2 vector) : base(vector)
    {
        WeaponGameLogic.Instance.moveWeaponsList.Add(this);
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
        LogUI.Log(collider.body.Label + " " + Label);
        //LogUI.Log(Position + " " + collider.body.Position);
        isAlive = false;
        Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();
        WeaponGameLogic.Instance.moveWeaponsList.Remove(this);
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

    public MissileInWorld missileinworld;
}

/// <summary>
/// 激光导弹在物理世界的描述
/// </summary>
public class BoltInBody : PointEntity, ITickable, IAliveable
{
    public BoltInBody()
    {
        //LogUI.Log("isWeapon");
        WeaponGameLogic.Instance.moveWeaponsList.Add(this);
        birthtime = DateTime.Now.Ticks;
        lifetime = 50000000;
        isAlive = true;
    }
    public BoltInBody(Vector2 vector) : base(vector)
    {
        WeaponGameLogic.Instance.moveWeaponsList.Add(this);
        birthtime = DateTime.Now.Ticks;
        lifetime = 50000000;
        isAlive = true;
    }

    
    public void Tick()
    {

        if(Enable == false)
        {
            stop_dely = DateTime.Now.Ticks - currenttime;
            return;
        }
        currenttime = DateTime.Now.Ticks;
        if (DateTime.Now.Ticks - birthtime - stop_dely> lifetime)
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
        WeaponGameLogic.Instance.moveWeaponsList.Remove(this);
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

    public BoltInWorld boltinworld;
}


/// <summary>
/// 地雷在物理世界的描述
/// </summary>
public class MineInBody : CircleEntity, ITickable, IAliveable
{
    public MineInBody()
    {
        //LogUI.Log("isWeapon");
        WeaponGameLogic.Instance.moveWeaponsList.Add(this);
        birthtime = DateTime.Now.Ticks;
        //lifetime = 50000000;
        isAlive = true;
    }
    public MineInBody(Vector2 vector,float r) : base(vector,r)
    {
        WeaponGameLogic.Instance.moveWeaponsList.Add(this);
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
        WeaponGameLogic.Instance.moveWeaponsList.Remove(this);
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

    public MineInWorld mineinworld;
}


/// <summary>
/// 射线激光在物理世界的描述
/// TODO射线激光在物理世界的描述未完成
/// </summary>
public class LightInBody : LineEntity, ITickable, IAliveable,IWeanpon
{
   
    
    public LightInBody()
    {
        //LogUI.Log("isWeapon");
        WeaponGameLogic.Instance.moveWeaponsList.Add(this);
        birthtime = DateTime.Now.Ticks;
        lifetime = 50000000;
        isAlive = true;
    }
    public LightInBody(Vector2 start,Vector2 end) : base(start,end)
    {
        StartPoint = start;
        Length = (float)Math.Sqrt((end.x - start.x) * (end.x - start.x) + (end.y - start.y) * (end.y - start.y));
        WeaponGameLogic.Instance.moveWeaponsList.Add(this);
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
        this.Position = StartPoint + this.Forward.normalized * Length/2;
        //LogUI.Log(this.Collider.collider.Center);



    }

    public override void OnCollisionStay(Collider collider)
    {
       
    }

    public override void Dispose()
    {
        base.Dispose();
        WeaponGameLogic.Instance.moveWeaponsList.Remove(this);
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

    public LightInWorld lightinworld;
}
