using System;
using System.Collections.Generic;


namespace GameActorLogic
{
    /// <summary>
    /// 基础命令组件
    /// </summary>
    public class CommandComponentBase : 
        ICommandComponentBase,
        ICommandInternalComponentBase
    {
        protected List<ICommand> _commands;
        public CommandComponentBase()
        {
            _commands = new List<ICommand>();
        }

        public void Dispose()
        {
            _commands.Clear();
            _commands = null;
        }

        #region ICommandComponentBase

        public List<ICommand> GetCommands()
        {
            var list = new List<ICommand>(_commands);
            _commands.Clear();
            return list;
        }

        public void PostCommand(ICommand command)
        {
            _commands.Add(command);
        }
        #endregion


        #region ICommandInternalComponentBase
        public void TickCommand()
        {
           
        }
        #endregion





    }
}
