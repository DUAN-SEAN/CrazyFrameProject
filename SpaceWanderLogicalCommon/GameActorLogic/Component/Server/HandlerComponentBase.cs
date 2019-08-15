using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceWanderLogicalCommon.Event;

namespace GameActorLogic
{
    /// <summary>
    /// 处理事件和指令组件
    /// </summary>
    public class HandlerComponentBase : IHandlerComponentBase,
        IHandlerComponentInternalBase
    {
        protected ILevelActorComponentBaseContainer levelContainer;

        public HandlerComponentBase(ILevelActorComponentBaseContainer container)
        {
            levelContainer = container;
        }

        public void Update()
        {
            var commandlist = levelContainer.GetCommandComponentBase().GetCommands();
            foreach (var command in commandlist)
            {
                HandlerCommand(command);
            }

            var eventlist = levelContainer.GetEventComponentBase().GetHandleEventMessages();
            foreach (var eventMessage in eventlist)
            {
                HandlerEvent(eventMessage);
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

        #region 处理事件

        protected void HandlerEvent(IEventMessage eventMessage)
        {
            switch (eventMessage.MessageId)
            {
                case EventMessageConstDefine.BattleEventNone:
                    break;
                case EventMessageConstDefine.InitEvent:
                    HandlerInitEvent(eventMessage as InitEventMessage);
                    break;
                case EventMessageConstDefine.UpdateEvent:
                    break;
                case EventMessageConstDefine.DestroyEvent:
                    HandlerDestroyEvent(eventMessage as DestroyEventMessage);
                    break;
                case EventMessageConstDefine.TaskUpdateEvent:
                    HandlerTaskUpdateEvent(eventMessage as TaskUpdateEventMessage);
                    break;
                    

            }
        }

        /// <summary>
        /// 处理生成事件
        /// </summary>
        protected void HandlerInitEvent(InitEventMessage initEvent)
        {
            if (initEvent == null) return;
            ActorBase actor = null;
            if (initEvent.haveId)
            {
                actor = levelContainer.GetCreateInternalComponentBase().CreateActor(initEvent.actortype,initEvent.camp,
                    initEvent.point_x,
                    initEvent.point_y, initEvent.angle, initEvent.actorid);
            }
            else
            {
                actor = levelContainer.GetCreateInternalComponentBase().CreateActor(initEvent.actortype, initEvent.camp,
                    initEvent.point_x,
                    initEvent.point_y, initEvent.angle);
            }

            if (actor != null)
            {
                levelContainer.GetEnvirinfointernalBase().AddActor(actor);
            }
            //执行回调生成事件
            OnInitMessageHandler?.Invoke(initEvent.actorid);

        }

        /// <summary>
        /// 处理销毁事件
        /// </summary>
        protected void HandlerDestroyEvent(DestroyEventMessage destroyEvent)
        {
            if(destroyEvent == null) return;
            //先执行回调销毁事件
            OnDestroyMessageHandler?.Invoke(destroyEvent.actorid);
            ActorBase actor = null;
            foreach (var actorBase in levelContainer.GetEnvirinfointernalBase().GetAllActors())
            {
                if (actorBase.GetActorID() == destroyEvent.actorid)
                    actor = actorBase;
            }
            levelContainer.GetEnvirinfointernalBase().RemoveActor(actor);


        }

        /// <summary>
        /// 处理任务更新事件/
        /// </summary>
        protected void HandlerTaskUpdateEvent(TaskUpdateEventMessage taskUpdateEvent)
        {
            List<ITaskEvent> events = levelContainer.GeTaskEventComponentInternalBase().GetAllTaskEvents();

            ITaskEvent eventitme = null;
            foreach (var taskEvent in events)
            {
                if (taskEvent.GetTaskId() == taskUpdateEvent.eventid)
                {
                    eventitme = taskEvent;
                }
            }
            if(eventitme == null) return;

            if (taskUpdateEvent.IsObejct)
            {
                eventitme.SetValue(taskUpdateEvent.key,taskUpdateEvent.value);
            }

            if (taskUpdateEvent.isBool)
            {
                eventitme.SetValue(taskUpdateEvent.key,taskUpdateEvent.value_bool);
            }

            if (taskUpdateEvent.isInt)
            {
                eventitme.SetValue(taskUpdateEvent.key, taskUpdateEvent.value_int);
            }

            OnTaskUpdateMessageHandler?.Invoke(taskUpdateEvent.eventid);

        }

        #endregion

        #region 处理指令
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
            if (!(command is ThrustCommand commanditme)) return;
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
            if (!(GetActor(commanditme.actorid) is ShipActorBase ship)) return;
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


        #endregion

        #region 回调事件
        public event Action<ulong> OnInitMessageHandler;
        public event Action<ulong> OnDestroyMessageHandler;
        public event Action<ulong> OnTaskUpdateMessageHandler;

        #endregion

    }
}
