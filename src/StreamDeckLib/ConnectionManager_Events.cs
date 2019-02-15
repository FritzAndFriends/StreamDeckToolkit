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

		private static readonly string[] _ActionEventsIgnore = new[] {
			"deviceDidConnect", "deviceDidDisconnect", "applicationDidLaunch", "applicationDidTerminate"
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
