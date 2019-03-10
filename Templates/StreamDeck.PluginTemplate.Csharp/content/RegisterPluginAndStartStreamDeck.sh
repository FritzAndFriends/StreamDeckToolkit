#!/bin/sh

#Notify user we are killing the process
echo 'Killing the Stream Deck process'

# Kill the process
pkill 'Stream Deck'

# Pull the UUID from the manifest.json
uuid=$(sed -n 's/.*"UUID": "\(.*\)"/\1/p' manifest.json)

# Pull the plugin name from the UUID
pluginName=${uuid%.*}

# Set the plugins and project directories to a variable
pluginsDir="$HOME/Library/Application Support/com.elgato.StreamDeck/Plugins"
projectDir=$(PWD)

#Notify user we are installing the plugin to the plugins directory
echo "Installing the $pluginName plugin to $pluginsDir"

# Push the plugins directory on the stack
pushd "$pluginsDir"
# Check if the plugin direcotyr exists and remove it
[ -d "$pluginName.sdPlugin" ] && rm -r $pluginName.sdPlugin
# Create the plugins directory
mkdir $pluginName.sdPlugin
# Copy the debug build output to the plugins directory
cp -R "$projectDir/bin/Debug/netcoreapp2.2/osx-x64/." $pluginName.sdPlugin
# Pop the plugins directory off the stack returning to where we were
popd

#Notify user we successfully installed the plugin
echo "Done installing ${pluginName}"

# Reopen the Stream Deck app and background it
open /Applications/Stream\ Deck.app &

# terminiate execution of the script
exit
