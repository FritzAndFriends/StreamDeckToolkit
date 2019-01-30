﻿using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using StreamDeckLib.Messages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
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
		private IStreamDeckProxy _Proxy;
        private ConnectionManager() { }
        public Messages.Info Info { get; private set; }
        public static ConnectionManager Initialize(string[] commandLineArgs,
				ILoggerFactory loggerFactory = null, IStreamDeckProxy streamDeckProxy = null)
        {
			var waitForDebugger = false;
            using (var app = new CommandLineApplication())
            {
                app.HelpOption();

                var optionPort = app.Option<int>("-port|--port <PORT>", "The port the Elgato StreamDeck software is listening on", CommandOptionType.SingleValue);
                var optionPluginUUID = app.Option("-pluginUUID <UUID>", "The UUID that the Elgato StreamDeck software knows this plugin as.", CommandOptionType.SingleValue);
                var optionRegisterEvent = app.Option("-registerEvent <REGEVENT>", "The registration event", CommandOptionType.SingleValue);
                var optionInfo = app.Option("-info <INFO>", "Some information", CommandOptionType.SingleValue);

#if DEBUG
				var optionDebug = app.Option<bool>("-d|--debug", "Wait for a debugger to attach before execution", CommandOptionType.NoValue);
#endif

                app.Parse(commandLineArgs);

				// Validate that the command line parameters are valid, and display the help if they aren't.
				if (app.GetValidationResult() != ValidationResult.Success)
				{
					app.ShowHelp();
					throw new ArgumentException("The command line parameters were not valid or could not be parsed.");
				}
				
#if DEBUG
				waitForDebugger = optionDebug.HasValue() && optionDebug.ParsedValue;
#endif
				
                try
                {
                    return Initialize(optionPort.ParsedValue, optionPluginUUID.Values[0], optionRegisterEvent.Values[0], optionInfo.Values[0], loggerFactory, streamDeckProxy ?? new StreamDeckProxy(), waitForDebugger);
                }
                catch
                {
					app.ShowHelp();
					throw new ArgumentException($"{nameof(commandLineArgs)} must be the commandline args that the StreamDeck application calls this program with.", nameof(commandLineArgs));
                }
            }
        }

        private static ConnectionManager Initialize(int port, string uuid, string registerEvent, string info, ILoggerFactory loggerFactory, IStreamDeckProxy streamDeckProxy, bool waitForDebugger = false)
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
				_Proxy = streamDeckProxy,
				_WaitForDebugger = waitForDebugger,
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
			await AttachDebugger(token);

			// If a cancellation has been requested, let's not try to
			// connect before we exit.
			if (token.IsCancellationRequested)
			{
				return;
			}
			
			await _Proxy.ConnectAsync(new Uri($"ws://localhost:{_Port}"), token);
			await _Proxy.Register(_RegisterEvent, _Uuid);
			

            var keepRunning = true;

            while (!token.IsCancellationRequested)
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
            Dispose();
        }

		
		/// <summary>
		/// Waits in a loop for a debugger to attach to the process. Will return if either
		/// a debugger instance is attached or a cancellation has been requested through
		/// the <paramref name="token">cancellation token</paramref>.
		/// </summary>
		/// <param name="token">A cancellation token which can be used to break out of the wait loop.</param>
		/// <returns>A value indicating whether a debugger was attached (<c>true</c>) or not (<c>false</c>).</returns>
		private async Task<bool> AttachDebugger(CancellationToken token)
		{
			if (!_WaitForDebugger)
			{
				return true;
			}

			Console.Write("Attach a debugger to resume execution...");
			
			Debugger.Launch();

			// Wait for a debugger to attach, but if the token gets a cancelation request, we need to exit.
			while (!Debugger.IsAttached | !token.IsCancellationRequested)
			{
				// Polling every 1/2 second should be enough, rather than every 1/10th of a second.
				// We could make this a parameter at a later time, but some validation would be needed
				// to cap the minimum and maximum time allowed.
				await Task.Delay(500, token); 
			}

			// If we exited because the token cancellation has been requested, return before doing any socket-related operations
			return !token.IsCancellationRequested;
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
        private BaseStreamDeckPlugin _Plugin;
        private static ILoggerFactory _LoggerFactory;
        private static ILogger _Logger;
		private bool _WaitForDebugger;

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
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
