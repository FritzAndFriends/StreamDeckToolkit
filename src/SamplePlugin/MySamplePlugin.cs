using StreamDeckLib;

namespace SamplePlugin
{
  internal class MySamplePlugin : BaseStreamDeckPlugin
  {
	public override void RegisterActionTypes()
	{
	  this.RegisterActionType("com.csharpfritz.samplePlugin.action", typeof(MySamplePluginAction));
	}
  }
}
