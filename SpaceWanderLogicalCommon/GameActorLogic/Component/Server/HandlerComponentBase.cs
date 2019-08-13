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
                HandlerCommand(command);
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
                    HandlerForwardCommand(command);
                    break;
                case CommandConstDefine.SkillCommand:
                    HandlerSkillCommand(command);
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
            if (commanditme.ang > 0)
                actor?.Left(commanditme.ang);
            else
                actor?.Right(commanditme.ang);
        }

        protected void HandlerSkillCommand(ICommand command)
        {
            if (!(command is SkillCommand commanditme)) return;
            if(!(GetActor(commanditme.actorid) is ShipActorBase ship)) return;
            switch (commanditme.skillcontrol)
            {
                case 0:
                    ship.Fire(commanditme.skilltype);
                    break;
                case 1:
                    ship.End(commanditme.skilltype);
                    break;
            }

        }
    }
}
