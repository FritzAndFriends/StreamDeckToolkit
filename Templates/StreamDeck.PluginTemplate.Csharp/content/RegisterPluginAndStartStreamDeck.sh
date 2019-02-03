#!/usr/bin/env bash

#Define Plugin Directory
PLUGIN_DIR="~/Library/Application\ Support/com.elgato.StreamDeck/Plugins/${pluginName.sdPlugin}"
#Define suffix
SUFFIX=".action"

#Notify user of action.
echo 'Killing the stream deck process'

#Kill the process.
pkill 'Stream Deck'

#Pull the UUID from the manifest.json
uuid=$(sed -n 's/.*"UUID": "\(.*\)"/\1/p' manifest.json)

pluginName=$(echo "${uuid}" | tr -d \" | sed -e "s/${SUFFIX}$//")

#Notify user of Action.
echo "Installing the ${pluginName} plugin..."

#On first run the file does not exist, we check if it exists.
if [ -d ${PLUGIN_DIR} ]; then rm -fr ${PLUGIN_DIR}; fi

#Recreate directory for plugin.
mkdir -p ${PLUGIN_DIR}

#Move our build into folder.
cp bin/Debug/netcoreapp2.2/* ${PLUGIN_DIR}

#Notify user of Action.
echo "Done installing ${pluginName}."

exit
