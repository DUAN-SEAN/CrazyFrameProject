

namespace GameActorLogic
{
    /// <summary>
    /// 武器Actor对外接口
    /// </summary>
    public interface IWeaponBaseContainer : 
        IBaseContainer,
        IAIBase
    {

    }


    /// <summary>
    /// 武器Actor对内接口
    /// </summary>
    public interface IWeaponBaseComponentContainer : 
        IBaseComponentContainer,
        IWeaponBaseContainer
    {
        /// <summary>
        /// 获得ai组件对内接口
        /// </summary>
        IAIinternalBase GetAIinternalBase();
    }


}
