using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using StreamDeckLib.Messages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
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
		private readonly ClientWebSocket _Socket = new ClientWebSocket();

		private ConnectionManager() { }

		public Messages.Info Info { get; private set; }

		public static ConnectionManager Initialize(string[] commandLineArgs, ILoggerFactory loggerFactory = null)
		{
			using (var app = new CommandLineApplication())
			{
				app.HelpOption();
				// UNTESTED. I don't have a streamdeck device and don't know how to "click" the button in the app.
				var optionPort = app.Option<int>("-port|--port <PORT>", "The port the Elgato StreamDeck software is listening on", CommandOptionType.SingleValue);
				var optionPluginUUID = app.Option("-pluginUUID <UUID>", "The UUID that the Elgato StreamDeck software knows this plugin as.", CommandOptionType.SingleValue);
				var optionRegisterEvent = app.Option("-registerEvent <REGEVENT>", "The registration event", CommandOptionType.SingleValue);
				var optionInfo = app.Option("-info <INFO>", "Some information", CommandOptionType.SingleValue);

#if DEBUG
				var optionDebug = app.Option<bool>("-d|--debug", "Wait for a debugger to attach before execution", CommandOptionType.SingleOrNoValue);
#endif

				app.Parse(commandLineArgs);

#if DEBUG
				var waitForDebugger = optionDebug.HasValue() && optionDebug.ParsedValue;
#endif

				if (app.GetValidationResult() != ValidationResult.Success)
				{
					app.ShowHelp();
					throw new ArgumentException("The command line parameters were not valid or could not be parsed.");
				}

				try
				{
					return Initialize(optionPort.ParsedValue, optionPluginUUID.Value(), optionRegisterEvent.Value(), optionInfo.Value(), loggerFactory, waitForDebugger);
				}
				catch
				{
					throw new ArgumentException($"{nameof(commandLineArgs)} must be the commandline args that the StreamDeck application calls this program with.");
				}
			}
		}

		public static ConnectionManager Initialize(int port, string uuid, string registerEvent, string info, ILoggerFactory loggerFactory, bool waitForDebugger = false)

		{
			// TODO: Validate the info parameter
			var myInfo = JsonConvert.DeserializeObject<Messages.Info>(info);

			_LoggerFactory = loggerFactory;
			_Logger = loggerFactory?.CreateLogger("ConnectionManager") ?? NullLogger.Instance;

			var manager = new ConnectionManager()
			{
				_Port = port,
				_Uuid = uuid,
				_RegisterEvent = registerEvent,
				Info = myInfo,
				_waitForDebugger = waitForDebugger,
			};

			return manager;
		}

		public ConnectionManager SetPlugin(BaseStreamDeckPlugin plugin)
		{
			this._Plugin = plugin;
			plugin.Manager = this;
			return this;
		}

		public async Task<ConnectionManager> StartAsync(CancellationToken token)
		{
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

			await Task.Factory.StartNew(() => Run(token), TaskCreationOptions.LongRunning);

			return this;
		}

		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			throw new NotImplementedException();
		}

		private async Task Run(CancellationToken token)
		{
			if (_waitForDebugger)
			{
				Debugger.Launch();

				// Wait for a debugger to attach, but if the token gets a cancelation request, we need to exit.
				while (!Debugger.IsAttached | !token.IsCancellationRequested)
				{
					await Task.Delay(100);
				}

				// If we exited because the token cancellation has been requested, return before doing any socket-related operations
				if (token.IsCancellationRequested)
				{
					return;
				}
			}

			await _Socket.ConnectAsync(new Uri($"ws://localhost:{_Port}"), token);

			await _Socket.SendAsync(GetPluginRegistrationBytes(), WebSocketMessageType.Text, true, CancellationToken.None);

			var keepRunning = true;

			while (!token.IsCancellationRequested)
			{
				// Exit loop if the socket is closed or aborted
				switch (_Socket.State)
				{

					case WebSocketState.CloseReceived:
					case WebSocketState.Closed:
					case WebSocketState.Aborted:
						keepRunning = false;
						break;

				}
				if (!keepRunning) break;

				var jsonString = await GetMessageAsString(token);

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
						if (!_ActionDictionary.ContainsKey(msg.Event))
						{
							_Logger.LogWarning($"Plugin does not handle the event '{msg.Event}'");
							continue;
						}
						_ActionDictionary[msg.Event]?.Invoke(_Plugin, msg);
					}
					catch (Exception ex)
					{

						_Logger.LogError(ex, "Error while processing payload from StreamDeck");

					}
				}

				await Task.Delay(100);

			}
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

			await SendStreamDeckEvent(args);
		}

		public async Task SetSettingsAsync(string context, dynamic value)
		{
			var args = new SetSettingsArgs()
			{
				context = context,
				payload = value
			};
			await SendStreamDeckEvent(args);
		}

		public async Task ShowAlertAsync(string context)
		{
			var args = new ShowAlertArgs()
			{
				context = context
			};
			await SendStreamDeckEvent(args);
		}

		public async Task ShowOkAsync(string context)
		{
			var args = new ShowOkArgs()
			{
				context = context
			};
			await SendStreamDeckEvent(args);
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
			await SendStreamDeckEvent(args);
		}

		#endregion

		private Task SendStreamDeckEvent(BaseStreamDeckArgs args)
		{
			var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(args));
			return _Socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
		}

		private async Task<string> GetMessageAsString(CancellationToken token)
		{
			byte[] buffer = new byte[65536];
			var segment = new ArraySegment<byte>(buffer, 0, buffer.Length);
			await _Socket.ReceiveAsync(segment, token);
			var jsonString = Encoding.UTF8.GetString(buffer);
			return jsonString;
		}

		private ArraySegment<byte> GetPluginRegistrationBytes()
		{
			var registration = new Messages.Info.PluginRegistration
			{
				@event = _RegisterEvent,
				uuid = _Uuid
			};

			var outString = JsonConvert.SerializeObject(registration);
			var outBytes = Encoding.UTF8.GetBytes(outString);

			return new ArraySegment<byte>(outBytes);
		}

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls
		private BaseStreamDeckPlugin _Plugin;
		private static ILoggerFactory _LoggerFactory;
		private static ILogger _Logger;
		private bool _waitForDebugger;

		void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_Socket.Dispose();
					_LoggerFactory.Dispose();
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
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
