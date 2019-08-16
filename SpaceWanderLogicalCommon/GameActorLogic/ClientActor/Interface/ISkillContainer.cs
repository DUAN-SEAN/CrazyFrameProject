using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 技能容器
    /// </summary>
    public interface ISkillContainer:
        IBaseContainer,
        ISkillAttributeComponent
    {

    }


    public interface ISkillComponentContainer:
        IBaseComponentContainer
    {
        ISkillAttributeInternalComponent GetSkillAttributeInternalComponent();
    } 
}
