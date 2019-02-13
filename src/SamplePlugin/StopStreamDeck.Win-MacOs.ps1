param (
		[string]$pluginName = ""
)

$basePath = $PSScriptRoot

if (!($PSSCriptRoot)) {
  $basePath = $PWD.Path;
}

if(!($pluginName)) {
	$pluginName =	Split-Path $basePath -leaf
}



Write-Host "Stopping the processes 'StreamDeck' and '$pluginName' ..."
Get-Process -Name ("StreamDeck", $pluginName) -ErrorAction SilentlyContinue | Stop-Process –force -ErrorAction SilentlyContinue
Write-Host "Processes stopped"
