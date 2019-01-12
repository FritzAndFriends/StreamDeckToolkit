using StreamDeckLib;
using StreamDeckLib.Messages;
using System.Threading.Tasks;

namespace SamplePlugin
{
	internal class MySamplePlugin : BaseStreamDeckPlugin
	{

		// Cheer 200 kevin_downs Jan 11, 2019

		private static int _Counter = 0;

		public override async Task OnKeyUp(StreamDeckEventPayload args)
		{
			_Counter++;
			await Manager.SetTitleAsync(args.context, _Counter.ToString());
		}

	}
}