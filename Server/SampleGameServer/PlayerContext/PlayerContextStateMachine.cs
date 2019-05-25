using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.ServerBase;
namespace GameServer
{
    public class PlayerContextStateMachine:StateMachine
    {
        public PlayerContextStateMachine()
        {
            State = StateIdle;

        }
        public override int SetStateCheck(int commingEvent, int newState = -1)
        {
            Int32 returnState = -1;
            switch (State)
            {

                default:break;
            }

            if (newState != -1 && returnState != newState)
            {
                return -1;
            }
            return returnState;
        }
        #region 状态定义
        /// <summary>
        /// 空闲状态
        /// </summary>
        public const Int32 StateIdle = 0;
        /// <summary>
        /// 完成连接状态
        /// </summary>
        public const Int32 StateConnected = 1;
        /// <summary>
        /// AuthTokenTest验证成功状态
        /// </summary>
        public const Int32 StateAuthLoginOK = 2;
        /// <summary>
        /// SessionToken验证成功状态
        /// </summary>
        public const Int32 StateSessionLoginOK = 4;
        /// <summary>
        /// 正在断开连接状态
        /// </summary>
        public const Int32 StateDisconnecting = 5;
        /// <summary>
        /// 已经断开连接状态
        /// </summary>
        public const Int32 StateDisconnected = 6;
        /// <summary>
        /// 断线后等待重连状态
        /// </summary>
        public const Int32 StateDisconnectedWaitForReconnect = 7;
        /// <summary>
        /// 玩家已登录游戏
        /// </summary>
        public const Int32 StateEnteredGame = 8;

        #endregion

        #region 事件定义
        /// <summary>
        /// 连接完成事件
        /// </summary>
        public const Int32 EventOnConnected = 1;

        #endregion
    }
}
