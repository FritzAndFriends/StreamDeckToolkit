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

Useful for local testing of the template when developing.

    dotnet new -i /StreamDeckToolkit/Templates/StreamDeck.PluginTemplate.Csharp/

Uninstall the local template by running.

    dotnet new -u /StreamDeckToolkit/Templates/StreamDeck.PluginTemplate.Csharp/

### From NuGet

    dotnet add package StreamDeckPluginTemplate
    - OR -
    Install-Package StreamDeckPluginTemplate -Version 0.2.286

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
