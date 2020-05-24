## Install Project Template

### From File System

Installing the template from your filesystem is useful for local testing of the template itself. If you are actively working on the template making changes, this is the route you need to use.

To install, run the following command from the root of the repository.

`dotnet new -i Templates/StreamDeck.PluginTemplate.Csharp`

To pick up any changes you have made to the template source, you must uninstall the template and reinstall it.

To uninstall, run the following command from the root of the respository.

**Windows:**  `dotnet new -u Templates/StreamDeck.PluginTemplate.Csharp`

**OSX/Linux:** `dotnet new -u $PWD/Templates/StreamDeck.PluginTemplate.Csharp`

### From NuGet

`dotnet new -i StreamDeckPluginTemplate`

- OR -

`Install-Package StreamDeckPluginTemplate [-Version x.y.zzz]`
