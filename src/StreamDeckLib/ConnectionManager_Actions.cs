using System;
using System.Linq;
using System.Reflection;

namespace StreamDeckLib
{
  partial class ConnectionManager
  {
	public ConnectionManager RegisterAllActions(Assembly assembly)
	{
	  var actions = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(ActionUuidAttribute)));
	  foreach (var actionType in actions)
	  {
		var attr = actionType.GetType().GetCustomAttributes(typeof(ActionUuidAttribute), true).FirstOrDefault() as ActionUuidAttribute;
		this._Plugin.RegisterActionType(attr.Uuid, actionType);
	  }
	  return this;
	}
  }
}
