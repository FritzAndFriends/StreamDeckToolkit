using PhilipsHuePlugin.models;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Interfaces;
using StreamDeckLib;
using StreamDeckLib.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhilipsHuePlugin
{
  public class PhilipsHueGreenAction : BaseStreamDeckActionWithSettingsModel<GreenSettingsModel>
  {
	public override string ActionUuid => ActionUuidConstants.GREEN_COLOR_ACTION;
	public override async Task OnKeyUp(StreamDeckEventPayload args)
	{
	  if (SettingsModel.IsValid())
	  {
		ILocalHueClient client = new LocalHueClient(SettingsModel.hueHubIp);
		client.Initialize(SettingsModel.appUserId);
		var cmd = new LightCommand();
		cmd.TurnOn().SetColor(new RGBColor("#00FF00"));
		await client.SendCommandAsync(cmd, new List<string> { SettingsModel.lightIndex.ToString() });
	  }
	}

	public override async Task OnWillAppear(StreamDeckEventPayload args)
	{
	  await this.Manager.GetSettingsAsync(args.context);
	}

  }
}
