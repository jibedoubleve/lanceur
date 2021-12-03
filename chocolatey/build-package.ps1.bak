param(
    $version
)
<################################################################################
 # VARIABLES
 ################################################################################>
$ErrorActionPreference = 'Stop'; 

$publishDir = "..\Publish"
$outputDir = "$publishDir\logreader"
$nuspec = "$outputDir\logreader.nuspec"
$installScript    = "$pwd\logreader\tools\chocolateyinstall.ps1"

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

Copy-Item .\logreader $publishDir -Recurse -Force

$(Get-Content $nuspec) -replace "<version>.*</version>", "<version>$version</version>" | Set-Content -Path $nuspec

$content = $(Get-Content $installScript) -replace "https:\/\/github\.com\/jibedoubleve\/log-reader\/releases\/download\/[Vv]?\.?\d*\.?\d*\.?\d*\/logreader\.\d*\.?\d*\.?\d*\.setup\.exe", "https://github.com/jibedoubleve/log-reader/releases/download/$version/logreader.$version.setup.exe"

#Display content to screen
$content

Set-Content $content -Path $installScript 

choco pack $nuspec -out $publishDir

if (Test-Path  $outputDir) {
    Remove-Item -Force -Recurse $outputDir
}