using System;
using System.Collections.Generic;


namespace CrazyEngine
{
    public class World
    {
        /// <summary>
        /// 世界里所有的物体 
        /// </summary>
        public List<Body> Bodies;

        /// <summary>
        /// 世界的长
        /// </summary>
        public float WorldX = 1000f;
        /// <summary>
        /// 世界的宽
        /// </summary>
        public float WorldY = 1000f;

        /// <summary>
        /// 世界引擎停止
        /// </summary>
        public bool isWorldStop
        {
            set
            {
                isworldstop = value;

                foreach (var body in Bodies)
                {
                    body.Enable = value;
                }
            }

            get
            {
                return isworldstop;
            }
        }
        private bool isworldstop;




      

        public World()
        {
            Bodies = new List<Body>();
        }
        public T InitInWorld<T>(Vector2 vector) where T : PointEntity, new()
        {
            T t = new T()
            {
                Collider = new Collider(vector),
                Position = vector,
                Enable = isworldstop

            };

            //Bodies.Add(t);



            return t;
        }

        public T InitInWorld<T>(Vector2 min, Vector2 max) where T : RectangleEntity, new()
        {
            T t = new T()
            {
                Position = new Vector2((max.x + min.x) / 2, (max.y + min.y) / 2),
                Collider = new Collider(min, max),
                Enable = isworldstop

            };
            //Bodies.Add(t);
            return t;
        }

        public T InitInWorld<T>(Line line) where T : LineEntity, new()
        {
            T t = new T()
            {
                Collider = new Collider(line),
                Position = (line.Start + line.End) / 2,
                Length = line.Length,
                StartPoint = line.Start,
                Enable = isworldstop

            };
            //Bodies.Add(t);
            return t;
        }

        public T InitInWorld<T>(Vector2 vector, float radius) where T : CircleEntity, new()
        {
            T t = new T()
            {
                Collider = new Collider(vector, radius),
                Position = vector,
                Enable = isworldstop

            };
            //Bodies.Add(t);
            return t;
        }

        public void Dispose()
        {
            isWorldStop = true;
            for(int i = 0; i < Bodies.Count; i++)
            {
                Bodies[i].Dispose();
            }

            Bodies = null;
        }

    }
}