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
if ($IsMacOS) {
  $streamDeckExePath = "/Applications/Stream Deck.app"
  $bindir = "$basePath/bin/Debug/$targetFrameworkName/osx-x64"
} else {
  $streamDeckExePath = "$($ENV:ProgramFiles)\Elgato\StreamDeck\StreamDeck.exe"
  $bindir = "$basePath\bin\Debug\$targetFrameworkName\win-x64"
}

# Make sure we actually have a directory/build to deploy
If (-not (Test-Path $bindir)) {
  Write-Error "The output directory `"$bindir`" was not found.`n You must first build the `"SamplePlugin`" project before calling this script.";
  exit 1;
}

  $pluginUUID = "com.csharpfritz.samplePlugin";
  $destDir = "$($env:APPDATA)\Elgato\StreamDeck\Plugins\$pluginUUID.sdPlugin"

$pluginName = Split-Path $basePath -leaf

Get-Process -Name ("StreamDeck", $pluginName) -ErrorAction SilentlyContinue | Stop-Process –force -ErrorAction SilentlyContinue

# Delete the target directory, make sure the deployment/copy is clean
If (Test-Path $destDir) {
  Remove-Item -Recurse -Force -Path $destDir
}

# Then copy all deployment items to the plugin directory
New-Item -Type Directory -Path $destDir -ErrorAction SilentlyContinue # | Out-Null
$bindir = $bindir +"\*"
Copy-Item -Path $bindir -Destination $destDir -Recurse

Write-Host "Deployment complete. We will NOT restart the Stream Deck desktop application here, but will from the template..."
# Start-Process $streamDeckExePath
exit 0