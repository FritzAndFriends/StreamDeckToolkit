using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using StreamDeckLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SamplePlugin
{

	class Program
	{
		static ILoggerFactory GetLoggerFactory()
		{
			var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			Directory.SetCurrentDirectory(dir);

			var configuration = new ConfigurationBuilder()
			                    .AddJsonFile("appsettings.json")
			                    .Build();

			Log.Logger = new LoggerConfiguration()
			             .ReadFrom.Configuration(configuration)
			             .CreateLogger();

			var loggerFactory = new LoggerFactory()
				.AddSerilog(Log.Logger);

			TopLogger = loggerFactory.CreateLogger("top");

			TopLogger.LogInformation("Plugin started");

			return loggerFactory;
		}

		private static Microsoft.Extensions.Logging.ILogger TopLogger;

		static async Task Main(string[] args)
		{

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
					await Task.Delay(500);
					Console.Write(".");
				}
			}
#endif

				using (var loggerFactory = GetLoggerFactory())
				{
					try
					{
						// codingbandit: I had to take out the using statement as it was causing the dispose method to be called (and disposing the socket connection)
						await ConnectionManager.Initialize(args, loggerFactory)
                                   .RegisterAction(new MySampleAction())
						                       .StartAsync();

					}
					catch (Exception ex)
					{
						TopLogger.LogError(ex, "Error while running the plugin");
					}

			}

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
