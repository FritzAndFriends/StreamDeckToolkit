#$msgBox = new-object -comobject wscript.shell

$streamDeckExePath = "C:\Program Files\Elgato\StreamDeck\StreamDeck.exe"

$pluginName = split-path $pwd -leaf
#$msgBox.popup($pluginName, 0, "build 2", 1)

$manifestContent = Get-Content manifest.json | Out-String
$json = ConvertFrom-JSON $manifestcontent
$uuidAction = $json.Actions[0].UUID
$pluginID = $uuidAction.substring(0, $uuidAction.Length - ".action".Length)

$destDir = $env:APPDATA + "\Elgato\StreamDeck\Plugins\" + $pluginID + ".sdPlugin"
$bindir = $PSScriptRoot + "\bin\Debug\netcoreapp2.2\win10-x64"

WRITE-HOST "Current Working Directory: " $bindir
# get-process $pluginName | Stop-Process –force

WRITE-HOST "Copying files..."
mkdir $destDir
# TODO don't copy the .vs folder
Get-ChildItem -Path $bindir -Recurse | Copy-Item -Destination $destDir

#WRITE-HOST "Restart Stream Deck..."
#& $streamDeckExePath

WRITE-HOST "SCRIPT COMPLETED"

#$lockedFile= $destDir + "\hostfxr.dll"
#Get-Process | foreach{$processVar = $_;$_.Modules | foreach{if($_.FileName -eq $lockedFile){$processVar.Name + " PID:" + $processVar.id}}}

exit 0