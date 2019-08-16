using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class ShipActorBase :ActorBase ,
#pragma warning disable CS0535 // '“ShipActorBase”不实现接口成员“IFireControlBase.SetWeaponCd(ulong, int)”
#pragma warning disable CS0535 // '“ShipActorBase”不实现接口成员“IFireControlBase.SetWeaponBulletNum(ulong, int)”
#pragma warning disable CS0535 // '“ShipActorBase”不实现接口成员“IFireControlBase.GetWeaponCd(ulong)”
#pragma warning disable CS0535 // '“ShipActorBase”不实现接口成员“IFireControlBase.GetWeaponBulletNum(ulong)”
        IShipBaseContainer,
#pragma warning restore CS0535 // '“ShipActorBase”不实现接口成员“IFireControlBase.GetWeaponBulletNum(ulong)”
#pragma warning restore CS0535 // '“ShipActorBase”不实现接口成员“IFireControlBase.GetWeaponCd(ulong)”
#pragma warning restore CS0535 // '“ShipActorBase”不实现接口成员“IFireControlBase.SetWeaponBulletNum(ulong, int)”
#pragma warning restore CS0535 // '“ShipActorBase”不实现接口成员“IFireControlBase.SetWeaponCd(ulong, int)”
        IShipComponentBaseContainer
    {
        protected FireControlComponentBase _fireControlComponent;
        protected HealthShieldComponentBase _healthShieldComponent;
        protected ShipEventComponentBase _shipEventComponent;

        public ShipActorBase(ulong id,ILevelActorComponentBaseContainer level) : base(id,level)
        {

        }

        protected override void CreateComponent()
        {
            base.CreateComponent();
            //应该在构造器参数中添加武器集合信息
            _fireControlComponent = new FireControlComponentBase(this, level);
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

        public void InitializeFireControl(List<IWeaponBaseContainer> containers)
        {
            _fireControlComponent.InitializeFireControl(new List<int>
            {

            });
        }

        public void InitializeFireControl(List<int> containers)
        {
            _fireControlComponent.InitializeFireControl(containers);
        }

        public void SendButtonState(ulong actorid, int skilltype, int skillcontrol)
        {
            _fireControlComponent.SendButtonState(actorid, skilltype, skillcontrol);
        }



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

        public int GetWeaponBulletNum(ulong id)
        {
            return _fireControlComponent.GetWeaponBulletNum(id);
        }

        public List<IWeaponBaseContainer> GetWeaponSkills()
        {
            return _fireControlComponent.GetWeaponSkills();
        }

        public int GetWeaponCd(ulong id)
        {
            return _fireControlComponent.GetWeaponCd(id);
        }

        public void SetWeaponBulletNum(ulong id, int num)
        {
            _fireControlComponent.SetWeaponBulletNum(id, num);
        }

        public void SetWeaponCd(ulong id, int cd)
        {
            _fireControlComponent.SetWeaponCd(id, cd);
        }

        #endregion

        #region HealthShieldComponent

        public void InitializeHealthShieldBase(int hp, int shieldval, int maxshield, int shieldrecoverVal)
        {
             _healthShieldComponent.InitializeHealthShieldBase(hp, shieldval, maxshield, shieldrecoverVal);
        }

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

        public void SetHP(int hp)
        {
            _healthShieldComponent.SetHP(hp);
        }

        public void SetShieldNum(int shield)
        {
            _healthShieldComponent.SetShieldNum(shield);
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
        IFireControlInternalBase IShipComponentBaseContainer.GetFireControlinternalBase()
        {
            return _fireControlComponent;
        }

        IHealthShieldInternalBase IShipComponentBaseContainer.GetHealthShieldinternalBase()
        {
            return _healthShieldComponent;
        }

        IShipEventinternalBase IShipComponentBaseContainer.GetShipEventinternalBase()
        {
            return _shipEventComponent;
        }


        #endregion



    }
}
