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

        public InvariantAttributeComponentBase(int camp = 0,double maxSpeed  = 2,float maxForceProc = 0.0002f)
        {
            this.maxSpeed = maxSpeed;
            this.maxForceProc = maxForceProc;
            this.camp = camp;
        }

        public InvariantAttributeComponentBase(InvariantAttributeComponentBase clone)
        {
            this.maxSpeed = clone.maxSpeed;
            this.maxForceProc = clone.maxForceProc;
            this.camp = clone.camp;
            this.initData = clone.initData;
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

        #endregion


    }
}
