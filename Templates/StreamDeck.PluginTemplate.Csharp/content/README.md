# Working With This Template

## Welcome

Congratulations! You've just taken the first major step in writing a custom plugin for the [Elgato Stream Deck][Stream Deck].

## State of Things

Within the directory from which you are reading this file, there exist a few other necessary files. These are:

* `_StreamDeckPlugin_.csproj`: The C# project file used to build the plugin
* `Program.cs`: The code for the application which will be called by the [Stream Deck software][] when loading and running your plugin.
* `_PluginName_.cs`: The file in which the functionality of the plugin will be written. This file provides the proverbial guts of your plugin.

## What's Next?

### The `manifest.json` file

The `manifest.json` file is the mechanism used by the [Stream Deck SDK][] to identify plugins and their unique parameters.

At the very least, you should set a value for both the **Author** and the **URL** values, representing you and your plugin.

### Images and Other Assets

The following are the base set of images/icons which are needed for the plugin. While defaults have been provided, they should be changed to help distinguish your plugin and its action(s) from others.

Unless otherwise noted, image assets should all be in the "Portable Network Graphics" (PNG) format.

*Note:* While all efforts have been made to ensure the correctness of this information, please refer to the official [Style Guide][] for the lateat, up-to-date, information.

#### Plugin Icon

##### Purpose

The plugin icon, identified by the **Icon** property in the `manifest.json` file, is the primary visual identifier for your plugin. It is also used to display information about your plugin in the **More Actions...** list and as the category (group) icon in the **Actions** list if your plugin supports more than a single action.

##### Specifications

The **Icon** property in the `manifest.json` file represents the base file name of the image, without a file extension. For example, if your icon's file name is `myPluginIcon.png`, you would set the value as `myPluginIcon`.

There are two of these files necessary. A default one for a regular, non-scaled (high-DPI) display, and another for scaled display. The default icon should be 28 pixels squared (28x28px), and named with the base file name and extension. i.e. `_PluginName_.png`.

The other file, for high-DPI displayes, must be 56 pixels squared (56x56px), and the value `@2x` appended to the file name, before the extension. i.e. `_PluginName_@2x.png`

#### Actions Icon



The following

## References
Here are some helpful references for both this template and the Stream Deck:
* [Plugin Homepage](https://github.com/FritzAndFriends/StreamDeckToolkit)
* [Stream Deck Page][Stream Deck]
* [Stream Deck SDK Documentation][Stream Deck SDK]




<!-- References -->
[Stream Deck]: https://www.elgato.com/en/gaming/stream-deck "Elgato's Stream Deck landing page for the hardware, software, and SDK"
[Stream Deck software]: https://www.elgato.com/gaming/downloads "Download the Stream Deck software"
[Stream Deck SDK]: https://developer.elgato.com/documentation/stream-deck "Elgato's online SDK documentation"
[Style Guide]: https://developer.elgato.com/documentation/stream-deck/sdk/style-guide/ "The Stream Deck SDK Style Guide"
