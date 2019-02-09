# create a switch for the option to create and check a NTFS Junciton
# Further named param must be declared here. This also leverages tap-completion on commandline.
param (
		[switch]$NtfsJunction = $false, # Adds the -NtfsJunction switch
		[switch]$J = $NtfsJunction # Adds the -J switch (alias for -NtfsJunction)
)

Write-Host "Gathering deployment items..."

Write-Host "Script root: $PSScriptRoot`n"

$basePath = $PSScriptRoot

if (!($PSSCriptRoot)) {
  $basePath = $PWD.Path;
}

# Load and parse the plugin project file
$pluginProjectFile = Join-Path $basePath "SamplePlugin.csproj"
$projectContent = Get-Content $pluginProjectFile | Out-String;
$projectXML = [xml]$projectContent;

$buildConfiguration = "Debug"

# Get the target .net core framework
$targetFrameworkName = $projectXML.Project.PropertyGroup.TargetFramework;

# Set local path references
if ($IsMacOS) {
  $useJunction = $false # There is no NtfsJunction available on MacOS so we can NOT use it here
  $streamDeckExePath = "/Applications/Stream Deck.app"
  $bindir = "$basePath/bin/Debug/$targetFrameworkName/osx-x64"
}
else {
  $streamDeckExePath = "$($ENV:ProgramFiles)\Elgato\StreamDeck\StreamDeck.exe"
  $bindir = "$basePath\bin\Debug\$targetFrameworkName\win-x64"
  $useJunction = $j.IsPresent # Uses NtfsJunction when the -NtfsJunction switch is present
}

# Make sure we actually have a directory/build to deploy
If (-not (Test-Path $bindir)) {
  Write-Error "The output directory `"$bindir`" was not found.`n You must first build the `"SamplePlugin.csproj`" project before calling this script.";
  exit 1;
}

# Load and parse the plugin's manifest file
$manifestPath = Join-Path $bindir "manifest.json"
$json = Get-Content -Path $manifestPath -Raw | ConvertFrom-Json

$uuidAction = $json.Actions[0].UUID

$pluginID = $uuidAction.substring(0, $uuidAction.Length - ".action".Length)

if ($IsMacOS) {
  $destDir = "$HOME/Library/Application Support/com.elgato.StreamDeck/Plugins/$pluginID.sdPlugin"
}
else {
  $destDir = "$($env:APPDATA)\Elgato\StreamDeck\Plugins\$pluginID.sdPlugin"
}

$pluginName = Split-Path $basePath -leaf

Get-Process -Name ("StreamDeck", $pluginName) -ErrorAction SilentlyContinue | Stop-Process –force -ErrorAction SilentlyContinue

If (!$useJunction) {
  # default behavior

  # Delete the target directory, make sure the deployment/copy is clean
  Remove-Item -Recurse -Force -Path $destDir -ErrorAction SilentlyContinue

  $bindir = Join-Path $bindir "*"

  # Then copy all deployment items to the plugin directory
  New-Item -Type Directory -Path $destDir -ErrorAction SilentlyContinue # | Out-Null
  $bindir = $bindir + "\*"
  Copy-Item -Path $bindir -Destination $destDir -Recurse
}
else {
  # checks if NtfsJunction exists and creates it, if it does not exist
  if (!(Test-Path $destDir)) {
    # $destDir doesn't exist
    cmd /c mklink /j $destDir $bindir # create the junction pointing to $binddir
    # using the cmd /c mklink because a "New-Item" needs admin rights!
  }
  else {
    # something exists as $destDir
    $junction = Get-Item $destDir
    # Checks if the $destDir is a junction and is pointing to the correct $bindir
    if (!($junction.Target.Contains($bindir))) {
      # $destDir is either already a Junction but not targeting $bindir or it is someting else we can't use
      Remove-Item -Recurse -Force -Path $destDir -ErrorAction SilentlyContinue # remove whatever $destDir is
      cmd /c mklink /j $destDir $bindir # creates the junction to $binddir
    }
  }
}

Write-Host "Deployment complete. We will NOT restart the Stream Deck desktop application here, but will from the template..."
# Start-Process $streamDeckExePath
