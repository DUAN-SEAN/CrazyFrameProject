using System;


namespace CrazyEngine
{
    public class MoveSystem : ITickable
    {
        public World world;

        public MoveSystem(World world)
        {
            this.world = world;
        }


        private void Move()
        {
            foreach (Body body in world.Bodies)
            {
                if (body.Enable)
                {
                    body.Position += body.Velocity / 100;

                    if (!body.Static && body.MaxVelocity - body.Velocity.magnitude > float.Epsilon)
                    {
                        body.Velocity += body.Acceleration;
                    }

                    if (body.MaxVelocity - body.Velocity.magnitude < float.Epsilon)
                    {
                        Vector2 newVelocity = new Vector2(body.MaxVelocity * body.Forward.CosNoSqrt, body.MaxVelocity * body.Forward.SinNoSqrt);
                        body.Velocity = Vector2.Lerp(body.Velocity, newVelocity, 1f);
                    }
                }

                body.ClearForce();
            }
        }

        public void Tick()
        {
            Move();
        }


        public void Dispose()
        {
            world = null;
        }
    }
}