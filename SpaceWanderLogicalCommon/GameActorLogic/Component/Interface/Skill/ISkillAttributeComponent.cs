using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 技能属性组件
    /// </summary>
    public interface ISkillAttributeComponent
    {
        /// <summary>
        /// 设置技能容量
        /// </summary>
        int GetSkillCapacity();

        /// <summary>
        /// 获取技能容量
        /// </summary>
        void SetSkillCapacity(int cap);

        /// <summary>
        /// 获取技能CD
        /// </summary>
        int GetSkillCd();

        /// <summary>
        /// 设置技能CD
        /// </summary>
        void SetSkillCd(int cd);
    }


    public interface ISkillAttributeInternalComponent : ISkillAttributeComponent
    {

    }
}
