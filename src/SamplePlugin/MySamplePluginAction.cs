using SamplePlugin.Models;
using StreamDeckLib;
using StreamDeckLib.Messages;
using System.Threading.Tasks;

namespace SamplePlugin
{
	[ActionUuid(Uuid = "com.csharpfritz.samplePlugin.action")]
	public class MySamplePluginAction : BaseStreamDeckActionWithSettingsModel<CounterSettingsModel>
	{

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
