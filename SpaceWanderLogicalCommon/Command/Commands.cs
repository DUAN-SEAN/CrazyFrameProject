﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    [Serializable]
    public class Command:ICommand
    {
        protected int _commandtype = CommandConstDefine.CommandDefine;
        public DateTime currenttime;
        public int CommandType
        {
            get => _commandtype;
        }

        public virtual string tostring()
        {
            return _commandtype +"";
        }
    }

}
