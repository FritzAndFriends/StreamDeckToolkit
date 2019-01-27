$streamDeckExePath = "C:\Program Files\Elgato\StreamDeck\StreamDeck.exe"
$bindir = $PSScriptRoot + "\bin\Debug\netcoreapp2.2"
$manifestFile = $bindir +"\manifest.json"

$manifestContent = Get-Content $manifestFile | Out-String
$json = ConvertFrom-JSON $manifestcontent
$uuidAction = $json.Actions[0].UUID
$pluginID = $uuidAction.substring(0, $uuidAction.Length - ".action".Length)
$destDir = $env:APPDATA + "\Elgato\StreamDeck\Plugins\" + $pluginID + ".sdPlugin"

$pluginName = split-path $PSScriptRoot -leaf

Get-Process StreamDeck,$pluginName | Stop-Process –force -ErrorAction SilentlyContinue 

# mkdir $destDir -ErrorAction SilentlyContinue
# Copy content over
# Get-ChildItem -Path $bindir -Recurse | ? { $_.FullName -inotmatch '.vs' } | Copy-Item -Destination $destDir

if (!(Test-Path $destDir)) # Checks if Path exists
{
    # $destDir doesn't exist
    cmd /c mklink /j $destDir $bindir # create the junction pointing to $binddir
    # using the cmd /c mklink because a "New-Item" needs admin rights!
    # this will also not work on any other system than Windows
}
else
{
    # something exists as $destDir
    $junction = Get-Item $destDir
    if (!($junction.Target.Contains($bindir))) # Checks if the $destDir is a junction and is pointing to the correct $bindir
    {
        # $destDir is either already a Junction but not targeting $bindir or it is someting else we can't use
        Remove-Item -Path $destDir -Force -Recurse # remove whatever $destDir is
        cmd /c mklink /j $destDir $bindir # creates the junction to $binddir
    }    
}

Write-Host "Script Completed"

#& $streamDeckExePath
exit 0
