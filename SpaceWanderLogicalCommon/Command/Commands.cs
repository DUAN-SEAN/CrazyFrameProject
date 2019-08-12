using System;
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
       
        public int CommandType
        {
            get => _commandtype;
        }



    }

}
