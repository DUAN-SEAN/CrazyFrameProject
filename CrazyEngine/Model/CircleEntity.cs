using System;
public class CircleEntity : Body, IGameEvent
{
    public float radius;
    public CircleEntity()
    {
        Collider = new Collider(Vector2.Zero, 0);
        Position = Vector2.Zero;
    }
    public CircleEntity(Vector2 vector, float radius)
    {
        Collider = new Collider(vector, radius);
        this.radius = radius;
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
