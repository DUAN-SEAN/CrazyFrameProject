using Crazy.Common;
namespace Crazy.Common
{
// ��ս��ϵͳ��ص���Ϣд������
	[Message(BattleMessageOpcode.C2S_ReadyBattleBarrierReq)]
	public partial class C2S_ReadyBattleBarrierReq : IBattleMessage {}

// ��ս��ϵͳ��ص���Ϣд������
	[Message(BattleMessageOpcode.S2CM_ReadyBattleBarrierAck)]
	public partial class S2CM_ReadyBattleBarrierAck : IBattleMessage {}

// ս��ָ��
	[Message(BattleMessageOpcode.C2S_BattleCommandMessage)]
	public partial class C2S_BattleCommandMessage : IBattleMessage {}

//����������Ϣ
	[Message(BattleMessageOpcode.S2C_BodyInitBattleMessage)]
	public partial class S2C_BodyInitBattleMessage : IBattleMessage {}

//�������������߼�
	[Message(BattleMessageOpcode.C2S_CommandBattleMessage)]
	public partial class C2S_CommandBattleMessage : IBattleMessage {}

	[Message(BattleMessageOpcode.S2C_EventBattleMessage)]
	public partial class S2C_EventBattleMessage : IBattleMessage {}

	[Message(BattleMessageOpcode.S2C_SyncHpShieldStateBattleMessage)]
	public partial class S2C_SyncHpShieldStateBattleMessage : IBattleMessage {}

	[Message(BattleMessageOpcode.S2C_SyncPhysicsStateBattleMessage)]
	public partial class S2C_SyncPhysicsStateBattleMessage : IBattleMessage {}

	[Message(BattleMessageOpcode.C2S_ExitBattleMessage)]
	public partial class C2S_ExitBattleMessage : IBattleMessage {}

	[Message(BattleMessageOpcode.S2C_ExitBattleMessage)]
	public partial class S2C_ExitBattleMessage : IBattleMessage {}

}
namespace Crazy.Common
{
	public static partial class BattleMessageOpcode
	{
		 public const ushort C2S_ReadyBattleBarrierReq = 1002;
		 public const ushort S2CM_ReadyBattleBarrierAck = 1003;
		 public const ushort C2S_BattleCommandMessage = 1004;
		 public const ushort S2C_BodyInitBattleMessage = 1005;
		 public const ushort C2S_CommandBattleMessage = 1006;
		 public const ushort S2C_EventBattleMessage = 1007;
		 public const ushort S2C_SyncHpShieldStateBattleMessage = 1008;
		 public const ushort S2C_SyncPhysicsStateBattleMessage = 1009;
		 public const ushort C2S_ExitBattleMessage = 1010;
		 public const ushort S2C_ExitBattleMessage = 1011;
	}
}
