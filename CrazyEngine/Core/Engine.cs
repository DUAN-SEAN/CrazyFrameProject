using System;
using System.Timers;


namespace CrazyEngine
{
    public class Engine : ITickable
    {

        public int UpdateInterval = 20;
        public bool isStop;
        private readonly MoveSystem _moveSystem = new MoveSystem();
        private readonly CollisionSystem _collisionSystem = new CollisionSystem();


        public Engine()
        {
            TimerManager.Instanse.doLoop(UpdateInterval, Tick);
        }

        public void Tick()
        {

            //if (World.Instanse.isWorldStop) return;

            _moveSystem.Tick();

            _collisionSystem.Tick();
        }



    }
}