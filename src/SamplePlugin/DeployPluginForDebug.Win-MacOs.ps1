# create a switch for the option to create or check an existing NTFS Junciton
# Further named param must be declared here. This also leverages tap-completion on commandline.
param (
		[switch]$NtfsJunction = $false, # Adds the -NtfsJunction switch, default is false so the default is to copy to destDir
		[switch]$J = $NtfsJunction, # Adds the -J switch (alias for -NtfsJunction)
		[string]$Configuration = "Debug", # have the Configuration value available. Defaults to 'Debug'
		[string]$pluginName = "",
		[string]$TargetFramework = ""
)

if($Configuration.Contains("Release")){
	# This script is not soposed to do anything in 'Release' configurations
	Write-Host "This script is only intended to do something in Debug configuration! Exit now!"
	exit 1
}

# create functions for often used lines/commands to adhere to DRY principles
function Remove-DestDir($destDir) {
  Remove-Item -Recurse -Force -Path $destDir -ErrorAction SilentlyContinue
}
function New-Junciton($destDir, $bindir) {
  # using the cmd /c mklink because a "New-Item" needs admin rights!
  cmd /c mklink /j $destDir $bindir
}

Write-Host "Gathering deployment items..."
Write-Host "Script root: $PSScriptRoot"

$basePath = $PSScriptRoot

if (!($PSSCriptRoot)) {
  $basePath = $PWD.Path;
}

if(!($pluginName)) {
	$pluginName =	Split-Path $basePath -leaf
}

Write-Host "Plugin Name: $pluginName"
Write-Host "Used Configuration: $Configuration"
Write-Host "TargetFramework: $TargetFramework"

# Load and parse the plugin project file
$pluginProjectFile = Join-Path $basePath "$pluginName.csproj"
$projectContent = Get-Content $pluginProjectFile | Out-String;
$projectXML = [xml]$projectContent;



# Get the target .net core framework
if (!($TargetFramework)) {$TargetFramework = $projectXML.Project.PropertyGroup.TargetFramework;}

# Set local path references
if ($IsMacOS) {
  $useJunction = $false # There is no NtfsJunction available on MacOS so we can NOT use it here
  $streamDeckExePath = "/Applications/Stream Deck.app"
  $bindir = "$basePath/bin/$Configuration/$TargetFramework/osx-x64"
}
else {
  $streamDeckExePath = "$($ENV:ProgramW6432)\Elgato\StreamDeck\StreamDeck.exe"
  $bindir = "$basePath\bin\$Configuration\$TargetFramework\win-x64"
  $useJunction = $j.IsPresent # Uses NtfsJunction when the -NtfsJunction switch is present
}

# Make sure we actually have a directory/build to deploy
If (-not (Test-Path $bindir)) {
  Write-Error "The output directory `"$bindir`" was not found.`n You must first build the `"$pluginName.csproj`" project before calling this script.";
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


Get-Process -Name ("StreamDeck", $pluginName) -ErrorAction SilentlyContinue | Stop-Process –force -ErrorAction SilentlyContinue

If (!$useJunction) {
  # default behavior

  # Delete the target directory, make sure the deployment/copy is clean
  Remove-DestDir $destDir

  $bindir = Join-Path $bindir "*"

  # Then copy all deployment items to the plugin directory
  New-Item -Type Directory -Path $destDir -ErrorAction SilentlyContinue | Out-Null
  Copy-Item -Path $bindir -Destination $destDir -Recurse
}
else {
  # checks if NtfsJunction exists and creates it, if it does not exist
  if (!(Test-Path $destDir)) {
    # $destDir doesn't exist
    New-Junciton $destDir $bindir # create the junction pointing to $binddir
  }
  else {
    # something exists as $destDir
    $junction = Get-Item $destDir
    # Checks if the $destDir is a junction and is pointing to the correct $bindir
    if (!($junction.Target.Contains($bindir))) {
      # $destDir is either already a Junction but not targeting $bindir or it is someting else we can't use
      Remove-DestDir $destDir # remove whatever $destDir is
      New-Junciton $destDir $bindir # creates the junction to $binddir
    }
  }
}

#$ENV:ProgramFiles ${env:ProgramFiles(x86)}
Write-Host "Deployment complete. Restart the Stream Deck desktop application here."
Start-Process "$streamDeckExePath"
