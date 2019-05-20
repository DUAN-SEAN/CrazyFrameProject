using Crazy.Common;
namespace Crazy.Common
{
	[Message(MessageOpcode.ChatOneMessage)]
	public partial class ChatOneMessage : IMessage {}

	[Message(MessageOpcode.C2S_CreateMatchTeam)]
	public partial class C2S_CreateMatchTeam : IMeesage {}

	[Message(MessageOpcode.S2C_CreateMatchTeamComplete)]
	public partial class S2C_CreateMatchTeamComplete : IMeesage {}

	[Message(MessageOpcode.C2S_InvitePlayerMatchTeam)]
	public partial class C2S_InvitePlayerMatchTeam : IMeesage {}

	[Message(MessageOpcode.S2C_InvitePlayerMatchTeam)]
	public partial class S2C_InvitePlayerMatchTeam : IMeesage {}

	[Message(MessageOpcode.C2S_JoinMatchTeam)]
	public partial class C2S_JoinMatchTeam : IMeesage {}

	[Message(MessageOpcode.S2C_JoinMatchTeamComplete)]
	public partial class S2C_JoinMatchTeamComplete : IMeesage {}

	[Message(MessageOpcode.C2S_JoinMatchQueue)]
	public partial class C2S_JoinMatchQueue : IMeesage {}

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
		 public const ushort S2C_JoinMatchTeamComplete = 1007;
		 public const ushort C2S_JoinMatchQueue = 1008;
	}
}
