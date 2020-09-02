Get-Process | where Name -Like 'Probel.Lanceur' | Stop-Process

$plugins = "$env:APPDATA\probel\lanceur\plugins\"
$repositories = "$env:APPDATA\probel\lanceur\repositories\"

if (Test-Path $plugins) {
    Remove-Item -Recurse -Force $plugins
}
if (Test-Path $repositories) {
    Remove-Item -Recurse -Force $repositories
}
