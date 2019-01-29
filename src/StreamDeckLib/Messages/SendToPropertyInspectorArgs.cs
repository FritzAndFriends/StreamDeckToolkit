namespace StreamDeckLib.Messages
{
	public class SendToPropertyInspectorArgs : BaseStreamDeckArgs
	{

		public override string Event => "sendToPropertyInspector";

		public string action { get; set; }

		public dynamic payload { get; set; }

	}

}
