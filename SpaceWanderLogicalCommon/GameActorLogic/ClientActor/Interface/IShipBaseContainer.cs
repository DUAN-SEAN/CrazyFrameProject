

namespace GameActorLogic
{
    /// <summary>
    /// 船Actor对外接口
    /// </summary>
    public interface IShipBaseContainer : IBaseContainer,
        IFireControlBase,
        IHealthShieldBase,
        IShipEventBase
    {

    }


    /// <summary>
    /// 船对内接口
    /// </summary>
    public interface IShipComponentBaseContainer : 
        IBaseComponentContainer
        ,IShipBaseContainer
    {
        /// <summary>
        /// 返回火控组件
        /// </summary>
        IFireControlInternalBase GetFireControlinternalBase(); 

        /// <summary>
        /// 获取血量护盾组件
        /// </summary>
        /// <returns></returns>
        IHealthShieldinternalBase GetHealthShieldinternalBase();

        /// <summary>
        /// 获取船事件组件
        /// </summary>
        /// <returns></returns>
        IShipEventinternalBase GetShipEventinternalBase();
    }
}
