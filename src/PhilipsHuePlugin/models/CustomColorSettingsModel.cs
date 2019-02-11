namespace PhilipsHuePlugin.models
{
  public class CustomColorSettingsModel : BaseSettingsModel
  {
	public string colorHex { get; set; } = string.Empty;
	public int lightIndex { get; set; } = 1;
	public override bool IsValid()
	{
	  return base.IsValid() && !string.IsNullOrEmpty(colorHex);
	}
  }
}
