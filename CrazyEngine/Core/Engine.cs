using System;
using System.Timers;


namespace CrazyEngine
{
    public class Engine : ITickable
    {

        public int UpdateInterval = 20;
        public bool isStop;
        private MoveSystem _moveSystem;
        private CollisionSystem _collisionSystem;


        public Engine(World world)
        {
            //TimerManager.Instanse.doLoop(UpdateInterval, Tick);
            _moveSystem = new MoveSystem(world);
            _collisionSystem = new CollisionSystem(world);
        }

        public void Tick()
        {

            //if (World.Instanse.isWorldStop) return;

            _moveSystem.Tick();

            _collisionSystem.Tick();
        }

        public void Dispose()
        {
            _moveSystem.Dispose();
            _collisionSystem.Dispose();

            _moveSystem = null;
            _collisionSystem = null;
            
        }

    }
}