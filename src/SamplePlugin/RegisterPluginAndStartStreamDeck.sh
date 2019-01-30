echo 'Killing the Stream Deck process'
pkill 'Stream Deck'

uuid=$(sed -n 's/.*"UUID": "\(.*\)"/\1/p' manifest.json)
pluginName=${uuid%.*}
pluginsDir="$HOME/Library/Application Support/com.elgato.StreamDeck/Plugins"
projectDir=$(PWD)


echo "Installing the $pluginName plugin to $pluginsDir"

pushd "$pluginsDir"
[ -d "$pluginName.sdPlugin" ] && rm -r $pluginName.sdPlugin
mkdir $pluginName.sdPlugin
cp -R "$projectDir/bin/Debug/netcoreapp2.2/osx-x64/." $pluginName.sdPlugin
popd

echo "Done installing ${pluginName}"

open /Applications/Stream\ Deck.app &

exit