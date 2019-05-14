using System;


public class MoveSystem :  ITickable
{
    private void Move()
    {
        foreach (Body body in World.Instanse.Bodies)
        {
            if (body.Static) continue;

            body.Position += body.Velocity;

            body.Velocity += body.Acceleration;

            body.ClearForce();
        }
    }

    public void Tick()
    {
        Move();
    }
}
