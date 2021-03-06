﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crazy.Common;


namespace GameActorLogic
{
    /// <summary>
    /// 处理事件和指令组件
    /// </summary>
    public class HandlerComponentBase : IHandlerComponentBase,
        IHandlerComponentInternalBase
    {
        protected ILevelActorComponentBaseContainer levelContainer;

        protected int initMessageNum;
        protected int DestoryMessageNum;

        protected long lastNumframe;
        protected long Numdely = 10000000;




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

            TickNum();

        }



        public void Dispose()
        {
            levelContainer = null;

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

        protected void TickNum()
        {



            if(DateTime.Now.Ticks - lastNumframe > Numdely)
            {
                //if (initMessageNum != 0 || DestoryMessageNum != 0)
                //    Log.Debug("TickNum 一秒钟消息：生成消息数" + initMessageNum + " 销毁消息数：" + DestoryMessageNum);
                initMessageNum = 0;
                DestoryMessageNum = 0;

                lastNumframe = DateTime.Now.Ticks;
            }
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
                case EventMessageConstDefine.VictoryEvent:
                    OnGameVictory?.Invoke();
                    break;
                case EventMessageConstDefine.FailEvent:
                    OnGameFail?.Invoke();
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
                actor = levelContainer.GetCreateInternalComponentBase().CreateActor(initEvent.actortype, initEvent.camp,
                    initEvent.point_x,
                    initEvent.point_y, initEvent.angle, initEvent.actorid, initEvent.IsPlayer, initEvent.weapontype_a,
                    initEvent.weapontype_b,initEvent.name,initEvent.time);
            }
            else
            {
                actor = levelContainer.GetCreateInternalComponentBase().CreateActor(initEvent.actortype, initEvent.camp,
                    initEvent.point_x,
                    initEvent.point_y, initEvent.angle, initEvent.IsPlayer, initEvent.weapontype_a,
                    initEvent.weapontype_b,initEvent.name, initEvent.time);
                initEvent.actorid = actor.GetActorID();
            }
            if(initEvent.time != 0)
            {
                actor.SetActorInitPro(initEvent.time);
            }

            if (actor != null)
            {
                if (actor is ISkillContainer skill)
                {
                    skill.SetOwnerID(initEvent.onwerid);
                }

                actor.SetRelPosition(initEvent.relatpoint_x, initEvent.relatpoint_y);
                actor.SetLinerDamping(initEvent.LinerDamping);
                levelContainer.GetEnvirinfointernalBase().AddActor(actor);
            }
            Log.Trace("HandlerComponentBase HandlerInitEvent: 生成一个Actor id" + actor.GetActorID() + " " + actor.GetActorType());
            //执行回调生成事件
            OnInitMessageHandler?.Invoke(initEvent.actorid);
            initMessageNum++;
        }

        /// <summary>
        /// 处理销毁事件
        /// </summary>
        protected void HandlerDestroyEvent(DestroyEventMessage destroyEvent)
        {
            if(destroyEvent == null) return;
            //Log.Trace("处理销毁事件" + destroyEvent.actorid);

            //先执行回调销毁事件
            OnDestroyMessageHandler?.Invoke(destroyEvent.actorid);
            ActorBase actor = null;
            if (levelContainer == null) return;
            if (levelContainer.GetEnvirinfointernalBase() == null) return;
            if (levelContainer.GetEnvirinfointernalBase().GetAllActors() == null) return;
            foreach (var actorBase in levelContainer.GetEnvirinfointernalBase().GetAllActors())
            {
                if (actorBase.GetActorID() == destroyEvent.actorid)
                    actor = actorBase;
            }
            if(actor == null) return;
            if(actor.IsPlayer() && actor.IsShip())
            {
                string i = null;
                var players = levelContainer.GetPlayerDict();
                foreach (var p in players)
                {
                    if(p.Value == actor.GetActorID())
                    {
                        i = p.Key;
                    }
                }
                if (i != null)
                players.Remove(i);
                if (players.Count == 0) levelContainer.AddEventMessagesToHandlerForward(new FailEventMessage(levelContainer.GetLevelID()));
            }
            levelContainer.GetEnvirinfointernalBase().RemoveActor(actor);
            actor.Dispose();


            DestoryMessageNum++;
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
           // Log.Trace("指令处理时间："+DateTime.Now);
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
                case CommandConstDefine.RemoteCommand:
                    HandlerRemoteCommand(command);
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
            if(actor.IsWeapon()) return;
            actor?.AddThrust(commanditme.Thrustproc);
        }


        protected void HandlerForwardCommand(ICommand command)
        {
            if (!(command is ForwardCommand commanditme)) return;
            var actor = GetActor(commanditme.actorid);
            if(actor.IsWeapon()) return;
            if (commanditme.ang > 0)
                actor?.Left(commanditme.ang);
            else
                actor?.Right(commanditme.ang);
        }

        protected void HandlerSkillCommand(ICommand command)
        {
            if (!(command is SkillCommand commanditme)) return;
            if (!(GetActor(commanditme.actorid) is ShipActorBase ship)) return;
            //Log.Debug("HandlerSkillCommand: 处理了一个Skill指令" + commanditme.actorid + " " + commanditme.skilltype + " " + commanditme.skillcontrol);
            ship.SendButtonState(commanditme.actorid,commanditme.skilltype,commanditme.skillcontrol);
            //ship.SendButtonState(commanditme.skillcontrol);
            //switch (commanditme.skillcontrol)
            //{
            //    case 0:
            //        ship.Fire(commanditme.skilltype);
            //        break;
            //    case 1:
            //        ship.End(commanditme.skilltype);
            //        break;
            //}

        }

        protected void HandlerRemoteCommand(ICommand command)
        {
            if(!(command is RemoteCommand commanditme)) return;
            if(!(GetActor(commanditme.actorid) is ShipActorBase ship)) return;
            ship.Remote(commanditme.remote_x, commanditme.remote_y);
        }


        #endregion

        #region 回调事件
        public event Action<ulong> OnInitMessageHandler;
        public event Action<ulong> OnDestroyMessageHandler;
        public event Action<int> OnTaskUpdateMessageHandler;
        public event Action OnGameVictory;
        public event Action OnGameFail;

        #endregion

    }
}
