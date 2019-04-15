using System;

namespace BlackJack.LibClient
{
	public class CCMSGConnectionFailure
	{
        public Int32 MessageId { get { return (Int32)LocalMsgId.CCMSGConnectionFailure; } }
	}

    public class CCMSGConnectionSendFailure
    {
        public Int32 MessageId { get { return (Int32)LocalMsgId.CCMSGConnectionSendFailure; } }

        public String ExceptionInfo { get; set; }
    }

    public class CCMSGConnectionRecvFailure
    {
        public Int32 MessageId { get { return (Int32)LocalMsgId.CCMSGConnectionRecvFailure; } }

        public String ExceptionInfo { get; set; }
    }
}

