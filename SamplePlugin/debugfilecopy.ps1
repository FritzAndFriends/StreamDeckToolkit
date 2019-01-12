$streamDeckExePath = "C:\Program Files\Elgato\StreamDeck\StreamDeck.exe"
$pluginName = "SamplePlugin"
$destDir = $env:APPDATA +"\Elgato\StreamDeck\Plugins\com.csharpfritz.samplePlugin.sdPlugin"
$bindir = $PSScriptRoot +"\bin\Debug\netcoreapp2.2\win10-x64"

WRITE-HOST "Current Working Directory: " $bindir

Get-Process StreamDeck,SamplePlugin | stop-process –force

WRITE-HOST "Copying files..."
Get-ChildItem -Path $bindir -Recurse | Copy-Item -Destination $destDir

WRITE-HOST "Restart Stream Deck..."
& $streamDeckExePath

WRITE-HOST "SCRIPT COMPLETED"

#$lockedFile= $destDir + "\hostfxr.dll"
#Get-Process | foreach{$processVar = $_;$_.Modules | foreach{if($_.FileName -eq $lockedFile){$processVar.Name + " PID:" + $processVar.id}}}
