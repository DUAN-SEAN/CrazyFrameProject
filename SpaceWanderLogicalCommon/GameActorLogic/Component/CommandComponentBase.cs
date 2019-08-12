﻿using System;
using System.Collections.Generic;


namespace GameActorLogic
{
    /// <summary>
    /// 基础命令组件
    /// </summary>
    public abstract class CommandComponentBase : 
        ICommandComponentBase,
        ICommandInternalComponentBase
    {
        protected List<ICommand> _commands;
        protected CommandComponentBase()
        {
            _commands = new List<ICommand>();
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
