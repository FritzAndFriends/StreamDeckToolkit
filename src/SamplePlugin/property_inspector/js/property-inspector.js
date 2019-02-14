// global websocket, used to communicate from/to Stream Deck software
// as well as some info about our plugin, as sent by Stream Deck software 
var websocket = null,
  uuid = null,
  inInfo = null,
  actionInfo = {},
  settingsModel = {
		counter: 0
  };

function connectSocket(inPort, inUUID, inRegisterEvent, inInfo, inActionInfo) {
  uuid = inUUID;
  actionInfo = JSON.parse(inActionInfo);
  inInfo = JSON.parse(inInfo);
  websocket = new WebSocket('ws://localhost:' + inPort);

  websocket.onopen = function () {
		var json = { event: inRegisterEvent, uuid: inUUID };
		// register property inspector to Stream Deck
		websocket.send(JSON.stringify(json));
		sendEventToPlugin('propertyInspectorConnected');
  };
}

window.addEventListener('unload', function (event) {
  sendEventToPlugin('propertyInspectorDisconnected');
});

function sendValueToPlugin(value, param) {
  if (websocket) {
		settingsModel[param] = value;
		const json = {
			"action": actionInfo['action'],
			"event": "sendToPlugin",
			"context": uuid,
			"payload": {
				"settingsModel": settingsModel
			}
		};
		websocket.send(JSON.stringify(json));
	}
}

function sendEventToPlugin(value) {
  if (websocket) {
	settingsModel[param] = value;
	const json = {
	  "action": actionInfo['action'],
	  "event": "sendToPlugin",
	  "context": uuid,
	  "payload": {
		  "property_inspector": value
	  }
	};
	websocket.send(JSON.stringify(json));
  }
}


