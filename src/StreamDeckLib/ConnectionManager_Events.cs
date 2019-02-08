using StreamDeckLib.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamDeckLib
{
  partial class ConnectionManager
  {

	// Cheer ramblinggeek 100 January 21, 2019

	private static readonly Dictionary<string, Func<BaseStreamDeckPlugin, StreamDeckEventPayload, Task>> _ActionDictionary
	= new Dictionary<string, Func<BaseStreamDeckPlugin, StreamDeckEventPayload, Task>>()
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
	};

	private static readonly Dictionary<string, Func<BaseStreamDeckPlugin, PropertyInspectorEventPayload, Task>> _PropertyInspectorActionDictionary
= new Dictionary<string, Func<BaseStreamDeckPlugin, PropertyInspectorEventPayload, Task>>()
{
  ["propertyInspectorConnected"] = (plugin, args) => plugin.OnPropertyInspectorConnected(args),
  ["propertyInspectorDisconnected"] = (plugin, args) => plugin.OnPropertyInspectorDisconnected(args),
  ["sendToPlugin"] = (plugin, args) => plugin.OnPropertyInspectorMessageReceived(args)
};

  }
}
