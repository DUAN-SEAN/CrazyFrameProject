using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 基础不变类型组件
    /// </summary>
    public class InvariantAttributeComponentBase :
        IInvariantAttributeBase,
        IInvariantAttributeInternalBase
    {
        /// <summary>
        /// 移动的最大速度
        /// </summary>
        protected double maxSpeed;

        /// <summary>
        /// 移动最大推力
        /// </summary>
        protected float maxForceProc;

        protected int camp;

        protected InitData initData;

        protected bool isDead;

        protected BoomDataPackage BoomDataPackage;

        /// <summary>
        /// 生成body前使用 用于判断需要生成多大的形状
        /// 主要用于激光 蓄力或其他内容
        /// </summary>
        protected float time;

        public InvariantAttributeComponentBase(int camp = 0, double maxSpeed = 2, float maxForceProc = 0.0002f)
        {
            this.maxSpeed = maxSpeed;
            this.maxForceProc = maxForceProc;
            this.camp = camp;
            BoomDataPackage = new BoomDataPackage(40, 100);
        }

        public InvariantAttributeComponentBase(InvariantAttributeComponentBase clone)
        {
            this.maxSpeed = clone.maxSpeed;
            this.maxForceProc = clone.maxForceProc;
            this.camp = clone.camp;
            this.initData = clone.initData;
            this.BoomDataPackage = clone.BoomDataPackage;
        }
        #region IInvariantAttributeInternalBase

        public void InitializeInvariantAttributeBase(int camp, double maxSpeed, float maxForceProc)
        {
            this.maxSpeed = maxSpeed;
            this.maxForceProc = maxForceProc;
            this.camp = camp;
        }




        public double GetMaxSpeed()
        {
            return maxSpeed;
        }

        public float GetMaxForceProc()
        {
            return maxForceProc;
        }

        public int GetCamp()
        {
            return camp;
        }

        public void SetCamp(int camp)
        {
            this.camp = camp;
        }

        public void CreateInitData(InitData data)
        {
            initData = data;
        }
        public void SetInitData(float x, float y, float angle)
        {
            initData.point_x = x;
            initData.point_y = y;
            initData.angle = angle;
        }

        public InitData GetInitData()
        {
            return initData;
        }

        public bool GetDeadState()
        {
            return isDead;
        }

        public void SetDeadState(bool dead)
        {
            isDead = dead;
        }

        public float GetActorInitPro()
        {
            return time;
        }

        public void SetActorInitPro(float pro)
        {
            time = pro;
        }

        public BoomDataPackage GetBoomData()
        {
            return BoomDataPackage;
        }

        #endregion


    }



    public struct BoomDataPackage
    {
        /// <summary>
        /// 爆炸距离
        /// </summary>
        public float boomdis;

        /// <summary>
        /// 爆炸力
        /// </summary>
        public float boomForce;

        public BoomDataPackage(float dis,float Force)
        {
            boomdis = dis;
            boomForce = Force;
        }

    }

}
