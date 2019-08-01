using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWanderLogicalCommon
{
    public interface IHealthShieldComponent
    {
        void GetHP();
    }
    public interface IInternalHealthShieldComponent:IHealthShieldComponent
    {
        
    }
}
