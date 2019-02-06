// global websocket, used to communicate from/to Stream Deck software
// as well as some info about our plugin, as sent by Stream Deck software 
var websocket = null,
  uuid = null,
	inInfo = null,
  actionInfo = {};

function connectSocket(inPort, inUUID, inRegisterEvent, inInfo, inActionInfo) {
  uuid = inUUID;
  actionInfo = JSON.parse(inActionInfo);
  inInfo = JSON.parse(inInfo);
  websocket = new WebSocket('ws://localhost:' + inPort);

  websocket.onopen = function () {
		var json = { event: inRegisterEvent, uuid: inUUID };
		// register property inspector to Stream Deck
		websocket.send(JSON.stringify(json));
		sendValueToPlugin('propertyInspectorConnected', 'property_inspector');
  };
}

window.addEventListener('unload', function (event) {
  sendValueToPlugin('propertyInspectorDisconnected', 'property_inspector');
});

function sendValueToPlugin(value, param) {
  if (websocket) {
		const json = {
			"action": actionInfo['action'],
			"event": "sendToPlugin",
			"context": uuid,
			"payload": {
			[param]: value
			}
		};
		websocket.send(JSON.stringify(json));
	}
}


