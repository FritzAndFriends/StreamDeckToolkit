using StreamDeckLib;
using StreamDeckLib.Messages;
using System.Threading.Tasks;

namespace SamplePlugin
{
	internal class MySamplePlugin : IStreamDeckPlugin
	{

		// Cheer 200 kevin_downs Jan 11, 2019

		public ConnectionManager Manager { get; set; }

		public Task OnKeyDown(string action, string context, StreamDeckEventPayload.Payload payload, string device)
		{
			return Task.CompletedTask;
		}

		private static int _Counter = 0;

		public async Task OnKeyUp(string action, string context, StreamDeckEventPayload.Payload payload, string device)
		{
			_Counter++;
			await Manager.SetTitleAsync(context, _Counter.ToString());
		}

		public Task OnWillAppear(string action, string context, StreamDeckEventPayload.Payload payload, string device)
		{
			return Task.CompletedTask;
		}

		public Task OnWillDisappear(string action, string context, StreamDeckEventPayload.Payload payload, string device)
		{
			return Task.CompletedTask;
		}
	}
}