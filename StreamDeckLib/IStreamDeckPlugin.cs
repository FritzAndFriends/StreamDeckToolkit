using StreamDeckLib.Messages;

namespace StreamDeckLib
{
	public interface IStreamDeckPlugin
	{
		ConnectionManager Manager { get; set; }

		void OnKeyDown(string action, string context, StreamDeckEventPayload.Payload payload, string device);
		void OnKeyUp(string action, string context, StreamDeckEventPayload.Payload payload, string device);
		void OnWillAppear(string action, string context, StreamDeckEventPayload.Payload payload, string device);
		void OnWillDisappear(string action, string context, StreamDeckEventPayload.Payload payload, string device);
	}
}