using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine;
namespace SpaceShip.Factory
{
    using SpaceShip.Base;
    using System;
    public class PlayerInBody : ShipBase
    {
        LightInBody light;

        public PlayerInBody()
        {
        }

        public PlayerInBody(Vector2 min, Vector2 max) : base(min, max)
        {


        }


        public void AttackByType(int type)
        {
            switch (type)
            {
                case 1:
                    var weanpon1 = BodyFactory.Instance.LoadBoltWeaponByType<BoltInBody>((Level)iSBSean, this);
                    break;
                case 2:
                    var weanpon2 = BodyFactory.Instance.LoadMissileWeaponByType<MissileInBody>((Level)iSBSean, this);
                    break;
                case 3:
                    var weanpon3 = BodyFactory.Instance.LoadMineWeaponByType<MineInBody>((Level)iSBSean, this);
                    break;
                case 4:
                    light = BodyFactory.Instance.LoadLightWeaponByType<LightInBody>((Level)iSBSean, this);
                    break;
            }

        }

        public void UnAttackByType(int type)
        {
            switch (type)
            {
               
                case 4:
                    if (light == null) break;
                    light.Dispose();
                    light = null;
                    break;
                default:
                    //
                    break;
            }
        }
    }

}
