using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StreamDeckLib
{
	partial class ConnectionManager
	{
		private ActionManager _ActionManager;

		//Cheer 100 svavablount 15/2/19 
		public ConnectionManager RegisterActionType(string actionUuid, Type actionType)
		{
			this._ActionManager.RegisterActionType(actionUuid, actionType);
			return this;
		}

		public BaseStreamDeckAction GetInstanceOfAction(string context, string actionUuid)
		{
			return this._ActionManager.GetActionForContext(context, actionUuid);
		}

		public ConnectionManager RegisterAllActions(Assembly assembly)
		{
			this._ActionManager.RegisterAllActions(assembly);
			return this;
		}

	}
}
