using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class EventMessageConstDefine
    {
        public const Int32 BattleEventNone = 1001;
        #region Actor操作
        public const Int32 InitEvent = 1002;
        public const Int32 DestroyEvent = 1003;
        public const Int32 UpdateEvent = 1004;
        #endregion

        #region 任务更新
        public const Int32 TaskUpdateEvent = 1005;
        #endregion

        #region 关卡事件
        public const Int32 VictoryEvent = 1006;
        public const Int32 FailEvent = 1007;

        #endregion


    }
}
