using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CONFIG = Microsoft.Extensions.Configuration;

namespace StreamDeckLib.Config
{
	public class ConfigurationBuilder : IDisposable
	{

		private ConfigurationBuilder(string[] args) {

#if DEBUG
			// This gives us our "first chance" debugging, before even parsing the command
			// line args, without the need to manually edit the code to toggle the feature
			// ability on or off.

			if (args.Select(arg => arg.Replace("--", "-"))
							.Any(arg => arg.Equals("-break")))
			{
				Console.WriteLine("Debugging has been requested. Waiting for a debugger to attach...");
				Debugger.Launch();

				while (!Debugger.IsAttached)
				{
					Task.Delay(500).GetAwaiter().GetResult();
					Console.Write(".");
				}
			}
#endif
		}
		private static ConfigurationBuilder Instance;

		public static ConfigurationBuilder BuildDefaultConfiguration(string[] args) {

			var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			Directory.SetCurrentDirectory(dir);

			var configuration = new CONFIG.ConfigurationBuilder()
													.AddJsonFile("appsettings.json")
													.Build();

			Log.Logger = new LoggerConfiguration()
									 .ReadFrom.Configuration(configuration)
									 .CreateLogger();

			Instance = new ConfigurationBuilder(args)
			{
				LoggerFactory = new LoggerFactory()
				.AddSerilog(Log.Logger)
			};
			
			return Instance;

		}

		public ILoggerFactory LoggerFactory { get; private set; }


		public void Dispose()
		{

		#if DEBUG
			if (Debugger.IsAttached)
			{
				// If a debugger is attached, give the developer a last chance to inspect
				// variables, state, etc. before the process terminates.
				Debugger.Break();
			}
#endif

		}
	}
}
