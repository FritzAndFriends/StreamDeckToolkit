using StreamDeckLib.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamDeckLib
{
    partial class ConnectionManager
	{

		private static readonly Dictionary<string, Func<BaseStreamDeckPlugin, StreamDeckEventPayload, Task>> _ActionDictionary
		= new Dictionary<string, Func<BaseStreamDeckPlugin, StreamDeckEventPayload, Task>>() {

			{ "keyDown", (plugin, args) => plugin.OnKeyDown(args) },
			{ "keyUp", (plugin, args) => plugin.OnKeyUp(args)},
			{ "willAppear", (plugin, args) => plugin.OnWillAppear(args)},
			{ "willDisappear", (plugin, args) => plugin.OnWillDisappear(args)},
			{ "titleParametersDidChange", (plugin, args) => plugin.OnTitleParametersDidChange(args)}
	};



	}
}
