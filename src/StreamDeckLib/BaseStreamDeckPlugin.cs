using System;
using System.Collections.Generic;
using System.Linq;

namespace StreamDeckLib
{
  public abstract class BaseStreamDeckPlugin
  {
	protected internal ConnectionManager Manager { get; set; }
	//string is the action UUID, type is the class type of the action
	private Dictionary<string, Type> _actions = new Dictionary<string, Type>();
	//string is the context, there will only ever be one action per context
	private Dictionary<string, BaseStreamDeckAction> _contextActions = new Dictionary<string, BaseStreamDeckAction>();

	public abstract void RegisterActionTypes();

	public void RegisterActionType(string actionUuid, Type actionType)
	{
	  _actions.Add(actionUuid, actionType);
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
		  action.Manager = this.Manager;
		  _contextActions.Add(context, action);
		  return action;
		}

	  }
	  return null;
	}

  }
}
