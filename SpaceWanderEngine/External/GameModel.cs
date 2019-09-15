namespace Box2DSharp.External
{
    public enum GameModel
    {
        ModelNone,
        WaspShip,          //黄蜂 20
        FighterShipA,      //战斗机A   30
        FighterShipB,      //战斗机B   30
        DroneShip,         //无人机    10
        AnnihilationShip,  //歼灭船    10
        EliteShipA,        //精英船A   1000 护盾200
        EliteShipB,        //精英船B   600 护盾400

        BaseStation,       //基站.
        S_Meteorolite,       //小陨石
        M_Meteorolite,       //小陨石
        L_Meteorolite,       //小陨石

        MachineGun,        //机关枪 ： 半激光 不带自瞄 CD短   200ms  8
        AntiAircraftGun,   //高射炮 : 射的够快  伤害一般 带自瞄   1500ms  15
        Torpedo,           //鱼雷 ：带自瞄 CD长 高伤害 小AOE  4000ms  40
        TrackingMissile,   //跟踪导弹 ：低伤害 无限自瞄   1000ms  9
        ContinuousLaser,   //持续激光 ：射程短 带能量条 100 按下 20 每秒16 （AI敌人：多一套逻辑） 1
        PowerLaser,        //蓄力激光 ：大AOE 蓄力  CD长 高伤害  5000ms 20-60
        TimeBomb,          //定时炸弹 ：AOE 2000ms 15
        TriggerBomb        //触发炸弹 ：较大AOE CD较长 伤害较高  4000ms  20 （自损1）
    }
}
