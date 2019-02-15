using StreamDeckLib;

namespace _StreamDeckPlugin_
{
  internal class $(PluginName) : BaseStreamDeckPlugin
  {
	public override void RegisterActionTypes()
	{
	  this.RegisterActionType("$(UUID)", typeof($(PluginName)Action));
	}
  }
}
