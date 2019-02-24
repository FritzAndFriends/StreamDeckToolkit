using SamplePlugin.Models;
using StreamDeckLib;
using StreamDeckLib.Messages;
using System.Threading.Tasks;

namespace SamplePlugin
{
	[ActionUuid(Uuid = "com.csharpfritz.samplePlugin.action")]
  public class MySamplePluginAction : BaseStreamDeckActionWithSettingsModel<CounterSettingsModel>
  {
	// Cheer 342 cpayette 15/2/19
	// Cheer 100 devlead 15/2/19 
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

	public override async Task OnKeyUp(StreamDeckEventPayload args)
	{
	  SettingsModel.Counter++;
	  await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());

	  if (SettingsModel.Counter % 10 == 0)
	  {
		await Manager.ShowAlertAsync(args.context);
	  }
	  else if (SettingsModel.Counter % 15 == 0)
	  {
		await Manager.OpenUrlAsync(args.context, "https://www.bing.com");
	  }
	  else if (SettingsModel.Counter % 3 == 0)
	  {
		await Manager.ShowOkAsync(args.context);
	  }
	  else if (SettingsModel.Counter % 7 == 0)
	  {
		await Manager.SetImageAsync(args.context, "images/Fritz.png");
	  }

	  //update settings
	  await Manager.SetSettingsAsync(args.context, SettingsModel);
	}

	public override async Task OnDidReceiveSettings(StreamDeckEventPayload args)
	{
	  await base.OnDidReceiveSettings(args);
	  await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
	}

	public override async Task OnWillAppear(StreamDeckEventPayload args)
	{
	  await base.OnWillAppear(args);
	  await Manager.SetTitleAsync(args.context, SettingsModel.Counter.ToString());
	}

  }
}
