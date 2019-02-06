using StreamDeckLib;
using StreamDeckLib.Messages;
using System.Threading.Tasks;

namespace SamplePlugin
{
  internal class MySamplePlugin : BaseStreamDeckPlugin
  {

	// Cheer 200 kevin_downs Jan 11, 2019
	// Cheer 401 cpayette Jan 15, 2019
	// Cheer 2501 themikejolley Jan 15, 2019
	// Cheer 100 wolfgang_blitz Jan 15, 2019
	// Cheer 157 jongalloway Jan 15, 2019
	// Cheer 100 johanb Jan 15, 2019
	// Cheer 400 faniereynders Jan 15, 2019
	// Cheer 100 TomMcQ Jan 15, 2019
	// Cheer 361 Crazy240sx Jan 15, 2019
	// Cheer 600 yarrgh Jan 15, 2019
	// Cheer 1030 kulu83 Jan 15, 2019
	// Cheer 2500 Auth0Bobby Jan 15, 2019

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
		await Manager.SetImageAsync(args.context, "images/Fritz.png");
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
	  if (args.PayloadHasProperty("starting_number"))
	  {
		_Counter = (int)args.payload.starting_number;
		await Manager.SetTitleAsync(args.context, _Counter.ToString());
	  }
	 
	}

  }
}
