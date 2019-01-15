using StreamDeckLib.Messages;
using System.Threading.Tasks;

namespace StreamDeckLib
{
    public class BaseStreamDeckPlugin
	{
		protected internal ConnectionManager Manager { get; set; }
        
        public virtual Task OnKeyDown(StreamDeckEventPayload args) => Task.CompletedTask;
        public virtual Task OnKeyUp(StreamDeckEventPayload args) => Task.CompletedTask;
        public virtual Task OnWillAppear(StreamDeckEventPayload args) => Task.CompletedTask;
        public virtual Task OnWillDisappear(StreamDeckEventPayload args) => Task.CompletedTask;
        public virtual Task OnTitleParametersDidChange(StreamDeckEventPayload args) => Task.CompletedTask;
    }
}