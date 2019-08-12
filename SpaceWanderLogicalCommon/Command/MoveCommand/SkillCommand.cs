using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    [Serializable]
    public class SkillCommand : Command
    {
        /// <summary>
        /// 0 按下
        /// 1 抬起
        /// </summary>
        public int skillcontrol;

        /// <summary>
        /// 执行技能的ActorID
        /// </summary>
        public ulong actorid;

        public SkillCommand(ulong actorid,int skillcontrol)
        {
            _commandtype = CommandConstDefine.SkillCommand;
            this.skillcontrol = skillcontrol;
            this.actorid = actorid;
        }
    }
}
