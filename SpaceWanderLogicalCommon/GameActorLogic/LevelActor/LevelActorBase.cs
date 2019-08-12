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

        public void Start()
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

        #region 关卡世界环境组件
        public void AddToEngine(ObjBase obj)
        {
            _envirinfoComponent.AddToEngine(obj);
        }

        public void RemoveFromEngine(ObjBase obj)
        {
            _envirinfoComponent.RemoveFromEngine(obj);
        }

        #endregion


        #endregion
        
        IEventComponentBase ILevelActorComponentBaseContainer.GetEventComponentBase()
        {
            return _eventComponent;
        }

        IEnvirinfointernalBase ILevelActorComponentBaseContainer.GetEnvirinfointernalBase()
        {
            return _envirinfoComponent;
        }
    }
}
