
using System;

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


        public HealthShieldComponentBase()
        {
            _hp = 3;
            _shieldval = 0;
            _maxshieldVal = 1;
            _shieldrecoverVal = 1;
            _addshieldrecoverVal = 0;
            reducerecoveryinterval = 0;
            lastTime = DateTime.Now.Ticks;
        }
        public  HealthShieldComponentBase(int hp,int shieldval,int maxshield,int shieldrecoverVal)
        {
            _hp = hp;
            _shieldval = shieldval;
            _maxshieldVal = maxshield;
            _shieldrecoverVal = shieldrecoverVal;
            _addshieldrecoverVal = 0;
            reducerecoveryinterval = 0;
            lastTime = DateTime.Now.Ticks;

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
            if (_shieldval > 0)
            {
                _shieldval -= loss;
                if (_shieldval < 0)
                {
                    _hp += _shieldval;
                    _shieldval = 0;
                }
            }
            else
            {
                _hp -= loss;
            }

            if (_hp <= 0)
            {
                //TODO 调用死亡方法
            }
        }

        public void AddBlood(int get)
        {
            if (_hp > 0) _hp += get;
        }
        #endregion



    }
}
