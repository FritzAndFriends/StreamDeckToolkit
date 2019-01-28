#!/usr/bin/env bash

#Define suffix
SUFFIX=".action"

#Notify user of action.
echo 'Killing the stream deck process'

#Kill the process.
pkill 'Stream Deck'

#Pull the UUID from the manifest.json
uuid=$(sed -n 's/.*"UUID": "\(.*\)"/\1/p' manifest.json)

pluginName=$(echo "${uuid}" | tr -d \" | sed -e "s/${SUFFIX}$//")

#Define Plugin Directory
PLUGIN_DIR="${HOME}/Library/Application Support/com.elgato.StreamDeck/Plugins/${pluginName}.sdPlugin"

#Notify user of Action.
echo "Installing the ${pluginName} plugin..."

#On first run the file does not exist, we check if it exists.
[[ -d "${PLUGIN_DIR}" ]] && rm -fr "${PLUGIN_DIR}"

#Recreate directory for plugin.
mkdir -p "${PLUGIN_DIR}"

#Move our build into folder.
cp bin/Debug/netcoreapp2.2/osx-x64/* "${PLUGIN_DIR}"

#Notify user of Action.
echo "Done installing ${pluginName}."

#start the stream deck
open /Applications/Stream\ Deck.app & 

exit