# Working With This Template

## Welcome

Congratulations! You've just taken the first major step in writing a custom plugin for the [Elgato Stream Deck][Stream Deck].

## State of Things

Within the directory from which you are reading this file, there exist a few other necessary files. These are:

* `_StreamDeckPlugin_.csproj`: The C# project file used to build the plugin
* `Program.cs`: The code for the application which will be called by the [Stream Deck software][] when loading and running your plugin.
* `DefaultPluginAction.cs`: The file in which the functionality of the first (default) action for the plugin will be written. This file provides a basic implementation of an action for your plugin, following a pattern which can be repeated.

## What's Next?

### First, a Word of Caution

Due to the multi-platform target of the [Stream Deck][], when specifying filesystem paths which will be used at runtime (images, other assets), use the POSIX/Unix standard forward slash (`/`) as the directory separator.

**Do this** `path/to/my/assets`
**Instead of** `path\to\my\assets`

### The `manifest.json` file

The [`manifest.json` file][Manifest File] (also referred to as the *manifest* or *manifest file*) is the mechanism used by the [Stream Deck SDK][] to uniquely identify plugins, their actions and other parameters.

Your first step should set a value for both the **Author** and the **URL** values, representing you and your plugin.

### Images and Other Assets

The following are the base set of images/icons which are needed for the plugin. While defaults have been provided, they should be changed to help distinguish your plugin and its action(s) from others.

Unless otherwise noted, image assets should all be in the "Portable Network Graphics" (PNG) format.

> [!Note]
> While all efforts have been made to ensure the correctness of this information, please refer to the official [Style Guide][] and [Manifest file definition][Manifest File]  for the latest and most up-to-date information.

-----

#### Category Icon (*a.k.a. Plugin Icon*)

**Path to property in manifest.json file:** `Icon`

##### Purpose

The category icon, identified by the **Icon** property in the *manifest*, is the primary visual identifier for your plugin. It is also used to display information about your plugin in the **More Actions...** list, which displays the list of available plugins to download to uses, as well as the category (group) icon in the **Actions** list if your plugin supports more than a single action.

##### Specifications

The **Icon** property in the *manifest* represents the base file name of the image, without a file extension. For example, if your icon's file name is `myPluginIcon.png`, you would set the value as `myPluginIcon`.

There are two of these files necessary. A default one for a regular, non-scaled (high-DPI) display, and another for scaled display. The default icon should be 28 pixels squared (28x28px), and named with the base file name and extension. i.e. `_PluginName_.png`.

The other file, for high-DPI displayes, must be 56 pixels squared (56x56px), and the value `@2x` appended to the file name, before the extension. i.e. `_PluginName_@2x.png`

-----

#### Action Image (Icon)

**Path to property in manifest.json file:** `Actions[x].Icon`

##### Purpose

The action image, for which there is one for each action available from the plugin, is the icon which helps identify the action item in the **Actions** list, within the category (group) defined by your plugin. Just as with the **Category Icon**, when setting its value in the *manifest*, do not specify an extension.

##### Specifications

Each element in the **Actions** element of the *manifest* has an **Icon** property which must be set (which one possible exception - see below). While not strictly necessary, each action should have its own, distinct icon for its visual identity. Per the current specifications, the **Action Image** should be a single color -  <div style="width: 20px;height:20px;background-color:#d8d8d8;display:inline-block"></div> `#d8d8d8` (`rgb(216,216,216)`).

Again, just as with the **Category Icon**, two separate copies of this file are needed, with the same naming rules but different sizes; a default one for a regular, non-scaled (high-DPI) display, and another for scaled display. The default icon should be 20 pixels squared (20x20px), and preferably named in a manner in which it can be easily related to its action, such as `actionIcon.png`. When setting its value in the *manifest*, here too specify only the file name, and do not include the etension.

The other file, for high-DPI displayes, must be 40 pixels squared (40x40px), and the value `@2x` appended to the file name, before the extension. i.e. `actionIcon@2x.png`

##### Exceptions

 An **Action** item is not required to have an icon specified (but still can) if its `VisibleInActionList` property in the *manifest* is set to `false`.

-----

#### Key Icon

**Path to property in manifest.json file:** `Actions[x].States[y].Image`

##### Purpose

The **Key Icon** is the icon which is displayed on the key(s) to which is is assigned on the [Stream Deck][], as well as within the [Stream Deck software][] during configuration. If your action supports multiple states, the **Key Icon** will be displayed when its assigned state is active. Each action hast as least one state, and as of this time, has at most two states.

 Once again, just as with the **Category Icon** and **Action Image**, when setting its value in the *manifest*, do not specify an extension.

##### Specifications

Again, just as with the **Category Icon** and the **Action Image**,two separate copies of this file are needed, with the same naming rules but different sizes; a default one for a regular, non-scaled (high-DPI) display, and another for scaled display. The default icon should be 72 pixels squared (72x72px). Again, it should preferably named in a manner in which it can be easily related to either the action or state is represents, such as `actionIconButton.png` or `actionIconActive.png`.

### Can I do any more?

Of course! First, congratulations on getting your first action for your Stream Deck plugin working! To allow your plugin to do more, you will need to create (and implement) a new Action definition. Here is how you do this:

1. Create a new class (in a new file or an existing one), and make sure it inherits from the `BaseStreamDeckAction` class
e.g.: `internal class MyNextPluginAction : BaseStreamDeckAction`
2. Implement the required properties and methods, such as `UUID`
3. Register it with the `ConnectionManager` instance within the `Program.cs` file. Look for the `.RegisterAction(new DefaultPluginAction()` code line, copy it, and change the type `DefaultPluginAction` to your new class' name. From the example in step 1, this would look like `.RegisterAction(new MyNextPluginAction())`.
4. Add a definition for the new action in the `manifest.json` file, ensuring that the value for the `UUID` field in the manifest matches the `UUID` property of your new class.

That's it! Repeat this process for any additional actions you wish to include and perform as part of your plugin.


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
[Manifest file]: https://developer.elgato.com/documentation/stream-deck/sdk/manifest "Definition of elements in the manifest.json file"
