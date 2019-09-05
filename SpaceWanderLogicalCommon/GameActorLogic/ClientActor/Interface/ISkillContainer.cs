using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.Dynamics;

namespace GameActorLogic
{
    /// <summary>
    /// 技能容器
    /// </summary>
    public interface ISkillContainer:
        IBaseContainer,
        ISkillAttributeComponent,
        ISkillEventComponent
    {
        Body GetBody();
    }


    public interface ISkillComponentContainer:
        IBaseComponentContainer
    {
        ISkillAttributeInternalComponent GetSkillAttributeInternalComponent();


        ISkillEventInternalComponent GetSkillEventInternalComponent();
    } 
}
