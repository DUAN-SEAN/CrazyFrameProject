﻿using Crazy.Common;
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
            //if (_commands == null || _commands.Count == 0)
            //    Log.Trace("CommandComponent: commands 数量" + _commands.Count);
            List<ICommand> list;
            lock (_commands)
            {
                list = new List<ICommand>(_commands);
                _commands.Clear();
            }
            //list = new List<ICommand>(_commands);
            //_commands.Clear();
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
