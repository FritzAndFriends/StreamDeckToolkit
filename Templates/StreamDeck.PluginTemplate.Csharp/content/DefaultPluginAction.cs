using StreamDeckLib;
using StreamDeckLib.Messages;
using System.Dynamic;
using System.Threading.Tasks;

namespace _StreamDeckPlugin_
{

	// This value must match the UUID for the action in the manifest.json
	// file, so that it can be called from the Stream Deck.
	[ActionUuid(Uuid = "$(UUID).DefaultPluginAction")]
	internal class DefaultPluginAction : BaseStreamDeckAction
	{
		private static int _Counter = 0;
		private static bool _IsPropertyInspectorConnected = false;

		public override async Task OnKeyUp(StreamDeckEventPayload args)
		{
			_Counter++;
			await Manager.SetTitleAsync(args.context, _Counter.ToString());

			if (_Counter % 10 == 0)
			{
				await Manager.ShowAlertAsync(args.context);
			}
			else if (_Counter % 15 == 0)
			{
				await Manager.OpenUrlAsync(args.context, "https://www.bing.com");
			}
			else if (_Counter % 3 == 0)
			{
				await Manager.ShowOkAsync(args.context);
			}
			else if (_Counter % 7 == 0)
			{
				await Manager.SetImageAsync(args.context, "Fritz.png");
			}
		}

		public override async Task OnWillAppear(StreamDeckEventPayload args)
		{
			if (args.payload != null && args.payload.settings != null && args.payload.settings.counter != null)
			{
				_Counter = args.payload.settings.counter;
			}
			await Manager.SetTitleAsync(args.context, _Counter.ToString());
		}

		public override async Task OnWillDisappear(StreamDeckEventPayload args)
		{

			var settings = new { counter = _Counter };

			await Manager.SetSettingsAsync(args.context, settings);
		}

		public override Task OnPropertyInspectorConnected(PropertyInspectorEventPayload args)
		{
	  		_IsPropertyInspectorConnected = true;
	  		return Task.CompletedTask;
		}

		public override Task OnPropertyInspectorDisconnected(PropertyInspectorEventPayload args)
		{
		  _IsPropertyInspectorConnected = false;
		  return Task.CompletedTask;
		}

		public async override Task OnPropertyInspectorMessageReceived(PropertyInspectorEventPayload args)
		{
		  if (args.SettingsPayloadHasProperty("starting_number"))
		  {
			_Counter = args.GetSettingsPayloadValue<int>("starting_number");
			await Manager.SetTitleAsync(args.context, _Counter.ToString());
		  }

		}
	}
}
