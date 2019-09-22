using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 技能事件组件
    /// </summary>
    public interface ISkillEventComponent
    {
        /// <summary>
        /// 当技能被实例化后 被组件外部开启
        /// </summary>
        void StartSkill();

        /// <summary>
        /// 技能被组件外部关闭或结束
        /// </summary>
        void EndSkill();

        /// <summary>
        /// 技能被组件外部销毁
        /// </summary>
        void DestroySkill();

        /// <summary>
        /// 当技能被实例化后 被组件外部开启
        /// </summary>
        event Action<IWeaponBaseContainer> OnStartSkill;

        /// <summary>
        /// 技能被组件外部关闭或结束
        /// </summary>
        event Action<IWeaponBaseContainer> OnEndSkill;

        /// <summary>
        /// 技能被组件外部销毁
        /// </summary>
        event Action<IWeaponBaseContainer> OnDestroySkill;
    }


    public interface ISkillEventInternalComponent : ISkillEventComponent
    {
        /// <summary>
        /// 当武器被实例化后 被组件外部开启
        /// </summary>
        event Action OnStartSkillInternal;

        /// <summary>
        /// 武器被组件外部关闭或结束
        /// </summary>
        event Action OnEndSkillInternal;

        /// <summary>
        /// 武器被组件外部销毁
        /// </summary>
        event Action OnDestroySkillInternal;
    }
}
