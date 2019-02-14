using System;
using System.ComponentModel;

namespace StreamDeckLib.Messages
{
	public static class PropertyInspectorEventPayloadExtensions
	{
	  return obj.payload.settingsModel[propertyName] != null;
	}


	public static T GetPayloadValue<T>(this PropertyInspectorEventPayload obj, string propertyName) where T: IConvertible
	{
	  if (obj.PayloadHasProperty(propertyName))
	  {
		return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.payload.settingsModel[propertyName].Value);
	  }
	  return default(T);

  }
}
