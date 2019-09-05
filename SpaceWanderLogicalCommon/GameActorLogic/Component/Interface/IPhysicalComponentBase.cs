using Box2DSharp.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace GameActorLogic { 
    /// <summary>
    /// 针对物理引擎对象的操作 对外接口
    /// 
    /// </summary>
    public interface IPhysicalBase
    {

        /// <summary>
        /// 碰撞
        /// </summary>
        void OnCollision(UserData data);

        /// <summary>
        /// 碰撞进入
        /// </summary>
        void OnContactEnter(UserData data);

        /// <summary>
        /// 碰撞退出
        /// </summary>
        void OnContactExit(UserData data);


        /// <summary>
        /// 获取相对坐标X
        /// </summary>
        float GetRelPositionX();

        /// <summary>
        /// 获取相对坐标Y
        /// </summary>
        float GetRelPositionY();

        /// <summary>
        /// 设置相对位置
        /// </summary>
        void SetRelPosition(float x, float y);
        void InitializePhysicalBase();

        UserData GetBodyUserData();

        #region 物理同步

        /// <summary>
        /// 获得位置坐标
        /// </summary>
        Vector2 GetPosition();


        /// <summary>
        /// 获取朝向
        /// </summary>
        float GetForwardAngle();

        Vector2 GetForward();

        /// <summary>
        /// 得到当前速度矢量
        /// </summary>
        Vector2 GetVelocity();



        /// <summary>
        /// 得到当前角速度
        /// </summary>
        float GetAngleVelocity();
        /// <summary>
        /// 得到当前力
        /// </summary>
        Vector2 GetForce();
        /// <summary>
        /// 得到当前转矩
        /// （角加速度）
        /// </summary>
        float GetTorque();

        /// <summary>
        /// 角速度
        /// </summary>
        void SetAngularVelocity(float vel);

        /// <summary>
        /// 设置物理数据
        /// </summary>
        void SetPhysicalValue(ulong actorId, float angleVelocity, float forceX, float forceY,
            float forwardAngle, float positionX, float positionY, float positionPrevX, float positionPrevY, float velocityX, float velocityY, float torque);



        #endregion



   


    }


    /// <summary>
    /// 针对物理引擎对象的操作 对内接口
    /// </summary>
    public interface IPhysicalInternalBase : IPhysicalBase
    {
        /// <summary>
        /// 给飞船一个与朝向相同的推力
        /// </summary>
        void AddThrust(float pro);

        /// <summary>
        /// 给一个弧度表示转向弧度
        /// 正负表示方向
        /// 正数向左
        /// 负数向右
        /// 默认0.0000001
        /// </summary>
        void AddForward(float ang);

       

        /// <summary>
        /// 返回自己的Body实体 用于添加到物理引擎中
        /// </summary>
        Body GetBody();





    }
}
