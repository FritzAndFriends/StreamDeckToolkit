using StreamDeckLib;
using System.Threading.Tasks;

namespace SamplePlugin
{

	class Program
	{

		// Cheer 200 careypayette February 14, 2019
		// Cheer 100 roberttables February 14, 2019
		// Cheer 100 careypayette February 15, 2019
		// Cheer 100 devlead 15/2/2019

		static async Task Main(string[] args)
		{

			using (var config = StreamDeckLib.Config.ConfigurationBuilder.BuildDefaultConfiguration(args))
			{

				await ConnectionManager.Initialize(args, config.LoggerFactory)
															 .RegisterAction(new MySampleAction())
															 .RegisterAction(new Mysecon())
															 .StartAsync();

			}

		}
	}

}
