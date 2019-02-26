# Copy registration scripts to the local tempalte directory
cp -avf scripts/registration/. Templates/StreamDeck.PluginTemplate.Csharp/content

# Install the template
dotnet new -i Templates/StreamDeck.PluginTemplate.Csharp
