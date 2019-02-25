using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Globalization;
using System.Linq;

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

		/// <summary>
		/// Initializes a new instance of the <see cref="T:StreamDeckLib.ActionManager"/> class.
		/// </summary>
		/// <param name="logger">An instance of a logger (<see cref="ILogger"/> class.</param>
		public ActionManager(ILogger logger = null) : this()
		{
			this._Logger = logger ?? NullLoggerFactory.Instance.CreateLogger(nameof(ActionManager));
		}

		#endregion


		#region Action registration methods


		/// <summary>
		/// Registers an action type for a given UUID.
		/// </summary>
		/// <returns>The instance of <seealso cref="ActionManager"/>.</returns>
		/// <param name="actionUUID">The UUID for the action.</param>
		/// <typeparam name="TActionType">The type to be registered for the <paramref name="actionUUID"/>. Must inherit from <seealso cref="BaseStreamDeckAction"/>.</typeparam>
		public ActionManager RegisterAction<TActionType>(string actionUUID)
			where TActionType : BaseStreamDeckAction
		{
			this._Actions.Add(actionUUID, typeof(TActionType));
			return this;
		}


		/// <summary>
		/// Registers an action type for a given UUID.
		/// </summary>
		/// <returns>The instance of <seealso cref="ActionManager"/>.</returns>
		/// <param name="actionUUID">The UUID for the action.</param>
		/// <param name="actionType">The type to be registered for the <paramref name="actionUUID"/>. Must inherit from <seealso cref="BaseStreamDeckAction"/>.</param>
		/// <remarks>If the <paramref name="actionType" /> does not inherit from <seealso cref="BaseStreamDeckAction"/>,
		/// a <seealso cref="TypeDoesNotInheritFromBaseStreamDeckAction"/> exception is thrown.</remarks>
		public ActionManager RegisterActionType(string actionUUID, Type actionType)
		{
			// Ensure that the type we're registering inherits from BaseStreamDeckAction. 
			if (!actionType.IsSubclassOf(typeof(BaseStreamDeckAction)))
			{
				throw new TypeDoesNotInheritFromBaseStreamDeckAction(actionType.Name, actionType.FullName, actionType.Assembly.FullName);
			}

			this._Actions.Add(actionUUID, actionType);
			return this;
		}


		/// <summary>
		/// Registers all actions which are decorated with an <seealso cref="ActionUuidAttribute"/>
		/// withn a given <seealso cref="Assembly"/>.
		/// </summary>
		/// <returns>The instance of <seealso cref="ActionManager"/>.</returns>
		/// <param name="actionsAssembly">The <seealso cref="Assembly"/> from within which actions are to be registered.</param>
		public ActionManager RegisterAllActions(Assembly actionsAssembly)
		{

			var actions = actionsAssembly.GetTypes().Where(type => Attribute.IsDefined(type, typeof(ActionUuidAttribute)));
			foreach (var actionType in actions)
			{
				var attr = actionType.GetCustomAttributes(typeof(ActionUuidAttribute), true).FirstOrDefault() as ActionUuidAttribute;
				this.RegisterActionType(attr.Uuid, actionType);
			}
			return this;


			return this;
		}

		#endregion


		#region Action instance creation and retrieval

		/// <summary>
		/// Gets an instance of an action from an action UUID.
		/// </summary>
		/// <returns>A new instance of an action.</returns>
		/// <param name="actionUUID">The UUID of the action type which is requested.</param>
		/// <typeparam name="TActionType">The specific type of action to be returned.</typeparam>
		/// <remarks>If no type is found to match the <paramref name="actionUUID"/>, an exception
		/// of type <seealso cref="ActionNotRegisteredException"/> is thrown.</remarks>
		public TActionType GetActionInstance<TActionType>(string actionUUID)
			where TActionType : BaseStreamDeckAction
		{
			if (this._Actions.ContainsKey(actionUUID))
			{
				return Activator.CreateInstance(this._Actions[actionUUID]) as TActionType;
			}

			throw new ActionNotRegisteredException(actionUUID);
		}


		/// <summary>
		/// Checks whether an action is registered for a given UUID.
		/// </summary>
		/// <returns><c>true</c>, if action registered was registered, <c>false</c> otherwise.</returns>
		/// <param name="actionUUID">The UUID to check</param>
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


		#region Specific action instance creation, registration, and retrieval (by Stream Deck context)


		/// <summary>
		/// Registers the action instance.
		/// </summary>
		/// <returns>The action instance.</returns>
		/// <param name="instanceKey">Instance key.</param>
		/// <param name="actionInstance">Action instance.</param>
		public ActionManager RegisterActionInstance(string instanceKey, BaseStreamDeckAction actionInstance)
		{
			this._Logger.LogDebug($"Registering an instance of {actionInstance.GetType().FullName} with a key of \"{instanceKey}\".");
			if (this._ActionInstances.ContainsKey(instanceKey))
			{
				throw new DuplicateActionInstanceRegistrationException(instanceKey);
			}

			this._ActionInstances.Add(instanceKey, actionInstance);

			return this;
		}



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
