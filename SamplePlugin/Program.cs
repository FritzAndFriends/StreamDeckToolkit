using McMaster.Extensions.CommandLineUtils;
using StreamDeckLib;
using System;
using System.Threading;
using System.Threading.Tasks;

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

			await ConnectionManager.Initialize(Port, PluginUUID, RegisterEvent, Info)
				.SetPlugin(new MySamplePlugin())
				.StartAsync(source.Token);

			Console.ReadLine();
			source.Cancel();


		}
	}
}
