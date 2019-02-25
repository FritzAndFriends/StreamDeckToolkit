using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Globalization;
namespace StreamDeckLib
{

	public partial class ActionManager : IDisposable
	{
		#region Type and instance properties

		private static readonly IEqualityComparer<string> _StringEqualityComparer = StringComparer.Create(CultureInfo.InvariantCulture, true);

		private readonly Dictionary<string, Type> _Actions;

		private readonly Dictionary<string, BaseStreamDeckAction> _ActionInstances;

		private readonly ILogger _Logger;

		#endregion


		#region Constructors

		private ActionManager()
		{
			this._Actions = new Dictionary<string, Type>(_StringEqualityComparer);
			this._ActionInstances = new Dictionary<string, BaseStreamDeckAction>(_StringEqualityComparer);
		}


		public ActionManager(ILogger logger) : this()
		{
			this._Logger = logger ?? NullLoggerFactory.Instance.CreateLogger(nameof(ActionManager));
		}

		#endregion


		#region Action registration methods

				public ActionManager RegisterActionInstance(BaseStreamDeckAction actionInstance)
		{
			if (null == actionInstance)
			{
				throw new ArgumentNullException(nameof(actionInstance), $"No instance of a {nameof(BaseStreamDeckAction)} derived class was specified to be registered.");
			}

			this._Logger.LogDebug($"Logging an instance of {actionInstance.GetType().FullName} without a named key. Using the action's UUID.");
			return this.RegisterActionInstance(actionInstance.ActionUuid, actionInstance);
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
			where TActionType : BaseStreamDeckAction
		{
			this._Actions.Add(actionUUID, typeof(TActionType));
			return this;
		}

		public ActionManager RegisterAllActions(Assembly actionsAssembly)
		{
			throw new NotImplementedException();
			return this;
		}

		#endregion


		#region Action instance creation and retrieval

		public TActionType GetActionInstance<TActionType>(string actionUUID)
			where TActionType : BaseStreamDeckAction
		{
			if (this._Actions.ContainsKey(actionUUID))
			{
				return Activator.CreateInstance(this._Actions[actionUUID]) as TActionType;
			}

			throw new ActionNotRegisteredException(actionUUID);
		}



		public bool IsActionRegistered(string actionUUID)
		{
			return this._Actions.ContainsKey(actionUUID);
		}

		/// <summary>
		/// Gets a new instance of an <seealso cref="BaseStreamDeckAction"/> registered with a UUID
		/// of <paramref name="actionUUID"/>. This instance <strong>will not</strong> be stored or managed,
		/// and cannot be reused.
		/// </summary>
		/// <returns>A new instance of the action</returns>
		/// <param name="actionUUID">The UUID of the action type which is to be instantiated</param>
		/// <remarks>
		/// Throws a <exception cref="ActionNotRegisteredException" /> exception if there is no action with
		/// a UUID of <paramref name="actionUUID"/> registered.
		/// </remarks>
		public BaseStreamDeckAction GetAction(string actionUUID)
		{
			if (this._ActionInstances.ContainsKey(actionUUID))
			{
				return this._ActionInstances[actionUUID];
			}

			throw new ActionNotRegisteredException(actionUUID);
		}

		#endregion


		#region Specific action instance creation, registration, and retrieval



		#endregion


		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ActionManager() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		#endregion

	}
}
