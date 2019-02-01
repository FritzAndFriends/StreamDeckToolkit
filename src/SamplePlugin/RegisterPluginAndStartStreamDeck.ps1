Write-Host "Gathering deployment items..."

Write-Host "Script root: $PSScriptRoot`n"

$basePath = $PSScriptRoot

if ($PSSCriptRoot.Length -eq 0) {
  $basePath = $PWD.Path;
}


# Load and parse the plugin project file
$pluginProjectFile = "$basePath\SamplePlugin.csproj"
$projectContent = Get-Content $pluginProjectFile | Out-String;
$projectXML = [xml]$projectContent;

$buildConfiguration = "Debug"

# Get the target .net core framework
$targetFrameworkName = $projectXML.Project.PropertyGroup.TargetFramework;

# Set local path references
$streamDeckExePath = "$($ENV:ProgramFiles)\Elgato\StreamDeck\StreamDeck.exe"

# For now, this PS script will only be run on Windows.
$bindir = "$basePath\bin\Debug\$targetFrameworkName\win-x64\"

# Make sure we actually have a directory/build to deploy
If (-not (Test-Path $bindir)) {
  Write-Error "The output directory `"$bindir`" was not found.`n You must first build the `"SamplePlugin.csproj`" project before calling this script.";
  exit 1;
}

# Load and parse the plugin's manifest file
$manifestFile = $bindir +"\manifest.json"
$manifestContent = Get-Content $manifestFile | Out-String
$json = ConvertFrom-JSON $manifestcontent

$uuidAction = $json.Actions[0].UUID

$pluginID = $uuidAction.substring(0, $uuidAction.Length - ".action".Length)
$destDir = "$($env:APPDATA)\Elgato\StreamDeck\Plugins\$pluginID.sdPlugin"

$pluginName = Split-Path $basePath -leaf

Get-Process StreamDeck,$pluginName | Stop-Process –force -ErrorAction SilentlyContinue

# Delete the target directory, make sure the deployment/copy is clean
Remove-Item -Recurse -Path $destDir

# Then copy all deployment items to the plugin directory
New-Item -Type Directory -Path $destDir -ErrorAction SilentlyContinue # | Out-Null
Push-Location $bindir
Copy-Item -Path "$bindir\*.*" -Destination $destDir -Recurse
Pop-Location

Write-Host "Deployment complete. Restarting Stream Deck..."
Start-Process $streamDeckExePath
exit 0
