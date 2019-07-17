using System;
public class PointEntity : Body,IGameEvent
{
    public PointEntity()
    {
        Collider = new Collider(Vector2.Zero);
        Position = Vector2.Zero;
    }
    public PointEntity(Vector2 vector)
    {
        Collider = new Collider(vector);
        Position = vector;
    }

    public bool DoSomething()
    {
        return true;
    }

    public bool GetEventTrigger()
    {
        return GameState.playing == GameSceneLogic.Instance.GameState;
    }

   
}
