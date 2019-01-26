using StreamDeckLib.Messages;
using System;
using System.Threading.Tasks;

namespace StreamDeckLib
{
	public abstract class BaseStreamDeckPlugin
	{
		protected internal ConnectionManager Manager { get; set; }

		public virtual Task OnKeyDown(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnKeyUp(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnWillAppear(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnWillDisappear(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnTitleParametersDidChange(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnDeviceDidConnect(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnDeviceDidDisconnect(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnApplicationDidLaunch(StreamDeckEventPayload args) => Task.CompletedTask;

		public virtual Task OnApplicationDidTerminate(StreamDeckEventPayload args) => Task.CompletedTask;
	}
}
