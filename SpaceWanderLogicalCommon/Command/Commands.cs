using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic.Command
{
    [Serializable]
    public class MoveCommand:ICommand
    {
        public int CommandType
        {
            get => CommandConstDefine.MoveCommand;
        }



    }

}
