using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
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
        public long actorid;

        public SkillCommand(long actorid,int skillcontrol)
        {
            _commandtype = CommandConstDefine.SkillCommand;
            this.skillcontrol = skillcontrol;
            this.actorid = actorid;
        }
    }
}
