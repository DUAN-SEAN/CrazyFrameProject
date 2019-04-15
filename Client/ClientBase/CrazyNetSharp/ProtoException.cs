using System;

namespace BlackJack.LibClient
{
	public class ProtoException : Exception
	{
		public ProtoException (String sDesc) : base(sDesc)
		{
		}
	}
}

