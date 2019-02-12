using System;
using System.ComponentModel;

namespace StreamDeckLib.Messages
{
	public static class PropertyInspectorEventPayloadExtensions
	{
		public static bool PayloadHasProperty(this PropertyInspectorEventPayload obj, string propertyName)
		{
			return obj.payload[propertyName] != null;
		}


		public static T GetPayloadValue<T>(this PropertyInspectorEventPayload obj, string propertyName) where T : IConvertible
		{
			if (obj.PayloadHasProperty(propertyName))
			{
				return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.payload[propertyName].Value);
			}
			return default(T);
		}
	}
}
