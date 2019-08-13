using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 处理事件和指令组件
    /// </summary>
    public class HandlerComponentBase
    {
        protected ILevelActorComponentBaseContainer levelContainer;

        public HandlerComponentBase(ILevelActorComponentBaseContainer container)
        {
            levelContainer = container;
        }

        public void Update()
        {
            var commandlist = levelContainer.GetComponentBase().GetCommands();
            foreach (var command in commandlist)
            {
                
            }



        }

        protected ActorBase GetActor(ulong id)
        {
            var allactor = levelContainer.GetEnvirinfointernalBase().GetAllActors();
            foreach (var actorBase in allactor)
            {
                if (actorBase.GetActorID() == id)
                    return actorBase;
            }

            return null;
        }


        protected void HandlerCommand(ICommand command)
        {
            switch (command.CommandType)
            {
                case CommandConstDefine.CommandDefine:
                    break;
                case CommandConstDefine.ThrustCommand:
                    HandlerThrustCommand(command);
                    break;
                case CommandConstDefine.ForwardCommand:
                    break;
                case CommandConstDefine.SkillCommand:
                    break;
            }
        }

        /// <summary>
        /// 处理推进指令
        /// </summary>
        protected void HandlerThrustCommand(ICommand command)
        {
            if(!(command is ThrustCommand commanditme)) return;
            var actor = GetActor(commanditme.actorid);
            actor?.AddThrust(commanditme.Thrustproc);
        }


        protected void HandlerForwardCommand(ICommand command)
        {
            if (!(command is ForwardCommand commanditme)) return;
            var actor = GetActor(commanditme.actorid);
            actor?.;
        }
    }
}
