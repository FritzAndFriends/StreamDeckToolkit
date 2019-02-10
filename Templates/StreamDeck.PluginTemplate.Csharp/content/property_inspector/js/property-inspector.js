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
	// initiate call to receive settings async
	getSettings();
  };

  websocket.onmessage = function (evt) {
	// Received message from Stream Deck
	var jsonObj = JSON.parse(evt.data);
	var sdEvent = jsonObj['event'];
	switch (sdEvent) {
	  case "didReceiveSettings":
		if (jsonObj.payload.settings.counter) {
		  document.getElementById('txtCounterValue').value = jsonObj.payload.settings.counter;
		}
		break;
	  default:
		break;
	}
  };
}

const getSettings = () => {
  if (websocket) {
	var json = {
	  "event": "getSettings",
	  "context": uuid
	};
	websocket.send(JSON.stringify(json));
  }
};

const setSettings = (value, param) => {
  if (websocket) {
	var json = {
	  "event": "setSettings",
	  "context": uuid,
	  "payload": {
		[param]: value
	  }
	};
	websocket.send(JSON.stringify(json));
  }
};

