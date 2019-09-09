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

//��ȡ�ɴ��б�
	[Message(RpcMessageOpcode.C2S_ShipInfoReq)]
	public partial class C2S_ShipInfoReq : IRequest {}

//��ȡ�ɴ��б�
	[Message(RpcMessageOpcode.S2C_ShipInfoAck)]
	public partial class S2C_ShipInfoAck : IResponse {}

//�ϴ��ɴ���������
	[Message(RpcMessageOpcode.C2S_UpLoadShipInfoReq)]
	public partial class C2S_UpLoadShipInfoReq : IRequest {}

//�ϴ��ɴ�������Ӧ
	[Message(RpcMessageOpcode.S2C_UpLoadShipInfoAck)]
	public partial class S2C_UpLoadShipInfoAck : IResponse {}

//��ȡ�ɴ��б�
	[Message(RpcMessageOpcode.C2S_DelayReq)]
	public partial class C2S_DelayReq : IRequest {}

//��ȡ�ɴ��б�
	[Message(RpcMessageOpcode.S2C_DelayAck)]
	public partial class S2C_DelayAck : IResponse {}

}
namespace Crazy.Common
{
	public static partial class RpcMessageOpcode
	{
		 public const ushort C2S_LoginMessage = 1038;
		 public const ushort S2C_LoginMessage = 1039;
		 public const ushort C2S_RegisterMessage = 1040;
		 public const ushort S2C_RegisterMessage = 1041;
		 public const ushort C2S_UpdateOnlinePlayerList = 1042;
		 public const ushort S2C_UpdateOnlinePlayerList = 1043;
		 public const ushort OnlinePlayerInfo = 1044;
		 public const ushort C2S_ShipInfoReq = 1045;
		 public const ushort S2C_ShipInfoAck = 1046;
		 public const ushort C2S_UpLoadShipInfoReq = 1047;
		 public const ushort S2C_UpLoadShipInfoAck = 1048;
		 public const ushort C2S_DelayReq = 1049;
		 public const ushort S2C_DelayAck = 1050;
	}
}
