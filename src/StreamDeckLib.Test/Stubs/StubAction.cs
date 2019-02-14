namespace StreamDeckLib.Test
{

	public class StubAction : BaseStreamDeckAction
	{

		public StubAction() : this(string.Empty)
		{
			
		}

		public StubAction(string uuid)
		{
			this._uuid = uuid;
		}
		
		private         string _uuid;
		public override string UUID => this._uuid;


		public StubAction SetUUID(string uuid)
		{
			this._uuid = uuid;
			
			return this;
		}



	}

}