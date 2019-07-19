using Crazy.Common;
namespace Crazy.Common
{
	[Message(MessageOpcode.ChatOneMessage)]
	public partial class ChatOneMessage : IMessage {}

// ��ƥ��ϵͳ��ص���Ϣд����
	[Message(MessageOpcode.C2S_CreateMatchTeam)]
	public partial class C2S_CreateMatchTeam : IMessage {}

	[Message(MessageOpcode.S2C_CreateMatchTeamComplete)]
	public partial class S2C_CreateMatchTeamComplete : IMessage {}

	[Message(MessageOpcode.C2S_InvitePlayerMatchTeam)]
	public partial class C2S_InvitePlayerMatchTeam : IMessage {}

	[Message(MessageOpcode.S2C_InvitePlayerMatchTeam)]
	public partial class S2C_InvitePlayerMatchTeam : IMessage {}

	[Message(MessageOpcode.C2S_JoinMatchTeam)]
	public partial class C2S_JoinMatchTeam : IMessage {}

	[Message(MessageOpcode.S2CM_JoinMatchTeamComplete)]
	public partial class S2CM_JoinMatchTeamComplete : IMessage {}

	[Message(MessageOpcode.C2S_GetMatchTeamInfo)]
	public partial class C2S_GetMatchTeamInfo : IMessage {}

	[Message(MessageOpcode.S2C_UpdateMatchTeamInfo)]
	public partial class S2C_UpdateMatchTeamInfo : IMessage {}

	[Message(MessageOpcode.MatchTeamInfo)]
	public partial class MatchTeamInfo {}

	[Message(MessageOpcode.C2S_ExitMatchTeam)]
	public partial class C2S_ExitMatchTeam : IMessage {}

	[Message(MessageOpcode.S2CM_ExitMatchTeamComplete)]
	public partial class S2CM_ExitMatchTeamComplete : IMessage {}

	[Message(MessageOpcode.C2S_JoinMatchQueue)]
	public partial class C2S_JoinMatchQueue : IMessage {}

	[Message(MessageOpcode.S2CM_JoinMatchQueueComplete)]
	public partial class S2CM_JoinMatchQueueComplete : IMessage {}

	[Message(MessageOpcode.C2S_ExitMatchQueue)]
	public partial class C2S_ExitMatchQueue : IMessage {}

	[Message(MessageOpcode.S2CM_ExitMatchQueue)]
	public partial class S2CM_ExitMatchQueue : IMessage {}

	[Message(MessageOpcode.S2CM_MatchingFinish)]
	public partial class S2CM_MatchingFinish : IMessage {}

// ��ս��ϵͳ��ص���Ϣд������
	[Message(MessageOpcode.S2CM_CreateBattleBarrier)]
	public partial class S2CM_CreateBattleBarrier : IMessage {}

	[Message(MessageOpcode.CreateBattleBarrierInfo)]
	public partial class CreateBattleBarrierInfo {}

// ս����Ϣ
	[Message(MessageOpcode.C2S_BattleCommandMessage)]
	public partial class C2S_BattleCommandMessage : IMessage {}

//����Ϊ����ͨ����Ϣ
	[Message(MessageOpcode.C2S_ReConnectByLogin)]
	public partial class C2S_ReConnectByLogin : IMessage {}

}
namespace Crazy.Common
{
	public static partial class MessageOpcode
	{
		 public const ushort ChatOneMessage = 1001;
		 public const ushort C2S_CreateMatchTeam = 1002;
		 public const ushort S2C_CreateMatchTeamComplete = 1003;
		 public const ushort C2S_InvitePlayerMatchTeam = 1004;
		 public const ushort S2C_InvitePlayerMatchTeam = 1005;
		 public const ushort C2S_JoinMatchTeam = 1006;
		 public const ushort S2CM_JoinMatchTeamComplete = 1007;
		 public const ushort C2S_GetMatchTeamInfo = 1008;
		 public const ushort S2C_UpdateMatchTeamInfo = 1009;
		 public const ushort MatchTeamInfo = 1010;
		 public const ushort C2S_ExitMatchTeam = 1011;
		 public const ushort S2CM_ExitMatchTeamComplete = 1012;
		 public const ushort C2S_JoinMatchQueue = 1013;
		 public const ushort S2CM_JoinMatchQueueComplete = 1014;
		 public const ushort C2S_ExitMatchQueue = 1015;
		 public const ushort S2CM_ExitMatchQueue = 1016;
		 public const ushort S2CM_MatchingFinish = 1017;
		 public const ushort S2CM_CreateBattleBarrier = 1018;
		 public const ushort CreateBattleBarrierInfo = 1019;
		 public const ushort C2S_BattleCommandMessage = 1020;
		 public const ushort C2S_ReConnectByLogin = 1021;
	}
}
