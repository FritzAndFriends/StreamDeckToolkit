echo 'Killing the stream deck process'
pkill 'Stream Deck'
uuid=$(jq .Actions[0].UUID manifest.json)
suffix=".action"
pluginName=$(echo "$uuid" | tr -d \" | sed -e "s/$suffix$//")
echo $pluginName
exit