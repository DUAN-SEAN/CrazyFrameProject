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
        /// 发射武器
        /// </summary>
        /// <param name="i">武器槽</param>
        void Fire(int i);

        /// <summary>
        /// 关闭制定武器功能
        /// </summary>
        /// <param name="i">武器槽</param>
        void End(int i);

        /// <summary>
        /// 销毁武器
        /// </summary>
        /// <param name="i"></param>
        void Destroy(int i);


    }

    /// <summary>
    /// 火控组件
    /// 对内接口
    /// 目前没有需求
    /// </summary>
    public interface IFireControlinternalBase : IFireControlBase
    {
        event Action<int> OnFire;

        event Action<int> OnEnd;

        event Action<int> OnDestroy;
    }



}
