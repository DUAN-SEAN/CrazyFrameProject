using Crazy.Common;
namespace Crazy.Common
{
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

}
namespace Crazy.Common
{
	public static partial class BattleMessageOpcode
	{
		 public const ushort C2S_BattleCommandMessage = 1002;
		 public const ushort S2C_BodyInitBattleMessage = 1003;
		 public const ushort C2S_CommandBattleMessage = 1004;
		 public const ushort S2C_EventBattleMessage = 1005;
		 public const ushort S2C_SyncHpShieldStateBattleMessage = 1006;
		 public const ushort S2C_SyncPhysicsStateBattleMessage = 1007;
	}
}
