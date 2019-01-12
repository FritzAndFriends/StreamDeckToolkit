using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Settings.Configuration;
using StreamDeckLib;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SamplePlugin
{
	class Program
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

            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

			var loggerFactory = new LoggerFactory()
				.AddSerilog(Log.Logger);

			var topLogger = loggerFactory.CreateLogger("top");

			try
			{
				await ConnectionManager.Initialize(Port, PluginUUID, RegisterEvent, Info, loggerFactory)
					.SetPlugin(new MySamplePlugin())
					.StartAsync(source.Token);
				
				Console.ReadLine();	

			} catch (Exception ex) {
				topLogger.LogError(ex, "Error while running the plugin");
			}

			source.Cancel();

		}
	}
}
