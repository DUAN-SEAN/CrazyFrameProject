using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameActorLogic
{
    public interface ICommandComponentBase
    {
        /// <summary>
        /// 指令集合
        /// </summary>
        List<ICommand> Commands { get; }

        /// <summary>
        /// 向组件添加新的指令
        /// </summary>
        /// <param name="command"></param>
        void PostCommand(ICommand command);




    }

    public interface ICommandInternalComponentBase:ICommandComponentBase
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        void TickCommand();
    }
}
