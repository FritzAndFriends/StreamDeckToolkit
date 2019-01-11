using StreamDeckLib.Messages;
using System.Threading.Tasks;

namespace StreamDeckLib
{
	public interface IStreamDeckPlugin
	{
		ConnectionManager Manager { get; set; }

		Task OnKeyDown(string action, string context, StreamDeckEventPayload.Payload payload, string device);
		Task OnKeyUp(string action, string context, StreamDeckEventPayload.Payload payload, string device);
		Task OnWillAppear(string action, string context, StreamDeckEventPayload.Payload payload, string device);
		Task OnWillDisappear(string action, string context, StreamDeckEventPayload.Payload payload, string device);
	}
}