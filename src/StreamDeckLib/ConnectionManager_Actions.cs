using System;
using System.Collections.Generic;

namespace StreamDeckLib
{
	partial class ConnectionManager
	{
		private static readonly Dictionary<string, BaseStreamDeckAction> _ActionsDictionary = new Dictionary<string, BaseStreamDeckAction>();

		[Obsolete("This method is obsolete, and has been replaced with the \"RegisterAction()\" method. Update your code to make use of this new method.", false)]
		public ConnectionManager SetPlugin(BaseStreamDeckAction plugin) => this.RegisterAction(plugin);

		public ConnectionManager RegisterAction(BaseStreamDeckAction action) => RegisterActionInternal(this, action);

		private static ConnectionManager RegisterActionInternal(ConnectionManager manager, BaseStreamDeckAction action)
		{

			// Cheer 100 svavablount 15/2/19 

			ValidateActionForRegistration(action);

			action.Manager = manager;
			action.Logger = _LoggerFactory.CreateLogger(action.ActionUUID);

			_ActionsDictionary.Add(action.RegistrationKey, action);

			return manager;
		}

		private static void ValidateActionForRegistration(BaseStreamDeckAction action)
		{
			ValidateAction(action);

			if (IsActionRegistered(action.RegistrationKey))
			{
				throw new DuplicateActionRegistrationException(action.ActionUUID);
			}

		}

		private static void ValidateAction(BaseStreamDeckAction action)
		{
			if (null == action)
			{
				throw new ArgumentNullException(nameof(action), "No action instance was given to register.");
			}

			if (string.IsNullOrWhiteSpace(action.RegistrationKey))
			{
				throw new IncompleteActionDefinitionException($"The action of type \"{action}\" does not define a valid UUID.");
			}
		}

		private static bool IsActionRegistered(string actionUUID) => _ActionsDictionary.ContainsKey(actionUUID.ToLowerInvariant());

		private static BaseStreamDeckAction GetRegisteredActionByUUID(string actionUUID)
		{
			if (!IsActionRegistered(actionUUID))
			{
				throw new ActionNotRegisteredException(actionUUID);
			}

			return _ActionsDictionary[actionUUID.ToLowerInvariant()];
		}
	}
}
