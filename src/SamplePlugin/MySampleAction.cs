using Microsoft.Extensions.Logging;
using SamplePlugin.Models;
using Serilog.Core;
using StreamDeckLib;
using StreamDeckLib.Messages;
using System;
using System.Threading.Tasks;

namespace SamplePlugin
{
	[ActionUuid(Uuid = "com.csharpfritz.samplePlugin.action")]
	internal class MySampleAction : BaseStreamDeckAction
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

		private SampleActionSettingsModel _settingsModel = new SampleActionSettingsModel();
		private bool _IsPropertyInspectorConnected = false;

		public override async Task OnKeyUp(StreamDeckEventPayload args)
		{

			// Cheer 342 cpayette 15/2/19
			// Cheer 100 devlead 15/2/19 

			Logger.LogTrace($"Button pressed: {args}");

			_settingsModel.Counter++;
			await Manager.SetTitleAsync(args.context, _settingsModel.Counter.ToString());

			if (_settingsModel.Counter % 10 == 0)
			{
				await Manager.ShowAlertAsync(args.context);
			}
			else if (_settingsModel.Counter % 15 == 0)
			{
				await Manager.OpenUrlAsync(args.context, "https://www.bing.com");
			}
			else if (_settingsModel.Counter % 3 == 0)
			{
				await Manager.ShowOkAsync(args.context);
			}
			else if (_settingsModel.Counter == 0)
			{
				await Manager.SetImageAsync(args.context, "images/Fritz.png");
			}

	  await Manager.SendToPropertyInspectorAsync(args.context, _settingsModel);
		}

		public override async Task OnWillAppear(StreamDeckEventPayload args)
		{
	 
		if (args.PayloadSettingsHasProperty("Counter"))
		{
		  _settingsModel.Counter = args.GetPayloadSettingsValue<int>("Counter");
		}
			await Manager.SetTitleAsync(args.context, _settingsModel.Counter.ToString());
		}

		public override async Task OnWillDisappear(StreamDeckEventPayload args)
		{
			await Manager.SetSettingsAsync(args.context, _settingsModel);
		}

		public override async Task OnPropertyInspectorConnected(PropertyInspectorEventPayload args)
		{
			_IsPropertyInspectorConnected = true;
			await Manager.SendToPropertyInspectorAsync(args.context, _settingsModel);
		}

		public override Task OnPropertyInspectorDisconnected(PropertyInspectorEventPayload args)
		{
			_IsPropertyInspectorConnected = false;
			return Task.CompletedTask;
		}

		public override async Task OnPropertyInspectorMessageReceived(PropertyInspectorEventPayload args)
		{
			if (args.SettingsPayloadHasProperty("Counter"))
			{
				_settingsModel.Counter = args.GetSettingsPayloadValue<int>("Counter");
				
			}
		 await Manager.SetTitleAsync(args.context, _settingsModel.Counter.ToString());

	}
	}
}
