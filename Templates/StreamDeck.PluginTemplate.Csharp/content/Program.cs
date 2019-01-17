using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Serilog;
using StreamDeckLib;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace _StreamDeckPlugin_
{
	public class Program
	{
		static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Program>(args);


		[Option(Description = "The port the Elgato StreamDeck software is listening on", ShortName = "port")]
		public int Port { get; set; }

		[Option(ShortName = "pluginUUID")]
		public string PluginUUID { get; set; }

		[Option(ShortName = "registerEvent")]
		public string RegisterEvent { get; set; }

		[Option(ShortName = "info")]
		public string Info { get; set; }

		private async Task OnExecuteAsync()
		{
			var source = new CancellationTokenSource();
			var logLocation = Path.Combine(Path.GetTempPath(), $"{GetType().Assembly.GetName().Name}-.log");

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.Enrich
					.FromLogContext()
				.WriteTo
					.File(logLocation, rollingInterval: RollingInterval.Day)
				.CreateLogger();

			var loggerFactory = new LoggerFactory()
				.AddSerilog(Log.Logger);

			var topLogger = loggerFactory.CreateLogger("top");

			try
			{
				await ConnectionManager.Initialize(Port, PluginUUID, RegisterEvent, Info, loggerFactory)
					.SetPlugin(new $(PluginName))())
					.StartAsync(source.Token);
				
				Console.ReadLine();	

			} catch (Exception ex) {
				topLogger.LogError(ex, "Error while running the plugin: $(PluginName)");
			}

			source.Cancel();

		}
	}
}
