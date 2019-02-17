using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using StreamDeckLib.Messages;
using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace StreamDeckLib
{

	/// <summary>
	/// This class manages the connection to the StreamDeck hardware
	/// </summary>
	public partial class ConnectionManager : IDisposable
	{
		private int _Port;
		private string _Uuid;
		private string _RegisterEvent;
		private IStreamDeckProxy _Proxy;

		private ConnectionManager()
		{
		}

		public Messages.Info Info { get; private set; }

		public static ConnectionManager Initialize(string[] commandLineArgs,
																							 ILoggerFactory loggerFactory = null,
																							 IStreamDeckProxy streamDeckProxy = null)
		{
			using (var app = new CommandLineApplication())
			{
				app.HelpOption();

				var optionPort = app.Option<int>("-port|--port <PORT>",
																				 "The port the Elgato StreamDeck software is listening on",
																				 CommandOptionType.SingleValue);

				var optionPluginUUID = app.Option("-pluginUUID <UUID>",
																					"The UUID that the Elgato StreamDeck software knows this plugin as.",
																					CommandOptionType.SingleValue);

				var optionRegisterEvent = app.Option("-registerEvent <REGEVENT>", "The registration event",
																						 CommandOptionType.SingleValue);

				var optionInfo = app.Option("-info <INFO>", "Some information", CommandOptionType.SingleValue);

				var optionBreak = app.Option("-break", "Attach the debugger", CommandOptionType.NoValue);

				app.Parse(commandLineArgs);

				try
				{
					return Initialize(optionPort.ParsedValue, optionPluginUUID.Values[0], optionRegisterEvent.Values[0],
														optionInfo.Values[0], loggerFactory,
														streamDeckProxy ?? new StreamDeckProxy());
				}
				catch
				{
					throw new ArgumentException($"{nameof(commandLineArgs)} must be the commandline args that the StreamDeck application calls this program with.");
				}
			}
		}

		private static ConnectionManager Initialize(int port, string uuid,
																								string registerEvent, string info,
																								ILoggerFactory loggerFactory, IStreamDeckProxy streamDeckProxy)
		{
			// TODO: Validate the info parameter
			var myInfo = JsonConvert.DeserializeObject<Messages.Info>(info);

			_LoggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
			_Logger = loggerFactory?.CreateLogger("ConnectionManager") ?? NullLogger.Instance;

			var manager = new ConnectionManager()
			{
				_Port = port,
				_Uuid = uuid,
				_RegisterEvent = registerEvent,
				Info = myInfo,
				_Proxy = streamDeckProxy
			};

			return manager;
		}

		public async Task<ConnectionManager> StartAsync(CancellationToken token)
		{

			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

			await Run(token);

			return this;

		}

		public async Task<ConnectionManager> StartAsync()
		{

			var source = new CancellationTokenSource();

			return await this.StartAsync(source.Token);
		}

		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			throw new NotImplementedException();
		}

		private async Task Run(CancellationToken token)
		{

			await _Proxy.ConnectAsync(new Uri($"ws://localhost:{_Port}"), token);
			await _Proxy.Register(_RegisterEvent, _Uuid);

			var keepRunning = true;

			while (!token.IsCancellationRequested && keepRunning)
			{
				// Exit loop if the socket is closed or aborted
				switch (_Proxy.State)
				{
					case WebSocketState.CloseReceived:
					case WebSocketState.Closed:
					case WebSocketState.Aborted:
						keepRunning = false;

						break;
				}

				if (!keepRunning) break;

				var jsonString = await _Proxy.GetMessageAsString(token);

				if (!string.IsNullOrEmpty(jsonString) && !jsonString.StartsWith("\0"))
				{
					try
					{
						var msg = JsonConvert.DeserializeObject<StreamDeckEventPayload>(jsonString);

						if (msg == null)
						{
							_Logger.LogError($"Unknown message received: {jsonString}");

							continue;
						}

						if (_ActionEventsIgnore.Contains(msg.Event)) { continue; }

						// Make sure we have a registered BaseStreamDeckAction instance registered for the received action (UUID)
						if (!_ActionsDictionary.ContainsKey(msg.action))
						{
							_Logger.LogWarning($"The action requested (\"{msg.action}\") was not found as being registered with the plugin");
						}

						var action = _ActionsDictionary[msg.action];


						//property inspector payload
						if (msg.Event == "sendToPlugin")
						{
							var piMsg = JsonConvert.DeserializeObject<PropertyInspectorEventPayload>(jsonString);
							if (piMsg.PayloadHasProperty("property_inspector"))
							{
								//property inspector event
								var piEvent = piMsg.GetPayloadValue<string>("property_inspector");
								if (!_PropertyInspectorActionDictionary.ContainsKey(piEvent))
								{
									_Logger.LogWarning($"Plugin does not handle the Property Inspector event '{piEvent}'");
									continue;
								}
								else
								{
									_PropertyInspectorActionDictionary[piEvent]?.Invoke(action, piMsg);
									continue;

								}

							}

							//property inspector property value event
							_PropertyInspectorActionDictionary[piMsg.Event]?.Invoke(action, piMsg);
							continue;
						}

						if (!_EventDictionary.ContainsKey(msg.Event))
						{
							_Logger.LogWarning($"Plugin does not handle the event '{msg.Event}'");

							continue;
						}

						_EventDictionary[msg.Event]?.Invoke(action, msg);

					}
					catch (Exception ex)
					{
						_Logger.LogError(ex, "Error while processing payload from StreamDeck");
					}
				}

				await Task.Delay(100);
			}

			Dispose();
		}

		#region StreamDeck Methods

		public async Task SetTitleAsync(string context, string newTitle)
		{
			var args = new SetTitleArgs()
			{
				context = context,
				payload = new SetTitleArgs.Payload
				{
					title = newTitle,
					TargetType = SetTitleArgs.TargetType.HardwareAndSoftware
				}
			};

			await _Proxy.SendStreamDeckEvent(args);
		}

		public async Task SetImageAsync(string context, string imageLocation)
		{

			var imgString = Convert.ToBase64String(File.ReadAllBytes(imageLocation), Base64FormattingOptions.None);

			var args = new SetImageArgs
			{
				context = context,
				payload = new SetImageArgs.Payload
				{
					TargetType = SetTitleArgs.TargetType.HardwareAndSoftware,
					image = $"data:image/{new FileInfo(imageLocation).Extension.ToLowerInvariant().Substring(1)};base64, {imgString}"
				}
			};

			await _Proxy.SendStreamDeckEvent(args);
		}

		public async Task ShowAlertAsync(string context)
		{
			var args = new ShowAlertArgs()
			{
				context = context
			};

			await _Proxy.SendStreamDeckEvent(args);
		}

		public async Task ShowOkAsync(string context)
		{
			var args = new ShowOkArgs()
			{
				context = context
			};

			await _Proxy.SendStreamDeckEvent(args);
		}

		public async Task SetSettingsAsync(string context, dynamic value)
		{
			var args = new SetSettingsArgs()
			{
				context = context,
				payload = value
			};

			await _Proxy.SendStreamDeckEvent(args);
		}

		public async Task SetStateAsync(string context, int state)
		{
			var args = new SetStateArgs
			{
				context = context,
				payload = new SetStateArgs.Payload
				{
					state = state
				}
			};

			await _Proxy.SendStreamDeckEvent(args);
		}

		public async Task SendToPropertyInspectorAsync(string context, dynamic payload)
		{

			var args = new SendToPropertyInspectorArgs
			{
				action = _Uuid,
				context = context,
				payload = payload
			};

			await _Proxy.SendStreamDeckEvent(args);
		}

		public async Task SwitchToProfileAsync(string context, string device, string profileName)
		{

			var args = new SwitchToProfileArgs
			{
				context = context,
				device = device,
				payload = new SwitchToProfileArgs.Payload
				{
					profile = profileName
				}
			};

			await _Proxy.SendStreamDeckEvent(args);
		}

		public async Task OpenUrlAsync(string context, string url)
		{
			var args = new OpenUrlArgs()
			{
				context = context,
				payload = new OpenUrlArgs.Payload()
				{
					url = url
				}
			};

			await _Proxy.SendStreamDeckEvent(args);
		}

		#endregion

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls
		private static ILoggerFactory _LoggerFactory;
		private static ILogger _Logger;

		void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_Proxy.Dispose();
				}

				disposedValue = true;
			}
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			//GC.SuppressFinalize(this);
		}

		#endregion
	}

}
