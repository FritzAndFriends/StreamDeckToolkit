using StreamDeckLib.Messages;
using System;
using System.Threading.Tasks;

namespace StreamDeckLib
{
	public class BaseStreamDeckPlugin
	{

		protected internal ConnectionManager Manager { get; set; }

		public virtual Task OnKeyDown(StreamDeckEventPayload args) {

			return Task.CompletedTask;

		}
		
		public virtual Task OnKeyUp(StreamDeckEventPayload args) {

			return Task.CompletedTask;

		}
		
		
		public virtual Task OnWillAppear(StreamDeckEventPayload args) {

			return Task.CompletedTask;

		}
		public virtual Task OnWillDisappear(StreamDeckEventPayload args) {

			return Task.CompletedTask;

		}

		public virtual Task OnTitleParametersDidChange(StreamDeckEventPayload args)
		{
			return Task.CompletedTask;
		}
	}
}