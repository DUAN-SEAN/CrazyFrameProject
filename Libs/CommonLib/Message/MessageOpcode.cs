using Crazy.Common;
namespace Crazy.Common
{
	[Message(MessageOpcode.ChatOneMessage)]
	public partial class ChatOneMessage : IMessage {}

}
namespace Crazy.Common
{
	public static partial class MessageOpcode
	{
		 public const ushort ChatOneMessage = 1001;
	}
}
