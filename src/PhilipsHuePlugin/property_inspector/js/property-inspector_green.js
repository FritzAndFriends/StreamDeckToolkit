// global websocket, used to communicate from/to Stream Deck software
// as well as some info about our plugin, as sent by Stream Deck software 
var websocket = null,
  uuid = null,
  inInfo = null,
  actionInfo = {},
  settingsModel = {
		hueHubIp: '',
		appUserId: '',
		lightIndex: 1,
		colorHex: ''
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
	// initiate call to receive settings async
	getSettings();
  };

  websocket.onmessage = function (evt) {
	// Received message from Stream Deck
	var jsonObj = JSON.parse(evt.data);
	var sdEvent = jsonObj['event'];
	switch (sdEvent) {
	  case "didReceiveSettings":
		if (jsonObj.payload.settings.settingsModel.hueHubIp) {
		  settingsModel.hueHubIp = jsonObj.payload.settings.settingsModel.hueHubIp;
		  document.getElementById('txthueHubIp').value = settingsModel.hueHubIp;
		}
		if (jsonObj.payload.settings.settingsModel.appUserId) {
		  settingsModel.appUserId = jsonObj.payload.settings.settingsModel.appUserId;
		  document.getElementById('txtappUserId').value = settingsModel.appUserId;
		}
		if (jsonObj.payload.settings.settingsModel.lightIndex) {
		  settingsModel.lightIndex = jsonObj.payload.settings.settingsModel.lightIndex;
		  document.getElementById('txtlightIndex').value = settingsModel.lightIndex;
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
	settingsModel[param] = value;
	var json = {
	  "event": "setSettings",
	  "context": uuid,
	  "payload": {
		"settingsModel": settingsModel
	  }
	};
	websocket.send(JSON.stringify(json));
  }
};

