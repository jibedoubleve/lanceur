param(
    [Parameter(Mandatory = $true)] $version
)

$vFile = ".\src\Version.cs"
$vInno = ".\build\setup.iss"

$version = $version -replace "V", ""

Write-Host "Replace version '$version' in file $vFile"
Set-Content -Path $vFile -Value $($(Get-content $vFile) -replace '"([0-9]{1,3}\.?){3,4}"', """$version""")

Write-Host "Replace version '$version' in file $vInno"
Set-Content -Path $vInno -Value $($(Get-Content $vInno) -replace "#define MyAppVersion ""([0-9]{1,3}\.?){3}""", "#define MyAppVersion ""$version""")

Start-Sleep 1