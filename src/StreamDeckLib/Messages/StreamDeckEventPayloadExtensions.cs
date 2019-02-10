using System;
using System.ComponentModel;

namespace StreamDeckLib.Messages
{
  public static class StreamDeckEventPayloadExtensions
  {
	public static bool PayloadSettingsHasProperty(this StreamDeckEventPayload obj, string propertyName)
	{
	  return obj.payload.settings[propertyName] != null;
	}


	public static T GetPayloadSettingsValue<T>(this StreamDeckEventPayload obj, string propertyName) where T : IConvertible
	{
	  if (obj.PayloadSettingsHasProperty(propertyName))
	  {
		return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.payload.settings[propertyName].Value.ToString());
	  }
	  return default(T);
	}

  }
}
