echo 'Killing the stream deck process'
pkill 'Stream Deck'
uuid=$(jq .Actions[0].UUID manifest.json)
suffix=".action"
pluginName=$(echo "$uuid" | tr -d \" | sed -e "s/$suffix$//")
echo 'Installing the $pluginName plugin...'
rm -r ~/Library/Application\ Support/com.elgato.StreamDeck/Plugins/$pluginName.sdPlugin
mkdir ~/Library/Application\ Support/com.elgato.StreamDeck/Plugins/$pluginName.sdPlugin
cp bin/Debug/netcoreapp2.2/* ~/Library/Application\ Support/com.elgato.StreamDeck/Plugins/$pluginName.sdPlugin
exit