# StreamDeckToolkit

[![Build status](https://dev.azure.com/FritzAndFriends/StreamDeckTools/_apis/build/status/StreamDeckTools-CI)](https://dev.azure.com/FritzAndFriends/StreamDeckTools/_build/latest?definitionId=8)  ![](https://vsrm.dev.azure.com/FritzAndFriends/_apis/public/Release/badge/00a6d40c-eb0d-4aa8-a405-d13d03317ca9/1/1)  [![NuGet](https://img.shields.io/nuget/v/StreamDeckLib.svg)](https://www.nuget.org/packages/StreamDeckLib/)

![](https://img.shields.io/azure-devops/tests/FritzAndFriends/StreamDeckTools/8/dev.svg)

[Intellicode Model](https://prod.intellicode.vsengsaas.visualstudio.com/get?m=409A792BE4F74201806A848409B2984D)

## What Is This?

This is a template to help create plugins for the [Elgato Stream Deck][Stream Deck], using the [Stream Deck SDK][] with [Dotnet Core][].

## Pre-Requisites

In order to make use of this template, you will need to have the [Dotnet Core SDK][] (version 2.2.100 or above) installed on your development machine.

While not absolutely necessary, it is **strongly** recommended to have the [Stream Deck Software][] installed, to be able to perform some integration testing of your plugin.

## Install Project Template

### From File System

Installing the template from your filesystem is useful for local testing of the template itself. If you are actively working on the template making changes, this is the route you need to use.

To install, run the following command from the root of the repository.

    dotnet new -i Templates/StreamDeck.PluginTemplate.Csharp

To pick up any changes you have made to the template source, you must uninstall the template and reinstall it.

To uninstall, run the following command from the root of the respository.

**Windows:**  `dotnet new -u Templates/StreamDeck.PluginTemplate.Csharp`

**OSX/Linux:** `dotnet new -u $PWD/Templates/StreamDeck.PluginTemplate.Csharp`

### From NuGet

    dotnet new -i StreamDeckPluginTemplate
    - OR -
    Install-Package StreamDeckPluginTemplate [-Version x.y.zzz]

## Using the Template

Once the template is installed, open a terminal in the folder of your choice and create a new project.

    dotnet new streamdeck-plugin --plugin-name FirstPlugin --uuid com.yourcompany.pluginname.actionname --skipRestore false

Or create a directory in a location of your choice, change to that directory and run the command, which will inherit the directory name as the project name with default values.

    dotnet new streamdeck-plugin

## Creating a Plugin Action
The Stream Deck Toolkit provides the functionality that communicates directly with the Stream Deck software. When creating a plugin, you are responsible for creating actions for the Stream Deck buttons to perform. There are two base classes that you can inherit from when creating your action:

    1. BaseStreamDeckAction - this class contains all the integrations necessary to communicate with the Stream Deck at the 'barebones' level. Inheriting from this class will give you the greatest control over how your action sends and receives data from the software.

    2. BaseStreamDeckActionWithSettingsModel<T> - this class inherits from BaseStreamDeckAction, this class will automate the population of model properties, where type T is defined as the data that is stored when issuing a 'setSettings' event to the Stream Deck software. The property **SettingsModel** will automatically instantiate an instance of class T, so it is best to assign default values when defining your class T. Also, when using the Property Inspector and passing data back and forth, ensure that the properties defined in the settingsModel in JavaScript matches those that you have defined in T for the automatic mapping to occur between both environments.

Your project may contain any number of actions, inheriting from one of the classes above. In order for the Action to be automatically registered on start up, it must bear the **[ActionUuid(Uuid="com.fritzanfriends.pluginname.anotheraction")]** attribute.

Actions must also be manually registered in the **manifest.json** file, with the Uuid that matches ActionUuid attribute.

<!-- Reference Links -->

[Dotnet Core]: https://dotnet.microsoft.com/ "Free, cross-platform application framework"
[Dotnet Core SDK]: https://get.dot.net/ "Download the Dotnet Core SDK or Runtime"
[Stream Deck]: https://www.elgato.com/gaming/stream-deck/ "Elgato's Stream Deck product page"
[Stream Deck SDK]: https://developer.elgato.com/documentation/stream-deck "Elgato's Stream Deck SDK documentation and reference site"
[Stream Deck Software]: https://www.elgato.com/gaming/downloads "Download the Stream Deck desktop software"
