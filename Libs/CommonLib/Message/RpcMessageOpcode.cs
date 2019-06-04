using Crazy.Common;
namespace Crazy.Common
{
////��֤��½��Ϣ
	[Message(RpcMessageOpcode.C2S_LoginMessage)]
	public partial class C2S_LoginMessage : IRequest {}

	[Message(RpcMessageOpcode.S2C_LoginMessage)]
	public partial class S2C_LoginMessage : IResponse {}

	[Message(RpcMessageOpcode.C2S_RegisterMessage)]
	public partial class C2S_RegisterMessage : IRequest {}

	[Message(RpcMessageOpcode.S2C_RegisterMessage)]
	public partial class S2C_RegisterMessage : IResponse {}

}
namespace Crazy.Common
{
	public static partial class RpcMessageOpcode
	{
		 public const ushort C2S_LoginMessage = 1018;
		 public const ushort S2C_LoginMessage = 1019;
		 public const ushort C2S_RegisterMessage = 1020;
		 public const ushort S2C_RegisterMessage = 1021;
	}
}
