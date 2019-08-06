using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public interface ICommand
    {
        Int32 CommandType { get; }

    }
}
