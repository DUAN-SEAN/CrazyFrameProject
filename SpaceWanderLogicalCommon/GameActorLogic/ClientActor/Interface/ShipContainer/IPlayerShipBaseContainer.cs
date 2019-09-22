

namespace GameActorLogic
{
    /// <summary>
    /// 玩家船Actor对外接口
    /// </summary>
    public interface IPlayerShipBaseContainer : IShipBaseContainer
    {

    }


    /// <summary>
    /// 玩家船Actor 对内接口
    /// </summary>
    public interface IPlayerShipComponentBaseContainer :
        IPlayerShipBaseContainer
        ,IShipComponentBaseContainer
    {

    }


}
