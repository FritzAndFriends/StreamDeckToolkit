Write-Host "Gathering deployment items..."

Write-Host "Current Working Directory: $(get-location)"

$basePath = $(get-location)

# Load and parse the plugin project file - Fully expects there to be only one csproj file in there
$pluginProjectFile = Get-ChildItem -Filter *.csproj | Select-Object -First 1
$projectContent = Get-Content $pluginProjectFile | Out-String;
$projectXML = [xml]$projectContent;

$buildConfiguration = "Debug"

# Get the target .net core framework
$targetFrameworkName = $projectXML.Project.PropertyGroup.TargetFramework;

# Set local path references
if ($IsMacOS) {
  $streamDeckExePath = "/Applications/Stream Deck.app"
  $bindir = "$basePath/bin/Debug/$targetFrameworkName/osx-x64"
} else {
  $streamDeckExePath = "$($ENV:ProgramFiles)\Elgato\StreamDeck\StreamDeck.exe"
  $bindir = "$basePath\bin\Debug\$targetFrameworkName\win-x64"
}

# Make sure we actually have a directory/build to deploy
If (-not (Test-Path $bindir)) {
  Write-Error "The output directory `"$bindir`" was not found.`n You must first build the project before calling this script.";
  exit 1;
}

# Load and parse the plugin's manifest file
$manifestPath = Join-Path $bindir "manifest.json"
$json = Get-Content -Path $manifestPath -Raw | ConvertFrom-Json

$uuidAction = $json.Actions[0].UUID

$pluginID = $uuidAction.substring(0, $uuidAction.Length - ".action".Length)

if($IsMacOS) {
  $destDir = "$HOME/Library/Application Support/com.elgato.StreamDeck/Plugins/$pluginID.sdPlugin"
} else {
  $destDir = "$($env:APPDATA)\Elgato\StreamDeck\Plugins\$pluginID.sdPlugin"
}

$pluginName = Split-Path $basePath -leaf

Get-Process -Name ("StreamDeck", $pluginName) -ErrorAction SilentlyContinue | Stop-Process –force -ErrorAction SilentlyContinue

# Delete the target directory, make sure the deployment/copy is clean
Remove-Item -Recurse -Force -Path $destDir -ErrorAction SilentlyContinue
$bindir =  Join-Path $bindir "*"

# Then copy all deployment items to the plugin directory
New-Item -Type Directory -Path $destDir -ErrorAction SilentlyContinue # | Out-Null
Copy-Item -Path $bindir -Destination $destDir -Recurse

Write-Host "Deployment complete. We will NOT restart the Stream Deck desktop application here, but will from the template..."
# Start-Process $streamDeckExePath
exit 0