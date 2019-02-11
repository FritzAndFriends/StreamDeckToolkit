using StreamDeckLib;

namespace PhilipsHuePlugin
{

  public class PhilipsHuePlugin : BaseStreamDeckPlugin
  {
		public override void RegisterActionTypes()
		{
			base.RegisterActionType(ActionUuidConstants.CUSTOM_COLOR_ACTION, typeof(PhilipsHueCustomAction));
		}
  }
}
