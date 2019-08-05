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

//����
	[Message(RpcMessageOpcode.C2S_UpdateOnlinePlayerList)]
	public partial class C2S_UpdateOnlinePlayerList : IRequest {}

	[Message(RpcMessageOpcode.S2C_UpdateOnlinePlayerList)]
	public partial class S2C_UpdateOnlinePlayerList : IResponse {}

	[Message(RpcMessageOpcode.OnlinePlayerInfo)]
	public partial class OnlinePlayerInfo {}

}
namespace Crazy.Common
{
	public static partial class RpcMessageOpcode
	{
		 public const ushort C2S_LoginMessage = 1023;
		 public const ushort S2C_LoginMessage = 1024;
		 public const ushort C2S_RegisterMessage = 1025;
		 public const ushort S2C_RegisterMessage = 1026;
		 public const ushort C2S_UpdateOnlinePlayerList = 1027;
		 public const ushort S2C_UpdateOnlinePlayerList = 1028;
		 public const ushort OnlinePlayerInfo = 1029;
	}
}
