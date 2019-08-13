using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 基础事件组件
    /// </summary>
    public abstract class EventComponentBase : IEventComponentBase,
        IEventInternalComponentBase
    {
        /// <summary>
        /// 外部命令转发集合
        /// </summary>
        protected List<IEventMessage> _handleeventmessages;

        /// <summary>
        /// 内部命令转发集合
        /// </summary>
        protected List<IEventMessage> _forwardeventmessages;

        protected EventComponentBase()
        {
            _handleeventmessages = new List<IEventMessage>();
            _forwardeventmessages = new List<IEventMessage>();
        }

       


        #region 对外接口
        public void AddHandleEventMessage(IEventMessage msg)
        {
            _handleeventmessages.Add(msg);
        }

        /// <summary>
        /// 获取一次清空一次集合
        /// </summary>
        public List<IEventMessage> GetForWardEventMessages()
        {
            var list = new List<IEventMessage>(_forwardeventmessages);
            _forwardeventmessages.Clear();
            return list;
        }
        #endregion


        #region 对内接口
        public void AddForWardEventMessages(IEventMessage msg)
        {
            _forwardeventmessages.Add(msg);
        }

        public List<IEventMessage> GetHandleEventMessages()
        {
            var list = new List<IEventMessage>(_handleeventmessages);
            _handleeventmessages.Clear();
            return _handleeventmessages;
        }
        #endregion


    }
}
