using System;
using System.Collections.Generic;
using System.Linq;
using CrazyEngine.Common;

namespace CrazyEngine.Base
{
    /// <summary>
    /// 复合体
    /// </summary>
    public class Composite : ObjBase
    {
        public Composite Parent { get; private set; }

        public bool Modified { get; private set; }

        public List<Body> Bodies { get; private set; } = new List<Body>();

        public List<Composite> Composites { get; private set; } = new List<Composite>();

        public List<Constraint> Constraints { get; private set; } = new List<Constraint>();

        public void SetModified(bool isModified, bool updateParents = false, bool updateChildren = false)
        {
            Modified = isModified;
            if (updateParents)
            {
                Parent?.SetModified(isModified, true, updateChildren);
            }
            if (updateChildren)
            {
                foreach (var composite in Composites)
                {
                    composite.SetModified(isModified, updateParents, true);
                }
            }
        }

        public Composite()
        {
            Type = ObjType.Composite;
        }

        public Composite( List<ObjBase> Objs)
        {
            foreach (var obj in Objs)
            {
                Add(obj);
            }
        }

        public IEnumerable<Body> AllBodies =>
            Bodies.Union(Composites.SelectMany(composite => composite.AllBodies));

        public IEnumerable<Constraint> AllConstraints =>
            Constraints.Union(Composites.SelectMany(composite => composite.AllConstraints));

        public IEnumerable<Composite> AllComposites =>
            Composites.Union(Composites.SelectMany(composite => composite.AllComposites));

        public void Add(ObjBase obj)
        {
            switch (obj.Type)
            {
                case ObjType.Body:
                    AddBody(obj as Body);
                    break;
                case ObjType.Constraint:
                    AddConstraint(obj as Constraint);
                    break;
                case ObjType.Composite:
                    AddComposite(obj as Composite);
                    break;
            }
        }
        private void AddBody(Body body)
        {
            Bodies.Add(body);
            SetModified(true, true);
        }

        private void AddConstraint(Constraint constraint)
        {
            Constraints.Add(constraint);
            SetModified(true, true);
        }

        private void AddComposite(Composite composite)
        {
            Composites.Add(composite);
            composite.Parent = this;
            SetModified(true, true);
        }

        public void Remove(ObjBase obj)
        {
            switch (obj.Type)
            {
                case ObjType.Body:
                    RemoveBody(obj as Body);
                    break;
                case ObjType.Constraint:
                    RemoveConstraint(obj as Constraint);
                    break;
                case ObjType.Composite:
                    RemoveConstraint(obj as Composite);
                    break;
            }
        }

        private void RemoveBody(Body body)
        {
            if (Bodies.Contains(body)) Bodies.Remove(body);
            SetModified(true, true, true);
        }

        private void RemoveConstraint(Constraint constraint)
        {
            if (Constraints.Contains(constraint)) Constraints.Remove(constraint);
            SetModified(true, true, true);
        }

        private void RemoveConstraint(Composite composite)
        {
            if (Composites.Contains(composite)) Composites.Remove(composite);
            SetModified(true, true, true);
        }
    }
}