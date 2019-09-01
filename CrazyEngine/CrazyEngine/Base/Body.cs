using System;
using System.Collections.Generic;
using System.Linq;
using CrazyEngine.Common;

namespace CrazyEngine.Base
{
    /// <summary>
    /// 物体
    /// </summary>
    public class Body : ObjBase
    {
        private List<Body> _parts = new List<Body>();
        private Point _position = new Point();
        private bool _static = false;
        private Point _velocity = new Point();
        private double _angle;
        private double _angularVelocity;
        private double _density = 0.001;
        private double _inertia;
        private double _mass = 0.1;
        private Vertices _vertices;
        private double _anglePrev;

        public Body Parent { get; set; }

        

        public void Init(List<Point> path)
        {
            Vertices = Vertices.Create(this, path);
            Parts.Add(this);
            Parent = this;
        }

        public void Dispose()
        {
            _position = null;
            _velocity = null;
            Bounds.Dispose();
            Region?.Dispose();
            _vertices.Dispose();
            Axes.Dispose();
            _parts.Clear();
            _parts = null;
        }


        public Point Position
        {
            get { return _position; }
            set
            {
                if(value == null) return;
                var delta = value - _position;
                PositionPrev.Offset(delta);

                foreach (var part in Parts)
                {
                    part.Position.Offset(delta);
                    part.Vertices.Translate(delta);
                    part.Bounds.Update(part.Vertices, Velocity);
                }
            }
        }

        public Point PositionPrev { get; set; } = new Point();

        public Point PositionImpulse { get; set; } = new Point();

        public Point ConstraintImpulse { get; set; } = new Point();

        public double ConstraintAngle { get; set; }

        public Point Force { get; set; } = new Point();

        public Point Velocity
        {
            get { return _velocity; }
            set
            {
                PositionPrev.X = Position.X - value.X;
                PositionPrev.Y = Position.Y - value.Y;
                Velocity.X = value.X;
                Velocity.Y = value.Y;
                Speed = _velocity.Magnitude();
            }
        }

        public Point Forward
        {
            get
            {
                return new Point(-Math.Sin(Angle), Math.Cos(Angle));
            }

        }

        public Bound Bounds { get; set; } = new Bound();

        public Bound Region { get; set; }

        public double AngularVelocity
        {
            get { return _angularVelocity; }
            set
            {
                AnglePrev = Angle - value;
                _angularVelocity = value;
                AngularSpeed = Math.Abs(_angularVelocity);
            }
        }

        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                var delta = value - _angle;
                AnglePrev += delta;

                for (var i = 0; i < _parts.Count; i++)
                {
                    var part = _parts[i];
                    part._angle += delta;
                    part.Vertices.Rotate(delta, _position);
                    part.Axes.Rotate(delta);
                    part.Bounds.Update(part.Vertices, Velocity);
                    if (i > 0)
                    {
                        part.Position.RotateAbout(delta, Position);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化body时的朝向
        /// </summary>
        /// <param name="angle"></param>
        public void InitAngle(double angle)
        {
            var delta = angle - _angle;

            for (var i = 0; i < _parts.Count; i++)
            {
                var part = _parts[i];
                part._angle += delta;
                part.Vertices.Rotate(delta, _position);
                part.Axes.Rotate(delta);
                part.Bounds.Update(part.Vertices, Velocity);
                if (i > 0)
                {
                    part.Position.RotateAbout(delta, Position);
                }
            }
        }

        public double AnglePrev
        {
            get { return _anglePrev; }
            set { _anglePrev = value; }
        }

        public double Area { get; set; }

        public double InverseMass { get; set; }

        public double Mass
        {
            get { return _mass; }
            set
            {
                _mass = value;
                InverseMass = 1 / value;
                _density = value / Area;
            }
        }

        /// <summary>
        /// 是物体角速度和速度的组合，始终是正的，用于判断物体是否为正
        /// </summary>
        public double Motion { get; set; }

        /// <summary>
        /// 转矩
        /// </summary>
        public double Torque { get; set; }

        public double Restitution { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public double Speed { get; set; }
        /// <summary>
        /// 角速度
        /// </summary>
        public double AngularSpeed { get; set; }
        /// <summary>
        /// 密度
        /// </summary>
        public double Density
        {
            get { return _density; }
            set
            {
                _density = value;
                Mass = value * Area;
            }
        }

        public double Timescale { get; set; } = 1;

        public double Slop { get; set; } = 0.05;

        public int TotalContacts { get; set; }
        /// <summary>
        /// 物体是否可见
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 是否为触发器
        /// </summary>
        public bool Trigger { get; set; }

        /// <summary>
        /// 是否是静态物体
        /// </summary>
        public bool Static
        {
            get { return _static; }
            set
            {
                if (value)
                {
                    _static = true;
                    foreach (var part in Parts)
                    {
                        part._static = true;
                        part.Restitution = 0;
                        part.Friction = 1;
                        part._mass = part._inertia = part._density = double.MaxValue;
                        part.InverseMass = part.InverseInertia = 0;
                        part.PositionPrev.X = part.Position.X;
                        part.PositionPrev.Y = part.Position.Y;
                        part.AnglePrev = part.Angle;
                        part._angularVelocity = 0;
                        part.Speed = 0;
                        part.AngularSpeed = 0;
                        part.Motion = 0;
                    }
                }
                else
                {
                    _static = false;
                    foreach (var part in Parts)
                    {
                        part._static = false;
                    }
                }
            }
        }
        /// <summary>
        /// 是否处于睡眠状态
        /// </summary>
        public bool Sleep { get; set; }
        /// <summary>
        /// 惯性
        /// </summary>
        public double Inertia
        {
            get { return _inertia; }
            set
            {
                _inertia = value;
                InverseInertia = 1 / value;
            }
        }
        /// <summary>
        /// 惯性的倒数
        /// </summary>
        public double InverseInertia { get; set; }
        /// <summary>
        /// 摩擦力
        /// </summary>
        public double Friction { get; set; } = 0.1;
        /// <summary>
        /// static为true时的摩擦力
        /// </summary>
        public double FrictionStatic { get; set; } = 0.5;
        /// <summary>
        /// 在无碰撞时的摩擦力
        /// </summary>
        public double FrictionAir { get; set; } = 0.01;

        public Vertices Vertices
        {
            get { return _vertices; }
            set
            {
                _vertices = value.Vertexes[0].Body == this ? value : Vertices.Create(this);
                Axes = Vertices.ToAxes();
                Area = Vertices.Area();
                Mass = Density * Area;
                Vertices.Translate(Vertices.Centre() * -1);
                Inertia = 4 * Vertices.Inertia(Mass);
                Vertices.Translate(Position);
                Bounds.Update(Vertices, Velocity);
            }
        }

        public Axes Axes { get; set; }

        public List<Body> Parts
        {
            get { return _parts; }
            set
            {
                var parts = value.ToList();

                _parts.Add(this);
                Parent = this;

                foreach (var part in parts.Where(x => x != this))
                {
                    part.Parent = this;
                    _parts.Add(part);
                }

                if (_parts.Count == 1)
                    return;

                var vertices = new Vertices(_parts.SelectMany(x => x.Vertices.Vertexes).ToList());
                vertices.ClockwiseSort();

                var hull = vertices.Hull();
                var hullCentre = hull.Centre();

                Vertices = hull;
                Vertices.Translate(hullCentre);

                var mass = 0d;
                var area = 0d;
                var inertia = 0d;
                var centre = new Point();

                for (var i = _parts.Count == 1 ? 0 : 1; i < _parts.Count; i++)
                {
                    var part = _parts[i];
                    mass += part.Mass;
                    area += part.Area;
                    inertia += part.Inertia;
                    centre += part.Position * (part.Mass >= double.MaxValue ? part.Mass : 1d);
                }

                centre /= mass >= double.MaxValue ? mass : _parts.Count;


                Area = area;
                Parent = this;
                Position.X = centre.X;
                Position.Y = centre.Y;
                PositionPrev.X = centre.X;
                PositionPrev.Y = centre.Y;

                Mass = mass;
                Inertia = inertia;
                Position = centre;
            }
        }

        public void Update(double delta, double timescale, double correction, Bound bounds)
        {
            if (Math.Sign(Mass) == 0) return;

            var deltaTimeSquared = Math.Pow(delta * timescale * Timescale, 2);

            var frictionAir = 1 - FrictionAir * timescale * Timescale;
            var velocityPrevX = Position.X - PositionPrev.X;
            var velocityPrevY = Position.Y - PositionPrev.Y;

            Velocity.X = velocityPrevX * frictionAir * correction + Force.X / Mass * deltaTimeSquared;
            Velocity.Y = velocityPrevY * frictionAir * correction + Force.Y / Mass * deltaTimeSquared;

            PositionPrev.X = Position.X;
            PositionPrev.Y = Position.Y;
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;

            //_angularVelocity = (Angle - AnglePrev) * frictionAir * correction + Torque / Inertia * deltaTimeSquared;
            AnglePrev = Angle;
            Angle += AngularVelocity;

            Speed = Velocity.Magnitude();
            AngularSpeed = Math.Abs(AngularVelocity);

            for (var i = 0; i < _parts.Count; i++)
            {
                var part = _parts[i];
                part.Vertices.Translate(Velocity);
                if (i > 0)
                {
                    part._position.Offset(Velocity);
                }
                part.Vertices.Rotate(AngularVelocity, Position);
                part.Axes.Rotate(AngularVelocity);
                if (i > 0)
                {
                    part._position.RotateAbout(AngularVelocity, _position);
                }
                part.Bounds.Update(Vertices, Velocity);
            }
        }

    }
}