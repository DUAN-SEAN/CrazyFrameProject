using System;

namespace CrazyEngine
{
    public struct Line : IBaseStruct
    {
        /// <summary>
        /// 线段的起点
        /// </summary>
        public Vector2 Start { set; get; }
        /// <summary>
        /// 线段的终点
        /// </summary>
        public Vector2 End { set; get; }
        /// <summary>
        /// 线段的长度
        /// </summary>
        public float Length
        {
            get
            {
                return (float)Math.Sqrt((End.x - Start.x) * (End.x - Start.x) + (End.y - Start.y) * (End.y - Start.y));
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Line(Vector2 start, Vector2 end)
        {


            Start = start;
            End = end;

        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        public Line(float startX, float startY, float endX, float endY)
        {
            Vector2 start = new Vector2(startX, startY);
            Vector2 end = new Vector2(endX, endY);



            Start = start;
            End = end;

        }

        /// <summary>
        /// 线段的中点
        /// </summary>
        public Vector2 Center
        {
            set
            {
                float dX = End.x - Start.x;
                float dY = End.y - Start.y;
                Start = value + new Vector2(dX / 2, dY / 2);
                End = value - new Vector2(dX / 2, dY / 2);
                //  LogUI.Log("set start:"+Start + "end:" + End);
            }
            get
            {
                //LogUI.Log("get start:" + Start + "end:" + End);

                return new Vector2(End.x - Start.x, End.y - Start.y);
            }
        }

        /// <summary>
        /// 绕点旋转一定角度
        /// </summary>
        /// <param name="v"></param>
        /// <param name="degree"></param>
        /// <returns></returns>
        public Line Rotate(Vector2 v, int degree)
        {
            return new Line(Start.Rotate(v, degree), End.Rotate(v, degree));
        }

        /// <summary>
        /// 线绕中心转到forward
        /// </summary>
        /// <param name="forward"></param>
        /// <returns></returns>
        public Line Rotate(Vector2 forward)
        {
            forward = forward.normalized;
            Vector2 newStart = Center + forward * Length / 2;
            Vector2 newEnd = Center - forward * Length / 2;
            return new Line(newStart, newEnd);
        }

    }
}