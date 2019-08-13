using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Base;

namespace GameActorLogic
{
    public class LevelActorBase:
        ILevelActorBaseContainer,
        ILevelActorComponentBaseContainer
    {
        protected EventComponentBase _eventComponent;
        protected EnvirinfoComponentBase _envirinfoComponent;
        protected CommandComponentBase _commandComponent;
        protected 
        protected long levelid;
        protected ulong battleid;

        protected bool isStart = false;

        #region ILevelActorBaseContainer

        public long GetLevelID()
        {
            return levelid;
        }

        public ulong GetBattleID()
        {
            return battleid;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="players">玩家集合</param>
        /// <param name="barrierId">关卡类型id</param>
        public void Start(List<string> players,int barrierId)
        {
            isStart = true;
            //TODO 可能进行动态初始化



        }

        public void Update()
        {
            if(isStart == false) return;

            //TODO 添加一个处理事件集合组件

            

        }

        public void Dispose()
        {
            isStart = false;
            //TODO 对子对象进行动态Dispose

        }

        #region 关卡环境

        public List<ActorBase> GetAllActors()
        {
            return _envirinfoComponent.GetAllActors();
        }

        #endregion


        #region 关卡事件组件
        public void AddHandleEventMessage(IEventMessage msg)
        {
            _eventComponent.AddHandleEventMessage(msg);
        }

        public List<IEventMessage> GetForWardEventMessages()
        {
            return _eventComponent.GetForWardEventMessages();
        }
        #endregion

    

        #region 指令集合组件

        public List<ICommand> GetCommands()
        {
            return _commandComponent.GetCommands();
        }
        public void PostCommand(ICommand command)
        {
            _commandComponent.PostCommand(command);
        }


        #endregion

        #endregion

        ICommandInternalComponentBase ILevelActorComponentBaseContainer.GetComponentBase()
        {
            return _commandComponent;
        }
        IEventInternalComponentBase ILevelActorComponentBaseContainer.GetEventComponentBase()
        {
            return _eventComponent;
        }

        IEnvirinfoInternalBase ILevelActorComponentBaseContainer.GetEnvirinfointernalBase()
        {
            return _envirinfoComponent;
        }


       
    }
}
