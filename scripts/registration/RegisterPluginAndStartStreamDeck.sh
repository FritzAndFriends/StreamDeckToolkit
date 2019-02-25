#!bin/sh

#Notify user we are killing the process
echo 'Killing the Stream Deck process'

# Kill the process
pkill 'Stream Deck'

# When creating a project from the template, this will be set at code generation time.
pluginUUID="com.csharpfritz.samplePlugin"

# Set the plugins and project directories to a variable
pluginsDir="$HOME/Library/Application Support/com.elgato.StreamDeck/Plugins"
projectDir=$(PWD)

#Notify user we are installing the plugin to the plugins directory
echo "Installing the SamplePlugin plugin to $pluginsDir"

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
echo "Done installing SamplePlugin"

# Reopen the Stream Deck app and background it
# WILL NOT restart the Stream Deck software from the sample plugin, just as in the .PS1 script.
#open /Applications/Stream\ Deck.app &

# terminiate execution of the script
exit
