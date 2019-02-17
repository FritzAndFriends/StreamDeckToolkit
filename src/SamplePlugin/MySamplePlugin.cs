using StreamDeckLib;

namespace SamplePlugin
{
	internal class MySamplePlugin : StreamDeckPlugin
	{

		public MySamplePlugin(ConnectionManager manager) : base(manager)
		{

		}

		public override void RegisterActionTypes()
		{
			this.RegisterActionType("com.csharpfritz.samplePlugin.action", typeof(MySamplePluginAction));
		}
	}
}
