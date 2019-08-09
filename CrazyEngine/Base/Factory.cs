using System.Collections.Generic;
using CrazyEngine.Common;

namespace CrazyEngine.Base
{
    public static class Factory
    {
        public static Body CreateRectangleBody(double x, double y, double width, double height, bool isStatic = false)
        {
            var path = new List<Point> { new Point(0, 0), new Point(width, 0), new Point(width, height), new Point(0,height) };
            var body = new Body {Static = isStatic};
            body.Init(path);
            body.Position = new Point(x, y);
            return body;
        }

        public static Body CreateTriggleBody(double x, double y, double d, double h, bool isStatic = false)
        {
            var path = new List<Point> { new Point(0, 0), new Point(d, 0), new Point(d / 2, h) };
            var body = new Body { Static = isStatic };
            body.Init(path);
            body.Position = new Point(x, y);
            return body;
        }

        public static Body CreateRhombusBody(double x, double y,double width, double height, bool isStatic = false)
        {
            var path = new List<Point> { new Point(0, 0), new Point(width/2, height/2), new Point(width, 0), new Point(width/2, -height/2) };
            var body = new Body { Static = isStatic };
            body.Init(path);
            body.Position = new Point(x, y);
            return body;
        }

        public static Body CreateCircleBody(double x, double y, double radius, bool isStatic = false)
        {
            var gr = 0.707107 * radius;
            var path = new List<Point> { new Point(-radius, 0), new Point(-gr, gr), new Point(0, radius), new Point(gr, gr)
                                        ,new Point(radius, 0), new Point(gr, -gr), new Point(0, -radius), new Point(-gr, -gr) };
            var body = new Body { Static = isStatic };
            body.Init(path);
            body.Position = new Point(x, y);
            return body;
        }
        

    }
}
