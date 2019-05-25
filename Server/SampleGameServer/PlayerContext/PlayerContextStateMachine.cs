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
                case StateIdle:
                    {
                        switch (commingEvent)
                        {
                            case EventOnConnected:
                                returnState = StateConnected;
                                break;
                            case EventDisconnect:
                                returnState = StateDisconnecting;
                                break;
                            case EventOnDisconnected:
                                returnState = StateDisconnected;
                                break;
                            default:
                                return -1;
                        }
                    }
                    break;
                case StateConnected:
                    {
                        switch (commingEvent)
                        {
                            case EventOnDisconnected:
                                returnState = StateDisconnected;
                                break;
                            case EventDisconnect:
                                returnState = StateDisconnecting;
                                break;
                            case EventOnAuthLoginOK:
                                returnState = StateAuthLoginOK;
                                break;
                            case EventOnAuthLoginFail:
                                returnState = State;
                                break;
                            case EventOnSessionLoginReq:
                                returnState = State;
                                break;
                            case EventOnSessionLoginOK:
                                returnState = StateSessionLoginOK;
                                break;
                            case EventOnReCLoginFail:
                                returnState = State;
                                break;
                            default:
                                return -1;
                        }
                    }
                    break;
                case StateAuthLoginOK:
                    {
                        switch (commingEvent)
                        {
                            case EventOnDisconnected:
                                returnState = StateDisconnected;
                                break;
                            case EventDisconnect:
                                returnState = StateDisconnecting;
                                break;
                            case EventOnSessionLoginOK:
                                returnState = StateSessionLoginOK;
                                break;
                            default:
                                return -1;
                        }
                    }
                    break;
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
        /// <summary>
        /// 服务器主动断开连接事件  服务器主动调用断开连接
        /// </summary>
        public const Int32 EventDisconnect = 2;
        /// <summary>
        /// 连接断开事件
        /// </summary>
        public const Int32 EventOnDisconnected = 3;
        /// <summary>
        /// AuthTokenTest验证请求事件
        /// </summary>
        public const Int32 EventOnAuthLoginReq = 4;
        /// <summary>
        /// AuthTokenTest验证成功事件
        /// </summary>
        public const Int32 EventOnAuthLoginOK = 5;
        /// <summary>
        /// AuthTokenTest验证失败事件
        /// </summary>
        public const Int32 EventOnAuthLoginFail = 6;
        /// <summary>
        /// SessionToken验证请求事件
        /// </summary>
        public const Int32 EventOnSessionLoginReq = 7;
        /// <summary>
        /// SessionToken验证成功事件
        /// </summary>
        public const Int32 EventOnSessionLoginOK = 8;
        /// <summary>
        /// SessionToken验证失败事件
        /// </summary>
        public const Int32 EventOnReCLoginFail = 9;
        /// <summary>
        /// 向中心服务器发送现场注册事件
        /// </summary>
        public const Int32 EventOnCenterPlayerContextRegisterReqSend = 10;
        /// <summary>
        /// 中心服务器发来现场注册成功事件
        /// </summary>
        public const Int32 EventOnCenterPlayerContextRegisterAckOK = 11;
        /// <summary>
        /// 中心服务器发来现场注册失败事件
        /// </summary>
        public const Int32 EventOnCenterPlayerContextRegisterAckFail = 12;
        /// <summary>
        /// 现场恢复事件
        /// </summary>
        public const Int32 EventOnConextTransformOK = 13;
        /// <summary>
        /// 认证完成之后的游戏逻辑操作
        /// </summary>
        public const Int32 EventOnGameLogicOPTAfterLogin = 14;
        /// <summary>
        /// 登录游戏完成之后的游戏逻辑操作
        /// </summary>
        public const Int32 EventOnEnteredGameLogicOPT = 15;
        #endregion
    }
}
