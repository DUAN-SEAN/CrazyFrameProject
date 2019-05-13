using System;
using System.Timers;

public class Engine
{
    public int UpdateInterval = 20;

    private MoveSystem _moveSystem = new MoveSystem();
    private CollisionSystem _collisionSystem = new CollisionSystem();

    public Engine()
    {
        TimerManager.Instanse.doLoop(UpdateInterval, Update);
    }

    private void Update()
    {
        _moveSystem.Tick();

        _collisionSystem.Tick();
    }

}
