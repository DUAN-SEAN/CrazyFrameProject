using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 任务更新消息
    /// </summary>
    [Serializable]
    public class TaskUpdateEventMessage : BattleEventMessageBase
    {
        public int eventid;
        public TaskEventState state;
        public int key;

        /// <summary>
        /// 确定是不是引用类型
        /// </summary>
        public bool IsObejct;
        public Object value;
        /// <summary>
        /// 确定是不是int值
        /// </summary>
        public bool isInt;
        public int value_int;
        /// <summary>
        /// 确定是不是布尔值
        /// </summary>
        public bool isBool;
        public bool value_bool;

        public TaskUpdateEventMessage(int eventid, TaskEventState state)
        {
            this.eventid = eventid;
            this.state = state;
            IsObejct = false;
            isInt = false;
            isBool = false;
        }

        public TaskUpdateEventMessage(int eventid,TaskEventState state,int key,Object value)
        {
            this.eventid = eventid;
            this.state = state;
            this.key = key;
            this.value = value;
            IsObejct = true;
            isInt = false;
            isBool = false;
        }

        public TaskUpdateEventMessage(int eventid, TaskEventState state, int key, int value)
        {
            this.eventid = eventid;
            this.state = state;
            this.key = key;
            value_int = value;
            IsObejct = false;
            isInt = true;
            isBool = false;
        }

        public TaskUpdateEventMessage(int eventid, TaskEventState state, int key, bool value)
        {
            this.eventid = eventid;
            this.state = state;
            this.key = key;
            value_bool = value;
            IsObejct = false;
            isInt = false;
            isBool = true;
        }
    }
}
