using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamDeck_First
{

	public enum Destination {

		HARDWARE_AND_SOFTWARE = 0,
		HARDWARE_ONLY = 1,
		SOFTWARE_ONLY = 2

	}

	class Program
	{

		private static readonly Dictionary<string, Action<StreamDeckEventPayload>> _EventDictionary = new Dictionary<string, Action<StreamDeckEventPayload>> {
			{ "keyDown", Program.KeyDown }
		};

		public static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Program>(args);

		[Option(Description = "The port the Elgato StreamDeck software is listening on", ShortName ="port")]
		public string Port { get; set; }

		[Option(ShortName ="pluginUUID")]
		public string PluginUUID { get; set; }

		[Option(ShortName = "registerEvent")]
		public string RegisterEvent { get; set; }

		[Option(ShortName = "info")]
		public string Info { get; set; }

		private StreamDeckInfo _Info = null;
		internal StreamDeckInfo StreamDeckInfo
		{
			get
			{

				if (_Info == null)
				{
					// Parse it
					_Info = JsonConvert.DeserializeObject<StreamDeckInfo>(Info);
				}

				return _Info;

			}
		}

		private static ClientWebSocket _Socket = new ClientWebSocket();
		private async Task OnExecuteAsync()
		{

			Console.WriteLine($"Attempting to connect to port:'{Port}'");

			while (!Debugger.IsAttached) {
				Thread.Sleep(100);
			}


			await _Socket.ConnectAsync(new Uri($"ws://localhost:{Port}"), CancellationToken.None);

			await _Socket.SendAsync(GetPluginRegistrationBytes(), WebSocketMessageType.Text, true, CancellationToken.None);

			var buffer = new byte[] { };
			var tokenSource = new CancellationTokenSource();

			while (!tokenSource.IsCancellationRequested)
			{



				// Tommcq cheered 310 on Jan. 8, 2019
				// poeticAndroid cheered 100 on Jan 8, 2019
				// kevin_downs cheered 200 on Jan 8, 2019

				await _Socket.ReceiveAsync(buffer, tokenSource.Token);
				var jsonString = UTF8Encoding.UTF8.GetString(buffer);

				if (!string.IsNullOrEmpty(jsonString))
				{
					var payload = JsonConvert.DeserializeObject<StreamDeckEventPayload>(jsonString);
					_EventDictionary[payload.action]?.Invoke(payload);
				}

			}

		}


		private static int _Counter = 0;

		private static void KeyDown(StreamDeckEventPayload args)
		{

			_Counter++;
			SetTitle(args.context, _Counter.ToString());


		//	_Socket.

		}

		/// <summary>
		/// Set the title on the button passed in the context
		/// </summary>
		/// <param name="context"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		private static Task SetTitle(string context, string title) {

			var json = @"{
					""event"": ""setTitle"",
					""context"": " + context + @",
					""payload"": {
						""title"": """ + title + @""",
						""target"": " + (int)Destination.HARDWARE_AND_SOFTWARE + @"
					}
				}";

			var bytes = UTF8Encoding.UTF8.GetBytes(json);
			return _Socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);

		}


		private ArraySegment<byte> GetPluginRegistrationBytes()
		{

			var registration = new PluginRegistration
			{
				@event = RegisterEvent,
				uuid = PluginUUID
			};

			var outString = JsonConvert.SerializeObject(registration);
			var outBytes = UTF8Encoding.UTF8.GetBytes(outString);

			return outBytes;

		}
	}

}
