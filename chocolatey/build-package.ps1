
param(
    $version
)
<################################################################################
 # VARIABLES
 ################################################################################>
$ErrorActionPreference = 'Stop'; 

$publishDir = "..\Publish"
$outputDir = "$publishDir\lanceur"
$nuspec = "$outputDir\lanceur.nuspec"
$installScript = "$pwd\lanceur\tools\chocolateyinstall.ps1"

<################################################################################
 # MAIN
 ################################################################################>

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "==== BUILDING CHOCOLATEY PACKAGE      ====" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "==== Working dir  : $pwd"           -ForegroundColor Yellow
Write-Host "==== version      : $version"       -ForegroundColor Yellow
Write-Host "==== outputDir    : $outputDir"     -ForegroundColor Yellow
Write-Host "==== nuspec       : $nuspec"        -ForegroundColor Yellow
Write-Host "==== publishDir   : $publishDir"    -ForegroundColor Yellow
Write-Host "==== installScript: $installScript" -ForegroundColor Yellow
Write-Host "----" -ForegroundColor Yellow

if (Test-Path  $outputDir) {
    Remove-Item -Force -Recurse $outputDir
}

Write-Host "--> Copy directory 'lanceur' into '$publishDir'"
Copy-Item .\lanceur $publishDir -Recurse -Force

Write-Host "--> Update the nuspec file with the new version of Lanceur..."
$(Get-Content $nuspec) -replace "<version>.*<\/version>", "<version>$version</version>" | Set-Content -Path $nuspec

Write-Host "--> Update the install script with the URL of the package..."
$replacement = "`$1'https://github.com/jibedoubleve/lanceur/releases/download/$version/lanceur.$version.setup.exe'"
$script = $(Get-Content $installScript) 
$script = $script -replace '(\$url[ ]*=[ ]*)(''.*'')', $replacement 

$file = gci -path $publishDir | where Name -Match "lanceur.*\.setup\.exe" | select -First 1 -ExpandProperty FullName
Write-Host "--> Calculating checksum for '$file'..."
$checksum = checksum -t sha256 -f $file
Write-Host "--> Set the checksum of the package ['$checksum']..." -ForegroundColor Cyan
$script = $script -replace '([ ]*checksum[ ]*=[ ]*)(''.*'')', "`$1'$checksum'"

Set-Content -Value $script -Path $installScript


if (Test-Path $publishDir) {
    Write-Host "--> Build the package '$nuspec'..."
    choco pack $nuspec -out $publishDir -v
}
else {
    Write-Host "No nuspec file '$nuspec'!" -ForegroundColor Red
}

if (Test-Path  $outputDir) {
    Write-Host "--> Removing '$outputDir' ..." -ForegroundColor Cyan
    Remove-Item -Force -Recurse $outputDir
}