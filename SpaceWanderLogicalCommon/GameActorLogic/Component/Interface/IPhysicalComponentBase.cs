using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Base;
using CrazyEngine.Common;
using CrazyEngine.External;

namespace GameActorLogic { 
    /// <summary>
    /// 针对物理引擎对象的操作 对外接口
    /// 
    /// </summary>
    public interface IPhysicalBase
    {
        void InitializePhysicalBase();


        #region 物理同步

        /// <summary>
        /// 获得位置坐标
        /// </summary>
        Point GetPosition();

        /// <summary>
        /// 获取朝向
        /// </summary>
        double GetForwardAngle();

        Point GetForward();

        /// <summary>
        /// 得到当前速度矢量
        /// </summary>
        Point GetVelocity();

        /// <summary>
        /// 得到当前角速度
        /// </summary>
        double GetAngleVelocity();
        /// <summary>
        /// 得到当前力
        /// </summary>
        Point GetForce();
        /// <summary>
        /// 得到当前转矩
        /// （角加速度）
        /// </summary>
        double GetTorque();

        /// <summary>
        /// 设置物理数据
        /// </summary>
        void SetPhysicalValue(ulong actorId, double angleVelocity, double forceX, double forceY,
            double forwardAngle, double positionX, double positionY, double velocityX, double velocityY, double torque);


        
        #endregion

        /// <summary>
        /// 得到当前速度
        /// </summary>
        double GetSpeed();



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
        void AddForward(double ang);

        /// <summary>
        /// 返回自己的Body实体 用于添加到物理引擎中
        /// </summary>
        Body GetBody();

        /// <summary>
        /// 返回自己的碰撞机实体 用于添加到碰撞器中
        /// </summary>
        Collider GetCollider();



    }
}
