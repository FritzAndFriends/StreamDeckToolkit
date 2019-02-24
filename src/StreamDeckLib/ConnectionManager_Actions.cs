using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StreamDeckLib
{
	partial class ConnectionManager
	{
	//string is the action UUID, type is the class type of the action
	private Dictionary<string, Type> _actions = new Dictionary<string, Type>();
	//string is the context, there will only ever be one action per context
	private Dictionary<string, BaseStreamDeckAction> _contextActions = new Dictionary<string, BaseStreamDeckAction>();

	//Cheer 100 svavablount 15/2/19 
	public ConnectionManager RegisterActionType(string actionUuid, Type actionType)
	{
	  if (string.IsNullOrWhiteSpace(actionUuid)){
		throw new IncompleteActionDefinitionException("");
	  }
		if(_actions.ContainsKey(actionUuid))
	  {
		throw new DuplicateActionRegistrationException(actionUuid);
	  }
	  _actions.Add(actionUuid, actionType);
	  return this;
	}

	public BaseStreamDeckAction GetInstanceOfAction(string context, string actionUuid)
	{
	  //see if context exists, if so, return the associated action
	  if (_contextActions.Any(x => x.Key.Equals(context)))
	  {
		return _contextActions[context];
	  }
	  else
	  {
		//see if we have a recorded type for the action
		if (_actions.Any(x => x.Key.Equals(actionUuid)))
		{
		  var t = _actions[actionUuid];
		  var action = Activator.CreateInstance(t) as BaseStreamDeckAction;
		  action.Manager = this;
		  action.Logger = _LoggerFactory.CreateLogger(action.ActionUuid);
		  _contextActions.Add(context, action);
		  return action;
		}

	  }
	  return null;
	}

	public ConnectionManager RegisterAllActions(Assembly assembly)
	{
	  var actions = assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(ActionUuidAttribute)));
	  foreach (var actionType in actions)
	  {
		var attr = actionType.GetCustomAttributes(typeof(ActionUuidAttribute), true).FirstOrDefault() as ActionUuidAttribute;
		this.RegisterActionType(attr.Uuid, actionType);
	  }
	  return this;
	}

  }
}
