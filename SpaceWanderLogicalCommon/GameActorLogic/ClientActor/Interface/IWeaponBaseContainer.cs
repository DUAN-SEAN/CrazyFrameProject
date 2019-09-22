



namespace GameActorLogic
{
    /// <summary>
    /// 武器Actor对外接口
    /// </summary>
    public interface IWeaponBaseContainer : 
       
        ISkillContainer,
        IAIBase,
        IWeaponEventBase,
        IWeaponAttributeBase
    {
        
    }


    /// <summary>
    /// 武器Actor对内接口
    /// </summary>
    public interface IWeaponBaseComponentContainer : 

        IWeaponBaseContainer,
        ISkillComponentContainer
    {
        /// <summary>
        /// 获得ai组件对内接口
        /// </summary>
        IAIInternalBase GetAIinternalBase();

        /// <summary>
        /// 获取销毁组件
        /// </summary>
        IWeaponEventinternalBase GetWeaponEventinternalBase();

        /// <summary>
        /// 获取武器属性
        /// </summary>
        IWeaponAttributeInternalBase GetWeaponAttributeinternalBase();
    }


}
