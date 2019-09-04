using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public interface ITaskResult
    {
        void Execute();

        void Dispose();

        void StartResult();
    }
}
