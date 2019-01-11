using StreamDeckLib;
using StreamDeckLib.Messages;

namespace SamplePlugin
{
	internal class MySamplePlugin : IStreamDeckPlugin
	{
		public ConnectionManager Manager { get; set; }

		public void OnKeyDown(string action, string context, StreamDeckEventPayload.Payload payload, string device)
		{
			throw new System.NotImplementedException();
		}

		public void OnKeyUp(string action, string context, StreamDeckEventPayload.Payload payload, string device)
		{
			throw new System.NotImplementedException();
		}

		public void OnWillAppear(string action, string context, StreamDeckEventPayload.Payload payload, string device)
		{
			throw new System.NotImplementedException();
		}

		public void OnWillDisappear(string action, string context, StreamDeckEventPayload.Payload payload, string device)
		{
			throw new System.NotImplementedException();
		}
	}
}