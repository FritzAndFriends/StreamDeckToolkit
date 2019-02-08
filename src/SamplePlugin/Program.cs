using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using StreamDeckLib;
using System;
using System.Diagnostics;
using System.IO;
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

	  Debugger.Launch();

	  while (!Debugger.IsAttached)
	  {
		await Task.Delay(100);
	  }

#endif
	  using (var source = new CancellationTokenSource())
	  {
		using (var loggerFactory = GetLoggerFactory())
		{
		  try
		  {

			// codingbandit: I had to take out the using statement as it was causing the dispose method to be called (and disposing the socket connection)
			await ConnectionManager.Initialize(args, loggerFactory)
				.SetPlugin(new MySamplePlugin())
				.StartAsync(source.Token);

			Console.ReadLine();

		  }
		  catch (Exception ex)
		  {
			TopLogger.LogError(ex, "Error while running the plugin");
		  }
		  source.Cancel();
		}

	  }
	}
  }
}
