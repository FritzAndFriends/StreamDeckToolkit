using StreamDeckLib;

namespace PhilipsHuePlugin
{

  public class PhilipsHuePlugin : BaseStreamDeckPlugin
  {
	public override void RegisterActionTypes()
	{
	  base.RegisterActionType(ActionUuidConstants.CUSTOM_COLOR_ACTION, typeof(PhilipsHueCustomAction));
	  base.RegisterActionType(ActionUuidConstants.RED_COLOR_ACTION, typeof(PhilipsHueRedAction));
	  base.RegisterActionType(ActionUuidConstants.GREEN_COLOR_ACTION, typeof(PhilipsHueGreenAction));
	}
  }
}
