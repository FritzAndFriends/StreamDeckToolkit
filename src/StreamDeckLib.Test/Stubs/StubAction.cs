using System;

namespace StreamDeckLib.Test
{

	[ActionUuid(Uuid = "Test UUID")]
	public class StubAction : BaseStreamDeckAction
	{
		internal ConnectionManager GetConnectionManager()
		{
			return this.Manager;
		}
	}

}
