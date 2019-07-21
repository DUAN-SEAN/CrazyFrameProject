using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine;
using SpaceShip.Base;
namespace SpaceShip.Factory
{
    [Serializable]
    abstract public class ShipBase : RectangleEntity
    {
        public int HP;
        public int Armor;
        public float IntervalTime;
        public int maxArmor;

        public bool isLeft;
        public bool isRight;
        [NonSerialized]
        public ISBZhuying iSBSean;
        public ShipBase()
        {
            HP = 10;
            Armor = 10;
            maxArmor = 10;
            IntervalTime = 5;
        }
        public ShipBase(Vector2 min, Vector2 max) : base(min, max)
        {
            HP = 10;
            Armor = 10;
            maxArmor = 10;
            IntervalTime = 5;

        }
        public virtual void Init(Level seanD)
        {
            base.InitWorld(seanD);
            iSBSean = seanD;

        }

        public override void OnCollisionStay(Collider collider)
        {
            if (collider.body.Label.HasFlag(Label) || Label.HasFlag(collider.body.Label)) return;
            //LogUI.Log(Position + " " + collider.body.Position);

            if (Armor > 0) Armor--;
            else if (Armor == 0) HP--;

            if (HP == 0)
                Dispose();
        }

        public override void Dispose()
        {
            
            HP = 0;
            iSBSean.GetBodyMessages().Enqueue(new BodyDestoriedMessage(this));

            base.Dispose();
        }
    }

}
