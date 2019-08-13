using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class ShipActorBase :ActorBase ,
        IShipBaseContainer,
        IShipComponentBaseContainer
    {
        protected FireControlComponentBase _fireControlComponent;
        protected HealthShieldComponentBase _healthShieldComponent;
        protected ShipEventComponentBase _shipEventComponent;

        public ShipActorBase(ulong id) : base(id)
        {

        }

        protected override void CreateComponent()
        {
            base.CreateComponent();
            //应该在构造器参数中添加武器集合信息
            _fireControlComponent = new FireControlComponentBase(new List<IWeaponBaseContainer>
            {
                //在这之中添加武器actor实体
                //new WeaponActorBase()
            });
            _healthShieldComponent = new HealthShieldComponentBase();
            _shipEventComponent = new ShipEventComponentBase();
        }

        public override void Update()
        {
            //护盾恢复逻辑需要被Tick
            _healthShieldComponent.Tick();
        }

        #region IShipBaseContainer

        #region FireControlComponent
        /// <summary>
        /// 发射武器
        /// </summary>
        /// <param name="i">全局武器类型</param>
        public void Fire(int i)
        {
            _fireControlComponent.Fire(i);
        }

        /// <summary>
        /// 关闭制定武器功能
        /// </summary>
        /// <param name="i">全局武器类型</param>
        public void End(int i)
        {
            _fireControlComponent.End(i);
        }

        /// <summary>
        /// 销毁武器
        /// </summary>
        /// <param name="i">全局武器类型</param>
        public void Destroy(int i)
        {
            _fireControlComponent.Destroy(i);
        }

        #endregion




        #region HealthShieldComponent
        /// <summary>
        /// 获得当前血量
        /// </summary>
        public int GetHP()
        {
            return _healthShieldComponent.GetHP();
        }

        /// <summary>
        /// 获得当前护盾值
        /// </summary>
        public int GetShieldNum()
        {
            return _healthShieldComponent.GetShieldNum();
        }

        /// <summary>
        /// 减少护盾恢复间隔
        /// </summary>
        public void ReduceRecoveryInterval(int shieldrecovery)
        {
            _healthShieldComponent.ReduceRecoveryInterval(shieldrecovery);
        }

        /// <summary>
        /// 减少护盾恢复间隔
        /// </summary>
        public void AddRecoveryVal(int recoveryval)
        {
            _healthShieldComponent.AddRecoveryVal(recoveryval);
        }

        /// <summary>
        /// 增加护盾上限
        /// </summary>
        public void AddShieldMax(int shieldrecovery)
        {
            _healthShieldComponent.AddShieldMax(shieldrecovery);
        }
        #endregion


        #region ShipEventComponent
        /// <summary>
        /// 飞船动态初始化生成调用
        /// </summary>
        public void Init()
        {
            _shipEventComponent.Init();
        }

        /// <summary>
        /// 飞船动态销毁调用
        /// </summary>
        public void Destroy()
        {
            _shipEventComponent.Destroy();
        }

        #endregion

        #endregion



        #region IShipComponentBaseContainer
        public IFireControlInternalBase GetFireControlinternalBase()
        {
            return _fireControlComponent;
        }

        public IHealthShieldInternalBase GetHealthShieldinternalBase()
        {
            return _healthShieldComponent;
        }

        public IShipEventinternalBase GetShipEventinternalBase()
        {
            return _shipEventComponent;
        }


        #endregion



    }
}
