using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using CrazyEngine.Base;


namespace GameActorLogic
{
    public class ShipActorBase :ActorBase ,
        IShipBaseContainer,
        IShipComponentBaseContainer
    {
        protected FireControlComponentBase _fireControlComponent;
        protected HealthShieldComponentBase _healthShieldComponent;
        protected ShipEventComponentBase _shipEventComponent;
        protected AIComponentBase _aiComponent;

        public ShipActorBase(ulong id,Int32 type,ILevelActorComponentBaseContainer level) : base(id, type,level)
        {
            CreateBaseComponent();
        }

        protected sealed override void CreateBaseComponent()
        {
            base.CreateBaseComponent();
            //应该在构造器参数中添加武器集合信息
            _fireControlComponent = new FireControlComponentBase(this, level);
            _healthShieldComponent = new HealthShieldComponentBase(level,this);
            _shipEventComponent = new ShipEventComponentBase();
            AddColliderFunction();
        }

        protected override void AddColliderFunction()
        {
            _physicalBase.OnColliderEnter += Collider;
        }

        protected void Collider(Body body)
        {
            //Log.Trace("船受到碰撞" + body.Id);
            var actor = level.GetEnvirinfointernalBase().GetActorByBodyId(body.Id.Value);
            if(actor == null) return;
            //Log.Trace("船受到碰撞" + actor.GetActorID() + "阵营" + actor.GetCamp());
            if (actor.GetCamp() == GetCamp()) return;
            //Log.Trace("船被攻击类型" + actor.GetType() + actor.GetActorType());
            if (actor is WeaponActorBase weapon)
            {
                //Log.Trace("船受到武器碰撞" + weapon.GetActorID() +" 伤害"+weapon.GetWeaponDamage());
                _healthShieldComponent.LossBlood(weapon.GetWeaponDamage());
            }

            if (actor is ShipActorBase ship)
            {
                //Log.Trace("船受到船碰撞" + actor.GetActorID());
                _healthShieldComponent.LossBlood(1);
            }
        }

        public override void Update()
        {
            base.Update();
            //护盾恢复逻辑需要被Tick
            _healthShieldComponent.Tick();
            _fireControlComponent.TickFire();
            _aiComponent?.Update();
        }

        public override void Dispose()
        {
            _physicalBase.OnColliderEnter -= Collider;
            _aiComponent?.Dispose();
            _aiComponent = null;

            _fireControlComponent.Dispose();
            _fireControlComponent = null;

            _healthShieldComponent = null;

            _shipEventComponent = null;

            base.Dispose();
          

           

            
        }

        public void CreateAiComponent(AIComponentBase ai)
        {
            _aiComponent = ai;
        }

        public override ActorBase Clone()
        {
            var clone = this.MemberwiseClone() as ShipActorBase;
            clone._invariantAttributeComponent = new InvariantAttributeComponentBase(clone._invariantAttributeComponent);
            clone._physicalBase = new PhysicalBase(clone._physicalBase);
            clone._moveComponent = new MoveComponentBase(clone._physicalBase);

            clone._aiComponent = clone?._aiComponent?.Clone(clone);
            clone._fireControlComponent = new FireControlComponentBase(clone,clone._fireControlComponent);
            clone._healthShieldComponent = new HealthShieldComponentBase(clone,clone._healthShieldComponent);
            clone._shipEventComponent = new ShipEventComponentBase(clone._shipEventComponent);

            clone.AddColliderFunction();
            return clone;
        }


        #region IShipBaseContainer

        #region FireControlComponent



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

        public int GetSkillCapNum(int type)
        {
            return _fireControlComponent.GetSkillCapNum(type);
        }

 

        public List<ISkillContainer> GetSkills()
        {
            return _fireControlComponent.GetSkills();
        }

        public int GetSkillCd(int type)
        {
            return _fireControlComponent.GetSkillCd(type);
        }

        public void SetSkillCapNum(int type, int num)
        {
            _fireControlComponent.SetSkillCapNum(type, num);
        }




        public void SetSkillCd(int type, int cd)
        {
            _fireControlComponent.SetSkillCd(type, cd);
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

        #region Ai组件
        public bool StartAILogic()
        {
            return _aiComponent.StartAILogic();
        }

        public bool PauseAILogic()
        {
            return _aiComponent.PauseAILogic();
        }

        AIComponentBase IAIBase.Clone(IBaseComponentContainer container)
        {
            return _aiComponent.Clone(container);
        }

        #endregion


        #region ShipEventComponent
        /// <summary>
        /// 飞船动态初始化生成调用
        /// </summary>
        public void Init()
        {
            var position = GetPosition();
            level.AddEventMessagesToHandlerForward(new InitEventMessage(ActorID, GetCamp(), ActorType, position.X,
                position.Y, GetForwardAngle()));
            _shipEventComponent.Init();
        }

        /// <summary>
        /// 飞船动态销毁调用
        /// </summary>
        public void Destroy()
        {
            //Log.Trace("销毁船："+ActorID);
            level.AddEventMessagesToHandlerForward(new DestroyEventMessage(ActorID));
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

        IAIInternalBase IShipComponentBaseContainer.GetAIinternalBase()
        {
            return _aiComponent;
        }

        IShipEventinternalBase IShipComponentBaseContainer.GetShipEventinternalBase()
        {
            return _shipEventComponent;
        }


        #endregion


    }
}
