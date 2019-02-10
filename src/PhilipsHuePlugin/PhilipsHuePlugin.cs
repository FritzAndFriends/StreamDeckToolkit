using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Interfaces;
using StreamDeckLib;
using StreamDeckLib.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhilipsHuePlugin
{
  public class PhilipsHuePlugin : BaseStreamDeckPlugin
  {
		private const string HUEHUBIP_PROPERTYNAME = "hueHubIp";
		private const string APPUSERID_PROPERTYNAME = "appUserId";
		private const string COLORHEX_PROPERTYNAME = "colorHex";
		private const string LIGHTINDEX_PROPERTYNAME = "lightIndex";

		private string _hueHubIp { get; set; }
		private string _appUserId { get; set; }
		private string _colorHex { get; set; }
		private int _lightIndex { get; set; }

	public override async Task OnKeyUp(StreamDeckEventPayload args)
	{
	  ILocalHueClient client = new LocalHueClient(_hueHubIp);
	  client.Initialize(_appUserId);
	  var cmd = new LightCommand();
	  cmd.TurnOn().SetColor(new RGBColor(_colorHex));
	  await client.SendCommandAsync(cmd, new List<string> { _lightIndex.ToString() });
	}

	public override Task OnDidReceiveSettings(StreamDeckEventPayload args)
		{
			if (args.payload != null && args.payload.settings != null && args.payload.settings.settingsModel != null)
			{
				if (args.PayloadSettingsHasProperty(HUEHUBIP_PROPERTYNAME))
				{
					_hueHubIp = args.GetPayloadSettingsValue<string>(HUEHUBIP_PROPERTYNAME);
				}
				if (args.PayloadSettingsHasProperty(APPUSERID_PROPERTYNAME))
				{
					_appUserId = args.GetPayloadSettingsValue<string>(APPUSERID_PROPERTYNAME);
				}
				if (args.PayloadSettingsHasProperty(COLORHEX_PROPERTYNAME))
				{
					_colorHex = args.GetPayloadSettingsValue<string>(COLORHEX_PROPERTYNAME);
				}
				if (args.PayloadSettingsHasProperty(LIGHTINDEX_PROPERTYNAME))
				{
					_lightIndex = args.GetPayloadSettingsValue<int>(LIGHTINDEX_PROPERTYNAME);
				}
			}
			return Task.CompletedTask;
		}

		public override async Task OnWillAppear(StreamDeckEventPayload args)
		{
			//request instance settings
			await Manager.GetSettingsAsync(args.context);
		}

  }
}
