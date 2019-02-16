using StreamDeckLib.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamDeckLib
{
	partial class ConnectionManager
	{

		// Cheer ramblinggeek 100 January 21, 2019

		private static readonly Dictionary<string, Func<BaseStreamDeckAction, StreamDeckEventPayload, Task>> _EventDictionary
		= new Dictionary<string, Func<BaseStreamDeckAction, StreamDeckEventPayload, Task>>()
		{
			["keyDown"] = (action, args) => action.OnKeyDown(args),
			["keyUp"] = (action, args) => action.OnKeyUp(args),
			["willAppear"] = (action, args) => action.OnWillAppear(args),
			["willDisappear"] = (action, args) => action.OnWillDisappear(args),
			["titleParametersDidChange"] = (action, args) => action.OnTitleParametersDidChange(args),
			["deviceDidConnect"] = (action, args) => action.OnDeviceDidConnect(args),
			["deviceDidDisconnect"] = (action, args) => action.OnDeviceDidDisconnect(args),
			["applicationDidLaunch"] = (action, args) => action.OnApplicationDidLaunch(args),
			["applicationDidTerminate"] = (action, args) => action.OnApplicationDidTerminate(args),
		};

	= new Dictionary<string, Func<BaseStreamDeckAction, StreamDeckEventPayload, Task>>()
	{
	  ["keyDown"] = (plugin, args) => plugin.OnKeyDown(args),
	  ["keyUp"] = (plugin, args) => plugin.OnKeyUp(args),
	  ["willAppear"] = (plugin, args) => plugin.OnWillAppear(args),
	  ["willDisappear"] = (plugin, args) => plugin.OnWillDisappear(args),
	  ["titleParametersDidChange"] = (plugin, args) => plugin.OnTitleParametersDidChange(args),
	  ["deviceDidConnect"] = (plugin, args) => plugin.OnDeviceDidConnect(args),
	  ["deviceDidDisconnect"] = (plugin, args) => plugin.OnDeviceDidDisconnect(args),
	  ["applicationDidLaunch"] = (plugin, args) => plugin.OnApplicationDidLaunch(args),
	  ["applicationDidTerminate"] = (plugin, args) => plugin.OnApplicationDidTerminate(args),
	  ["propertyInspectorDidAppear"] = (plugin, args) => plugin.OnPropertyInspectorDidAppear(args),
	  ["propertyInspectorDidDisappear"] = (plugin, args) => plugin.OnPropertyInspectorDidDisappear(args),
	  ["didReceiveSettings"] = (plugin, args) => plugin.OnDidReceiveSettings(args),
		["didReceiveGlobalSettings"] = (plugin, args) => plugin.OnDidReceiveGlobalSettings(args)
	};

  }
		private static readonly string[] _ActionEventsIgnore = new[] {
		};

		private static readonly Dictionary<string, Func<BaseStreamDeckAction, PropertyInspectorEventPayload, Task>> _PropertyInspectorActionDictionary
	= new Dictionary<string, Func<BaseStreamDeckAction, PropertyInspectorEventPayload, Task>>()
	{
		["propertyInspectorConnected"] = (action, args) => action.OnPropertyInspectorConnected(args),
		["propertyInspectorDisconnected"] = (action, args) => action.OnPropertyInspectorDisconnected(args),
		["sendToPlugin"] = (action, args) => action.OnPropertyInspectorMessageReceived(args)
	};

	}

}
