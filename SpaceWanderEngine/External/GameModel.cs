using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Vector2 = System.Numerics.Vector2;

namespace Box2DSharp.External
{
    public enum GameModel
    {
        ModelNone,         //默认空
        WaspShip,          //黄蜂
        FighterShipA,      //战斗机A
        FighterShipB,      //战斗机B
        DroneShip,         //无人机
        AnnihilationShip,  //歼灭船
        EliteShipA,        //精英船A
        EliteShipB,        //精英船B

        BaseStation,       //基站

        MachineGun,        //机关枪
        AntiAircraftGun,   //高射炮
        Torpedo,           //鱼雷
        TrackingMissile,   //跟踪导弹
        ContinuousLaser,   //持续激光
        PowerLaser,        //蓄力激光
        TimeBomb,          //定时炸弹
        TriggerBomb        //出发炸弹
    }
}
