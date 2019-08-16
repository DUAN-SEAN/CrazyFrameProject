using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 火控组件
    /// 对外接口发射武器
    /// </summary>
    public interface IFireControlBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void InitializeFireControl(List<Int32> containers);

        /// <summary>
        /// 外部发送按下事件
        /// </summary>
        void SendButtonState(ulong actorid, int skilltype, int skillcontrol);
       
        /// <summary>
        /// 发射武器
        /// </summary>
        /// <param name="i">全局武器类型</param>
        void Fire(int i);

        /// <summary>
        /// 关闭制定武器功能
        /// </summary>
        /// <param name="i">全局武器类型</param>
        void End(int i);

        /// <summary>
        /// 销毁武器
        /// </summary>
        /// <param name="i">全局武器类型</param>
        void Destroy(int i);

        /// <summary>
        /// 获取武器弹药
        /// </summary>
        int GetWeaponBulletNum(ulong id);

        /// <summary>
        /// 获取武器当前Cd
        /// </summary>
        int GetWeaponCd(ulong id);

        void SetWeaponBulletNum(ulong id,int num);

        void SetWeaponCd(ulong id, int cd);

    }

    /// <summary>
    /// 火控组件
    /// 对内接口
    /// 目前没有需求
    /// </summary>
    public interface IFireControlInternalBase : IFireControlBase
    {
        event Action<IWeaponBaseContainer> OnFire;

        event Action<IWeaponBaseContainer> OnEnd;

        event Action<IWeaponBaseContainer> OnDestroy;
    }



}
