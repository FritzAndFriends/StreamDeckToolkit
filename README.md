# StreamDeckToolkit

![](https://dev.azure.com/FritzAndFriends/StreamDeckTools/_apis/build/status/StreamDeckTools-CI)

![](https://vsrm.dev.azure.com/FritzAndFriends/_apis/public/Release/badge/00a6d40c-eb0d-4aa8-a405-d13d03317ca9/1/1)

![](https://img.shields.io/nuget/v/StreamDeckLib.svg)

## Install Project Template

### From File System

Useful for testing local testing of the template when developing. 

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
