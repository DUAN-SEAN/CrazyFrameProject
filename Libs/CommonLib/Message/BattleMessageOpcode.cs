using Crazy.Common;
namespace Crazy.Common
{
// ս��ָ��
	[Message(BattleMessageOpcode.C2S_BattleCommandMessage)]
	public partial class C2S_BattleCommandMessage : IBattleMessage {}

//����������Ϣ
	[Message(BattleMessageOpcode.S2C_BodyInitBattleMessage)]
	public partial class S2C_BodyInitBattleMessage : IBattleMessage {}

}
namespace Crazy.Common
{
	public static partial class BattleMessageOpcode
	{
		 public const ushort C2S_BattleCommandMessage = 1001;
		 public const ushort S2C_BodyInitBattleMessage = 1002;
	}
}
