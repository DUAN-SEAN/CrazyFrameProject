using Crazy.Common;
namespace Crazy.Common
{
	[Message(RpcMessageOpcode.UnitInfo)]
	public partial class UnitInfo {}

////��֤��½��Ϣ
	[Message(RpcMessageOpcode.C2S_Login)]
	public partial class C2S_Login : IRequest {}

	[Message(RpcMessageOpcode.S2C_Login)]
	public partial class S2C_Login : IResponse {}

////����Unit
	[Message(RpcMessageOpcode.CreateUnits)]
	public partial class CreateUnits : IMessage {}

////֡��Ϣ
	[Message(RpcMessageOpcode.PositionFrameMsg)]
	public partial class PositionFrameMsg : IFrameMessage {}

////���������Ϣ
	[Message(RpcMessageOpcode.HeartBeatMessage)]
	public partial class HeartBeatMessage : IMessage {}

////����ϵͳ
	[Message(RpcMessageOpcode.C2S_SearchUser)]
	public partial class C2S_SearchUser : IRequest {}

	[Message(RpcMessageOpcode.S2C_SearchUser)]
	public partial class S2C_SearchUser : IResponse {}

}
namespace Crazy.Common
{
	public static partial class RpcMessageOpcode
	{
		 public const ushort UnitInfo = 1002;
		 public const ushort C2S_Login = 1003;
		 public const ushort S2C_Login = 1004;
		 public const ushort CreateUnits = 1005;
		 public const ushort PositionFrameMsg = 1006;
		 public const ushort HeartBeatMessage = 1007;
		 public const ushort C2S_SearchUser = 1008;
		 public const ushort S2C_SearchUser = 1009;
	}
}
