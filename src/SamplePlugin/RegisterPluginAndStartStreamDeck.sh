#!/usr/bin/env bash

#Notify user of action.
echo 'Killing the stream deck process'

#Kill the process.
pkill 'Stream Deck'

#Pull the UUID from the manifest.json
uuid=$(sed -n 's/.*"UUID": "\(.*\)"/\1/p' manifest.json)

#Define suffix
suffix=".action"

pluginName=$(echo "$uuid" | tr -d \" | sed -e "s/$suffix$//")

#Notify user of Action.
echo "Installing the $pluginName plugin..."

#On first run the file does not exist, we ignore errors from removing it.
#Not elegant, and needs love.
rm -r ~/Library/Application\ Support/com.elgato.StreamDeck/Plugins/$pluginName.sdPlugin 2> /dev/null || echo > /dev/null

#Recreate directory for plugin.
mkdir ~/Library/Application\ Support/com.elgato.StreamDeck/Plugins/$pluginName.sdPlugin

#Move our build into folder.
cp bin/Debug/netcoreapp2.2/* ~/Library/Application\ Support/com.elgato.StreamDeck/Plugins/$pluginName.sdPlugin

exit