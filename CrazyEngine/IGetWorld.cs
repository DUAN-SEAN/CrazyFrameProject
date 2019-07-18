using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyEngine
{
    public interface IGetWorld
    {
        /// <summary>
        /// 获得该对象中的世界引擎
        /// </summary>
        World GetCurrentWorld();
    }
}
