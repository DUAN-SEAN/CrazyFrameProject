using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine;
namespace SpaceShip.Factory
{
    public class MeteoriteInBody : EnviromentInBody
    {
        public MeteoriteInBody()
        {

        }

        public MeteoriteInBody(Vector2 vector, float radius) : base(vector, radius)
        {
        }

    }
}
