# StreamDeckToolkit

[![Build status](https://dev.azure.com/FritzAndFriends/StreamDeckTools/_apis/build/status/StreamDeckTools-CI)](https://dev.azure.com/FritzAndFriends/StreamDeckTools/_build/latest?definitionId=8)  ![](https://vsrm.dev.azure.com/FritzAndFriends/_apis/public/Release/badge/00a6d40c-eb0d-4aa8-a405-d13d03317ca9/1/1)  [![NuGet](https://img.shields.io/nuget/v/StreamDeckLib.svg)](https://www.nuget.org/packages/StreamDeckLib/)

![](https://img.shields.io/azure-devops/tests/FritzAndFriends/StreamDeckTools/8/dev.svg)

[Intellicode Model](https://prod.intellicode.vsengsaas.visualstudio.com/get?m=EE5419D495BE49528606139DA3ADC687)

## What Is This?

This is a template to help create plugins for the [Elgato Stream Deck][Stream Deck], using the [Stream Deck SDK][] with [Dotnet Core][].

## Pre-Requisites

In order to make use of this template, you will need to have the [Dotnet Core SDK][] (version 2.2.100 or above) installed on your development machine.

While not absolutely necessary, it is **strongly** recommended to have the [Stream Deck Software][] installed, to be able to perform some integration testing of your plugin.

## Install Project Template

### From File System

Installing the template from your filesystem is useful for local testing of the template itself. If you are actively working on the template making changes, this is the route you need to use.

To install, run the following command from the root of the repository.

    `dotnet new -i Templates/StreamDeck.PluginTemplate.Csharp`

To pick up any changes you have made to the template source, you must uninstall the template and reinstall it.

To uninstall, run the following command from the root of the respository.

    Windows: `dotnet new -u Templates/StreamDeck.PluginTemplate.Csharp`
		OSX/Linux: `dotnet new -u $PWD/Templates/StreamDeck.PluginTemplate.Csharp`

### From NuGet

    dotnet new -i StreamDeckPluginTemplate
    - OR -
    Install-Package StreamDeckPluginTemplate [-Version x.y.zzz]

## Using the Template

Once the template is installed, open a terminal and create a new project.

    dotnet new streamdeck-plugin -n FirstPlugin

Or create a directory in a location of your choice, change to that directory and run the command, which will inherit the directory name as the project name.

    dotnet new streamdeck-plugin

<!-- Reference Links -->

[Dotnet Core]: https://dotnet.microsoft.com/ "Free, cross-platform application framework"
[Dotnet Core SDK]: https://get.dot.net/ "Download the Dotnet Core SDK or Runtime"
[Stream Deck]: https://www.elgato.com/gaming/stream-deck/ "Elgato's Stream Deck product page"
[Stream Deck SDK]: https://developer.elgato.com/documentation/stream-deck "Elgato's Stream Deck SDK documentation and reference site"
[Stream Deck Software]: https://www.elgato.com/gaming/downloads "Download the Stream Deck desktop software"
