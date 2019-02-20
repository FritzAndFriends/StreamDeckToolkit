using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
namespace StreamDeckLib
{
  internal partial class ActionManager
  {
		private readonly Dictionary<string, Type> _Actions = new Dictionary<string, Type>();

		private readonly Dictionary<string, BaseStreamDeckAction> _ActionInstances = new Dictionary<string, BaseStreamDeckAction>();

		private readonly ILogger _Logger;

		private ActionManager()
		{
			this._Actions = new Dictionary<string, Type>();
			this._ActionInstances = new Dictionary<string, BaseStreamDeckAction>();
		}

		public ActionManager(ILogger logger) : this()
		{
			this._Logger = logger ?? NullLoggerFactory.Instance.CreateLogger(nameof(ActionManager));
		}

		public ActionManager RegisterActionInstance(BaseStreamDeckAction actionInstance)
		{
			if (null == actionInstance)
			{
				throw new ArgumentNullException(nameof(actionInstance), $"No instance of a {nameof(BaseStreamDeckAction)} derived class was specified to be registered.");
			}

			this._Logger.LogDebug($"Logging an instance of {actionInstance.GetType().FullName} without a named key. Using the action's UUID.");
			return this.RegisterActionInstance(actionInstance.UUID, actionInstance);
		}

		public ActionManager RegisterActionInstance(string actionKey, BaseStreamDeckAction actionInstance)
		{
			this._Logger.LogDebug($"Registering an instance of {actionInstance.GetType().FullName} with UUID/key of \"{actionKey}\".");
			if (this._ActionInstances.ContainsKey(actionKey))
			{
				throw new DuplicateActionRegistrationException(actionKey);
			}

			this._ActionInstances.Add(actionKey, actionInstance);

			return this;
		}

		public ActionManager RegisterAction<TActionType>(string actionUUID)
			where TActionType: BaseStreamDeckAction
		{
			this._Actions.Add(actionUUID, typeof(TActionType));
			return this;
		}

		public ActionManager RegisterAllActions(Assembly actionsAssembly)
		{
			throw new NotImplementedException();
			return this;
		}

  }
}
