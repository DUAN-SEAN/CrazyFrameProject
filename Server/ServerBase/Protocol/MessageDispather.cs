using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
namespace Crazy.Common
{
    /// <summary>
    /// 消息分发类型，将网络消息和handler进行绑定
    ///
    /// </summary>
    public class MessageDispather<PlayerContext>
    {
        /// <summary>
        /// Load 在组件启动时调用
        /// </summary>
        public void Init()
        {
            Handlers.Clear();

            // AppType appType = StartConfigComponent.Instance.StartConfig.AppType;

            List<Type> types = TypeManager.Instance.GetTypes(typeof(MessageHandlerAttribute));

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(MessageHandlerAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                MessageHandlerAttribute messageHandlerAttribute = attrs[0] as MessageHandlerAttribute;
                //if (!messageHandlerAttribute.Type.Is(appType))
                //{
                //    continue;
                //}

                IMHandler iMHandler = Activator.CreateInstance(type) as IMHandler;
                if (iMHandler == null)
                {
                    Log.Error($"message handle {type.Name} 需要继承 IMHandler");
                    continue;
                }

                Type messageType = iMHandler.GetMessageType();//获取消息的类型
                ushort opcode = OpcodeTypeDictionary.Instance.GetIdByType(messageType);
                if (opcode == 0)
                {
                    Log.Error($"消息opcode为0: {messageType.Name}");
                    continue;
                }
                RegisterHandler(opcode, iMHandler);
            }
        }
        public void RegisterHandler(ushort opcode, IMHandler handler)
        {
            if (!Handlers.ContainsKey(opcode))
            {
                Handlers.Add(opcode, new List<IMHandler>());
                //Log.Debug(opcode.ToString());
            }
            //Log.Debug(opcode.ToString());
            Handlers[opcode].Add(handler);
        }

        public void Handle(PlayerContext sender, MessageInfo messageInfo)
        {
            List<IMHandler> handlers;
            if (!Handlers.TryGetValue(messageInfo.Opcode, out handlers))
            {
                Log.Error($"消息没有处理:{messageInfo.Opcode} {messageInfo.Message}");
                return;
            }

            foreach (IMHandler ev in handlers)
            {
                try
                {

                    ev.Handle(sender, messageInfo.Message);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        public readonly Dictionary<ushort, List<IMHandler>> Handlers = new Dictionary<ushort, List<IMHandler>>();

    }
}
