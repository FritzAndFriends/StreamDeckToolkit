
$streamDeckExePath = "C:\Program Files\Elgato\StreamDeck\StreamDeck.exe"
$bindir = $PSScriptRoot + "\bin\Debug\netcoreapp2.2\win10-x64"
$manifestFile = $bindir +"\manifest.json"

$manifestContent = Get-Content $manifestFile | Out-String
$json = ConvertFrom-JSON $manifestcontent
$uuidAction = $json.Actions[0].UUID
$pluginID = $uuidAction.substring(0, $uuidAction.Length - ".action".Length)
$destDir = $env:APPDATA + "\Elgato\StreamDeck\Plugins\" + $pluginID + ".sdPlugin"

$pluginName = split-path $PSScriptRoot -leaf

Get-Process StreamDeck,$pluginName | Stop-Process –force -ErrorAction SilentlyContinue 

mkdir $destDir -ErrorAction SilentlyContinue
Get-ChildItem -Path $bindir -Recurse | ? { $_.FullName -inotmatch '.vs' } | Copy-Item -Destination $destDir

Write-Host "Script Completed"

#& $streamDeckExePath
exit 0
