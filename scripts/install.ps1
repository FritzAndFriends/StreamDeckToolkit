# Copy registration scripts to the local tempalte directory
get-childitem -path "scripts/registration/" -filter *.* -recurse | copy-item -destination "Templates/StreamDeck.PluginTemplate.Csharp/content"

# Install the template
dotnet new -i Templates/StreamDeck.PluginTemplate.Csharp
