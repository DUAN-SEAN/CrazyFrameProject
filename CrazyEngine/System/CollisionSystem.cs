using System;
namespace CrazyEngine
{
    public class CollisionSystem : ITickable
    {

        private void CollissionCheck()
        {
            //碰撞检测
            for (int i = 0; i < World.Instanse.Bodies.Count; i++)
            {
                for (int j = i + 1; j < World.Instanse.Bodies.Count; j++)
                {
                    bool flag = false;
                    Collider collider1 = World.Instanse.Bodies[i].Collider;
                    Collider collider2 = World.Instanse.Bodies[j].Collider;

                    if (!collider1.body.Enable || !collider2.body.Enable) continue;

                    switch (collider1.ColliderType)
                    {
                        case ColliderType.Point:
                            {
                                switch (collider2.ColliderType)
                                {
                                    case ColliderType.Point:
                                        {
                                            flag = Collision.isCollision((Point)collider1.collider, (Point)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Circle:
                                        {
                                            flag = Collision.isCollision((Point)collider1.collider, (Circle)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Rectangle:
                                        {
                                            flag = Collision.isCollision((Point)collider1.collider, (Rectangle)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Line:
                                        {
                                            flag = Collision.isCollision((Point)collider1.collider, (Line)collider2.collider);
                                            break;
                                        }
                                }
                                break;
                            }

                        case ColliderType.Circle:
                            {
                                switch (collider2.ColliderType)
                                {
                                    case ColliderType.Point:
                                        {
                                            flag = Collision.isCollision((Circle)collider1.collider, (Point)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Circle:
                                        {
                                            flag = Collision.isCollision((Circle)collider1.collider, (Circle)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Rectangle:
                                        {
                                            flag = Collision.isCollision((Circle)collider1.collider, (Rectangle)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Line:
                                        {
                                            flag = Collision.isCollision((Circle)collider1.collider, (Line)collider2.collider);
                                            break;
                                        }

                                }
                                break;
                            }
                        case ColliderType.Rectangle:
                            {
                                switch (collider2.ColliderType)
                                {
                                    case ColliderType.Point:
                                        {
                                            flag = Collision.isCollision((Rectangle)collider1.collider, (Point)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Circle:
                                        {
                                            flag = Collision.isCollision((Rectangle)collider1.collider, (Circle)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Rectangle:
                                        {
                                            flag = Collision.isCollision((Rectangle)collider1.collider, (Rectangle)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Line:
                                        {
                                            flag = Collision.isCollision((Rectangle)collider1.collider, (Line)collider2.collider);
                                            break;
                                        }
                                }
                                break;
                            }

                        case ColliderType.Line:
                            {
                                switch (collider2.ColliderType)
                                {
                                    case ColliderType.Point:
                                        {
                                            flag = Collision.isCollision((Line)collider1.collider, (Point)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Circle:
                                        {
                                            flag = Collision.isCollision((Line)collider1.collider, (Circle)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Rectangle:
                                        {
                                            flag = Collision.isCollision((Line)collider1.collider, (Rectangle)collider2.collider);
                                            break;
                                        }
                                    case ColliderType.Line:
                                        {
                                            flag = Collision.isCollision((Line)collider1.collider, (Line)collider2.collider);
                                            break;
                                        }
                                }
                                break;
                            }
                    }

                    if (flag)
                    {


                        collider1.OnCollisionStay(collider2);
                        collider2.OnCollisionStay(collider1);
                    }
                }
            }



        }

        public void Tick()
        {
            CollissionCheck();
        }

    }
}