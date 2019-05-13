using System;
public class CollisionSystem : ITickable
{

    private void CollissionCheck()
    {
        for(int i=0; i<World.Instanse.Bodies.Count; i++)
        {
            for(int j=i+1; j<World.Instanse.Bodies.Count; j++)
            {
                bool flag = false;

                ColliderType ct1 = World.Instanse.Bodies[i].ColliderType;
                ColliderType ct2 = World.Instanse.Bodies[j].ColliderType;
                switch (ct1)
                {
                    case ColliderType.Point:
                        {
                            switch (ct2)
                            {
                                case ColliderType.Point:
                                    {
                                        flag = Collision.isCollision((Point)World.Instanse.Bodies[i].Collider, (Point)World.Instanse.Bodies[j].Collider);
                                        break;
                                    }
                                case ColliderType.Circle:
                                    {
                                        flag = Collision.isCollision((Point)World.Instanse.Bodies[i].Collider, (Circle)World.Instanse.Bodies[j].Collider);
                                        break;
                                    }
                                case ColliderType.Rectangle:
                                    {
                                        flag = Collision.isCollision((Point)World.Instanse.Bodies[i].Collider, (Rectangle)World.Instanse.Bodies[j].Collider);
                                        break;
                                    }
                            }
                            break;
                        }
                    case ColliderType.Circle:
                        {
                            switch (ct2)
                            {
                                case ColliderType.Point:
                                    {
                                        flag = Collision.isCollision((Circle)World.Instanse.Bodies[i].Collider, (Point)World.Instanse.Bodies[j].Collider);
                                        break;
                                    }
                                case ColliderType.Circle:
                                    {
                                        flag = Collision.isCollision((Circle)World.Instanse.Bodies[i].Collider, (Circle)World.Instanse.Bodies[j].Collider);
                                        break;
                                    }
                                case ColliderType.Rectangle:
                                    {
                                        flag = Collision.isCollision((Circle)World.Instanse.Bodies[i].Collider, (Rectangle)World.Instanse.Bodies[j].Collider);
                                        break;
                                    }
                            }
                            break;
                        }
                    case ColliderType.Rectangle:
                        {
                            switch (ct2)
                            {
                                case ColliderType.Point:
                                    {
                                        flag = Collision.isCollision((Rectangle)World.Instanse.Bodies[i].Collider, (Point)World.Instanse.Bodies[j].Collider);
                                        break;
                                    }
                                case ColliderType.Circle:
                                    {
                                        flag = Collision.isCollision((Rectangle)World.Instanse.Bodies[i].Collider, (Circle)World.Instanse.Bodies[j].Collider);
                                        break;
                                    }
                                case ColliderType.Rectangle:
                                    {
                                        flag = Collision.isCollision((Rectangle)World.Instanse.Bodies[i].Collider, (Rectangle)World.Instanse.Bodies[j].Collider);
                                        break;
                                    }
                            }
                            break;
                        }

                }

                if (flag)
                {
                    GameManager.printS(ct1.ToString() + " "+ ct2.ToString());
                }
            }
        }
    }

    public void Tick()
    {
        CollissionCheck();
    }

}
