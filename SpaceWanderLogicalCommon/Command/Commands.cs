using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    [Serializable]
    public class MoveCommand:ICommand
    {
        protected int _commandtype = CommandConstDefine.MoveCommand;
       
        public int CommandType
        {
            get => _commandtype;
        }



    }

}
