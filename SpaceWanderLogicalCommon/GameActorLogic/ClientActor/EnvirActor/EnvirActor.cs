using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 环境Actor精简对象
    /// </summary>
    public class EnvirActor : ActorBase, IEnvirBaseContainer, IEnvirBaseComponentContainer
    {
        public EnvirActor(ulong id, int actortype, ILevelActorComponentBaseContainer level) : base(id, actortype, level)
        {
            CreateBaseComponent();
        }


        public override ActorBase Clone()
        {
            var clone = this.MemberwiseClone() as EnvirActor;
            clone._invariantAttributeComponent = new InvariantAttributeComponentBase(clone._invariantAttributeComponent);
            clone._physicalBase = new PhysicalBase(clone._physicalBase);
            clone._moveComponent = new MoveComponentBase(clone._physicalBase);
            return clone;
        }
    }
}
