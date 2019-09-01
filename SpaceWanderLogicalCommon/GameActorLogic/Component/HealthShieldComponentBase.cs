
using System;
using System.Diagnostics;
using Crazy.Common;

namespace GameActorLogic
{
    /// <summary>
    /// 完整的血量护盾组件
    /// 目前不会有特化功能
    /// 应该会去掉抽象标签
    /// </summary>
    /// 已去掉抽象标签
    public class HealthShieldComponentBase: 
        IHealthShieldBase,
        IHealthShieldInternalBase
    {
        /// <summary>
        /// 关卡对象
        /// </summary>
        protected ILevelActorComponentBaseContainer level;

        protected IShipBaseContainer Actor;

        protected int _hp;
        protected int _shieldval;

        /// <summary>
        /// 上次护盾恢复时间
        /// </summary>
        protected long lastTime;
        /// <summary>
        /// 护盾恢复间隔
        /// </summary>
        protected long recoveryinterval;
        /// <summary>
        /// 最大护盾值
        /// </summary>
        protected int _maxshieldVal;
        /// <summary>
        /// 默认 护盾恢复值
        /// </summary>
        protected int _shieldrecoverVal;


        #region 外界改变字段 每帧清0
        /// <summary>
        /// 外界增加的护盾上限
        /// </summary>
        protected int _addmaxshieldVal;

        /// <summary>
        /// 外界
        /// 减少的护盾恢复间隔
        /// </summary>
        protected long reducerecoveryinterval;

        /// <summary>
        /// 外界
        /// 增加护盾恢复数值
        /// </summary>
        protected int _addshieldrecoverVal;


        #endregion


        public HealthShieldComponentBase(ILevelActorComponentBaseContainer level, IShipBaseContainer ActorId)
        {
            _hp = 3;
            _shieldval = 0;
            _maxshieldVal = 1;
            _shieldrecoverVal = 1;
            _addshieldrecoverVal = 0;
            reducerecoveryinterval = 0;
            this.level = level;
            this.Actor = ActorId;
            lastTime = DateTime.Now.Ticks;
        }
        public  HealthShieldComponentBase(int hp,int shieldval,int maxshield,int shieldrecoverVal, ILevelActorComponentBaseContainer level, IShipBaseContainer ActorId)
        {
            _hp = hp;
            _shieldval = shieldval;
            _maxshieldVal = maxshield;
            _shieldrecoverVal = shieldrecoverVal;
            _addshieldrecoverVal = 0;
            reducerecoveryinterval = 0;
            lastTime = DateTime.Now.Ticks;
            this.level = level;
            this.Actor = ActorId;
        }

        public HealthShieldComponentBase(IShipComponentBaseContainer container,HealthShieldComponentBase clone)
        {
            _hp = clone._hp;
            _shieldval = clone._shieldval;
            _maxshieldVal = clone._maxshieldVal;
            _shieldrecoverVal = clone._shieldrecoverVal;
            _addshieldrecoverVal = clone._addshieldrecoverVal;
            reducerecoveryinterval = clone.reducerecoveryinterval;
            lastTime = DateTime.Now.Ticks;
            level = clone.level;
            Actor = container;
        }

        
        public virtual void Tick()
        {
            //TODO 回复护盾功能
            if (DateTime.Now.Ticks - lastTime > recoveryinterval - reducerecoveryinterval)
            {
                _shieldval += _shieldrecoverVal + _addshieldrecoverVal;
                if (_shieldval > _maxshieldVal + _addmaxshieldVal)
                    _shieldval = _maxshieldVal + _addmaxshieldVal;


                _addmaxshieldVal = 0;
                reducerecoveryinterval = 0;
                _addshieldrecoverVal = 0;
                lastTime = DateTime.Now.Ticks;

            }
        }


        #region IHealthShieldBase

        public void Initialize(int hp, int shieldval, int maxshield, int shieldrecoverVal)
        {
            _hp = hp;
            _shieldval = shieldval;
            _maxshieldVal = maxshield;
            _shieldrecoverVal = shieldrecoverVal;
            _addshieldrecoverVal = 0;
            reducerecoveryinterval = 0;
            lastTime = DateTime.Now.Ticks;
        }

        public void InitializeHealthShieldBase(int hp, int shieldval, int maxshield, int shieldrecoverVal)
        {
            _hp = hp;
            _shieldval = shieldval;
            _maxshieldVal = maxshield;
            _shieldrecoverVal = shieldrecoverVal;
            _addshieldrecoverVal = 0;
            reducerecoveryinterval = 0;
            lastTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetHP()
        {
            return _hp;
        }
        /// <summary>
        /// 获取护盾
        /// </summary>
        /// <returns></returns>
        public int GetShieldNum()
        {
            return _shieldval;
        }

        public void SetHP(int hp)
        {
            
        }

        public void SetShieldNum(int shield)
        {
            
        }

        public void ReduceRecoveryInterval(int shieldrecoveryinterval)
        {
            reducerecoveryinterval = shieldrecoveryinterval;
        }

        public void AddRecoveryVal(int recoveryval)
        {
            _addshieldrecoverVal = recoveryval;
        }

        public void AddShieldMax(int shieldmax)
        {
            _addmaxshieldVal = shieldmax;
        }

        #endregion

        #region IHealthShieldInternalBase
        public void LossBlood(int loss)
        {
            //Log.Trace("掉血："+loss);
            if (_shieldval > 0)
            {
                //Log.Trace("护盾掉血："+loss);
                _shieldval -= loss;
                if (_shieldval < 0)
                {
                    //Log.Trace("护盾不够 开始掉血："+loss);
                    _hp += _shieldval;
                    _shieldval = 0;
                }
            }
            else
            {
                    //Log.Trace("直接开始掉血："+loss);
                _hp -= loss;
            }

            if (_hp <= 0)
            {
                //Log.Trace("血量无："+Actor.GetActorID());
                Actor.Destroy();
            }
        }

        public void AddBlood(int get)
        {
            if (_hp > 0) _hp += get;
        }
        #endregion

        public void Dispose()
        {
            level = null;
        }

    }
}
