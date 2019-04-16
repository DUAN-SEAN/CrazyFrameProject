using System;
using System.Collections.Generic;
using System.Text;

namespace Crazy.Common
{
   

    public class OpcodeTypeComponent
    {
        /// <summary>
        /// 协议 和 消息类型双映射
        /// </summary>
        private readonly DoubleMap<ushort, Type> opcodeTypes = new DoubleMap<ushort, Type>();
        /// <summary>
        /// 协议和消息类型的实例字典
        /// Key 协议 ； value 消息类型实例
        /// </summary>
        private readonly Dictionary<ushort, object> typeMessages = new Dictionary<ushort, object>();

        public void Load()
        {
            this.opcodeTypes.Clear();
            this.typeMessages.Clear();

            List<Type> types =TypeManager.Instance.GetTypes(typeof(MessageAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(MessageAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                MessageAttribute messageAttribute = attrs[0] as MessageAttribute;
                if (messageAttribute == null)
                {
                    continue;
                }

                this.opcodeTypes.Add(messageAttribute.Opcode, type);
                this.typeMessages.Add(messageAttribute.Opcode, Activator.CreateInstance(type));
            }
        }

        public ushort GetOpcode(Type type)
        {
            return this.opcodeTypes.GetKeyByValue(type);
        }

        public Type GetType(ushort opcode)
        {
            return this.opcodeTypes.GetValueByKey(opcode);
        }

        // 客户端为了0GC需要消息池，服务端消息需要跨协程不需要消息池
        public object GetInstance(ushort opcode)
        {
#if SERVER
			Type type = this.GetType(opcode);
			return Activator.CreateInstance(type);
#else
            return this.typeMessages[opcode];
#endif
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}
