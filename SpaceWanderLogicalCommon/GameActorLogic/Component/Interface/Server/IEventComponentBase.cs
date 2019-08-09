﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameActorLogic
{
    /// <summary>
    /// 用于处理事件的外部接口
    /// </summary>
    public interface IEventComponentBase
    {
        /// <summary>
        /// 添加要处理的集合
        /// </summary>
        void AddHandleEventMessage(IEventMessage msg);
        /// <summary>
        /// 获取需要处理的事件，获取完后需要清空集合
        /// 在服务器上由服务器转发到客户端对应的相同组件上，在客户端上由相关组件获取。
        /// </summary>
        List<IEventMessage> GetForWardEventMessages();


    }
    /// <summary>
    /// 事件处理组件内部接口
    /// </summary>
    public interface IEventInternalComponentBase : IEventComponentBase
    {
        // <summary>
        // 执行需要处理的事件消息
        // </summary>
        //void TickHandleEvent();

        /// <summary>
        /// 向转发事件集合中添加要转发的消息
        /// </summary>
        void AddForWardEventMessages(IEventMessage msg);

        /// <summary>
        /// 获取从上层发来的事件待处理集合
        /// </summary>
        /// <returns></returns>
        List<IEventMessage> GetHandleEventMessages();



    }
}