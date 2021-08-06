## Creating a Plugin Action

The Stream Deck Toolkit provides the functionality that communicates directly with the Stream Deck software. When creating a plugin, you are responsible for creating actions for the Stream Deck buttons to perform. There are two base classes that you can inherit from when creating your action:

1. `BaseStreamDeckAction` - this class contains all the integrations necessary to communicate with the Stream Deck at the 'barebones' level. Inheriting from this class will give you the greatest control over how your action sends and receives data from the software.

2. `BaseStreamDeckActionWithSettingsModel<T>` - this class inherits from BaseStreamDeckAction, this class will automate the population of model properties, where type T is defined as the data that is stored when issuing a 'setSettings' event to the Stream Deck software. The property **`SettingsModel`** will automatically instantiate an instance of class T, so it is best to assign default values when defining your class T. Also, when using the Property Inspector and passing data back and forth, ensure that the properties defined in the settingsModel in JavaScript matches those that you have defined in T for the automatic mapping to occur between both environments.

Your project may contain any number of actions, inheriting from one of the classes above. In order for the Action to be automatically registered on start up, it must bear the **[StreamDeckAction(Uuid="com.fritzanfriends.pluginname.anotheraction")]** attribute, and have a UUID set via the attribute, or by overloading `ActionUuid` in your `BaseStreamDeckAction` implementation.

Actions must also be manually registered in the **manifest.json** file, with the Uuid that matches the one set with the `StreamDeckActionAttribute`, or an `ActionUuid` overload of the `BaseStreamDeckAction.ActionUuid` property.
