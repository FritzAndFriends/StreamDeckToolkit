using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Settings.Configuration;
using StreamDeckLib;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace _StreamDeckPlugin_
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

										await ConnectionManager.Initialize(args, loggerFactory)
																					.RegisterAction(new DefaultPluginAction())
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
