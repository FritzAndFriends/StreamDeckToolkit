#!bin/sh
#Notify user we are killing the process
echo 'Killing the Stream Deck process'

# Kill the process
pkill 'Stream Deck'

pluginUUID="$(UUID)"

# Set the plugins and project directories to a variable
pluginsDir="$HOME/Library/Application Support/com.elgato.StreamDeck/Plugins"
projectDir=$(PWD)

#Notify user we are installing the plugin to the plugins directory
echo "Installing the $(PluginName) plugin to $pluginsDir"

# Push the plugins directory on the stack
pushd "$pluginsDir"
# Check if the plugin direcotyr exists and remove it
[ -d "$pluginUUID.sdPlugin" ] && rm -r $pluginUUID.sdPlugin
# Create the plugins directory
mkdir $pluginUUID.sdPlugin
# Copy the debug build output to the plugins directory
cp -R "$projectDir/bin/Debug/netcoreapp2.2/osx-x64/." $pluginUUID.sdPlugin
# Pop the plugins directory off the stack returning to where we were
popd

#Notify user we successfully installed the plugin
echo "Done installing $(PluginName)"

# Reopen the Stream Deck app and background it
open /Applications/Stream\ Deck.app &

# terminiate execution of the script
exit
