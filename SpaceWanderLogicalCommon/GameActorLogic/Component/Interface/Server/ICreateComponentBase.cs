using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public interface ICreateComponentBase
    {

    }

    public interface ICreateInternalComponentBase : ICreateComponentBase
    {
        ActorBase CreateActor(int actortype,int camp, float point_x, float point_y, float angle, bool isPlayer = false, Int32 weapontype_a = 0, Int32 weapontype_b = 0,string name= "");
        ActorBase CreateActor(int actortype,int camp, float point_x, float point_y, float angle,ulong actorid, bool isPlayer = false, Int32 weapontype_a = 0, Int32 weapontype_b = 0,string name = "");

        ITaskEvent CreateTaskEvent(int taskcondition,int taskresult, int taskid, Dictionary<int, int> taskconditions,string taskdes);


        ulong GetCreateID();
    }
}
