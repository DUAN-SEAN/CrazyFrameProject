using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class CommandConstDefine
    {
        /// <summary>
        /// 移动控制command
        /// </summary>
        public const Int32 MoveCommand = 1001;

        /// <summary>
        /// 左指令
        /// </summary>
        public const Int32 LeftCommand = 1002;
        /// <summary>
        /// 右指令
        /// </summary>
        public const Int32 RightCommand = 1003;
        /// <summary>
        /// 前推指令
        /// </summary>
        public const Int32 ThrustCommand = 1004;
    }
}
