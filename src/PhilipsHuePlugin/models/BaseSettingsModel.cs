namespace PhilipsHuePlugin.models
{
  public class BaseSettingsModel
  {
	public string hueHubIp { get; set; } = string.Empty;
	public string appUserId { get; set; } = string.Empty;
	public virtual bool IsValid()
	{
	  return !(string.IsNullOrEmpty(hueHubIp) || string.IsNullOrEmpty(appUserId));
	}
  }
}
