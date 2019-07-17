using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    /// <summary>
    /// 用于生成BEntity
    /// </summary>
    public static class BEntityFactory
    {
        

        public static T CreateEntity<T>() where T:BEntity
        {
            BEntity bEntity;
            bEntity =  m_objectPool.Fetch<T>();
            bEntity.Start(GenerateId++);
            return bEntity as T;
        }
        public static void Recycle(BEntity bEntity)
        {
            m_objectPool.Recycle(bEntity);
        }
        private static UInt64 GenerateId = 0;
        private static ObjectPool m_objectPool = new ObjectPool();
    }
}
