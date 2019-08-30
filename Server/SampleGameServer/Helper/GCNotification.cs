using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Helper
{
    /// <summary>
    /// GC检测工具
    /// </summary>
    public static class GCNotification
    {
        private static Action<Int32> s_gcDone = null;

        public static event Action<Int32> GCDone
        {
            add
            {
                if (s_gcDone == null)
                {
                    new GenObject(0);
                    new GenObject(2);
                    s_gcDone += value;
                }
            }
            remove
            {
                s_gcDone = value;
            }
        }
        private sealed class GenObject
        {
            private Int32 m_generation;

            public GenObject(Int32 generation)
            {
                m_generation = generation;
            }

            ~GenObject()
            {
                if (GC.GetGeneration(this) >= m_generation)
                {
                    var temp = Volatile.Read(ref s_gcDone);
                    temp?.Invoke(m_generation);
                }

                if ((s_gcDone != null))
                {
                    
                    if (m_generation == 0) new GenObject(0);
                    else GC.ReRegisterForFinalize(this);
                }
            }
        }
    }


    
    

  
}
