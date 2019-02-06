namespace StreamDeckLib.Messages
{
  public static class PropertyInspectorEventPayloadExtensions
  {
	public static bool PayloadHasProperty(this PropertyInspectorEventPayload obj, string propertyName)
	{
	  if (obj == null || obj.payload == null)
	  {
		return false;
	  }
	  return obj.payload.GetType().GetProperty(propertyName) != null;
	}
  }
}
