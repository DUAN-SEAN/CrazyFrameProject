using Crazy.Common;
namespace Crazy.Common
{
	[Message(RpcMessageOpcode.UnitInfo)]
	public partial class UnitInfo {}

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
		 public const ushort CreateUnits = 1003;
		 public const ushort PositionFrameMsg = 1004;
		 public const ushort HeartBeatMessage = 1005;
		 public const ushort C2S_SearchUser = 1006;
		 public const ushort S2C_SearchUser = 1007;
	}
}
