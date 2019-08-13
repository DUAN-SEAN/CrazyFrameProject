using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class ActorTypeBaseDefine
    {
        public const Int32 ActorNone = 1001;

        #region 飞船定义
        /// <summary>
        /// 飞船默认定义
        /// </summary>
        public const Int32 ShipActorNone = 1002;
        /// <summary>
        /// 特殊玩家飞船默认定义
        /// </summary>
        public const Int32 PlayerShipActor = 1003;
        /// <summary>
        /// 黄蜂飞船
        /// </summary>
        public const Int32 WaspShipActorA = 1004;
        /// <summary>
        /// 战斗机A
        /// </summary>
        public const Int32 FighterShipActorA = 1005;
        /// <summary>
        /// 战斗机B
        /// </summary>
        public const Int32 FighterShipActorB = 1006;
        /// <summary>
        /// 无人机
        /// </summary>
        public const Int32 DroneShipActor = 1007;
        /// <summary>
        /// 歼灭船
        /// </summary>
        public const Int32 AnnihilationShipActor = 1008;
        /// <summary>
        /// 精英船A
        /// </summary>
        public const Int32 EliteShipActorA = 1009;
        /// <summary>
        /// 精英船B
        /// </summary>
        public const Int32 EliteShipActorB = 1010;
        #endregion

        #region 建筑定义
        /// <summary>
        /// 基站
        /// </summary>
        public const Int32 BaseStation = 1011;
        #endregion

        #region 武器定义
        /// <summary>
        /// 机关枪
        /// </summary>
        public const Int32 MachineGunActor = 1012;
        /// <summary>
        /// 高射炮
        /// </summary>
        public const Int32 AntiAircraftGunActor = 1013;
        /// <summary>
        /// 鱼雷
        /// </summary>
        public const Int32 TorpedoActor = 1014;
        /// <summary>
        /// 跟踪导弹
        /// </summary>
        public const Int32 TrackingMissileActor = 1015;
        /// <summary>
        /// 持续激光
        /// </summary>
        public const Int32 ContinuousLaserActor = 1016;
        /// <summary>
        /// 蓄力激光
        /// </summary>
        public const Int32 PowerLaserActor = 1017;
        /// <summary>
        /// 定时炸弹
        /// </summary>
        public const Int32 TimeBombActor = 1018;
        /// <summary>
        /// 触发炸弹
        /// </summary>
        public const Int32 TriggerBombActor = 1019;

        #endregion
    }


}
