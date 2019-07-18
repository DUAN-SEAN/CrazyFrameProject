using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine;
namespace SpaceShip.Base
{
    public class LevelData
    {
        public LevelData()
        {

            things = new List<Body>();
            //gameEvents = new List<IGameEvent>();

        }


        public void AddEntity<T>(T thing) where T : Body
        {
            things.Add(thing);
            //gameEvents.Add(thing);
        }
        /// <summary>
        /// 在游戏开始之前将地图数据添加进世界
        /// </summary>
        public void LoadingEntityToWorldBeforeBegin()
        {
            foreach (Body body in things)
            {
                World.Instanse.Bodies.Add(body);
            }
        }

        /// <summary>
        /// 在游戏结束之后将世界中的地图数据去除
        /// </summary>
        public void UnLoadingEntityFromWorldAfterEnd()
        {
            foreach (Body body in things)
            {
                World.Instanse.Bodies.Remove(body);
            }
        }
        public int id;
        public int level;
        public string Name;
        public int MemberCount;
        //物理引擎物体描述
        public List<Body> things;
        //场景事件描述
        //public List<IGameEvent> gameEvents;



    }
}
