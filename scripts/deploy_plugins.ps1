<#############################################################################
 # VARIABLES
 #############################################################################>
$release = "Debug"
# Plugins
$src = "$env:GIT_PRJ_SOURCE\lanceur\src\Plugins\Probel.Lanceur.Plugin.{0}\bin\$release\*.*"
$dst = "$env:APPDATA\probel\lanceur\plugins\{0}"
$plugins = "spotify", "calculator", "evernote", "clipboard"

# Repos
$repos = @("win32search", "uwpsearch")
$repoSrc = "$env:GIT_PRJ_SOURCE\lanceur\src\Repositories\Probel.Lanceur.Repository.{0}\bin\$release\*.*"
$repoDst = "$env:APPDATA\probel\lanceur\repositories\{0}"

<#############################################################################
 # FUNCTIONS 
 #############################################################################>
function Write-Debug($msg) {
    Write-Host $msg -ForegroundColor Green
}
function Write-Info($msg) {
    Write-Host $msg -ForegroundColor Yellow
}
function Add-Plugins() {
    foreach ($plugin in $plugins) {
        Write-Info "Copying files for plugin '$plugin'..."
        $s = $src -f $plugin
        $d = $dst -f $plugin
        Write-Debug "[PLUGIN]      Source : $s"
        Write-Debug "[PLUGIN] Destination : $d"
        Write-Host "-------------------------------------"
    
        $file_exists = Test-Path $d
        if ($file_exists -eq $false) {
            Write-Info "Creating directory '$d'"
            mkdir $d
        }
    
        Copy-Item $s $d -Recurse -Force
    }
}
function Add-Repositories() {
    foreach ($repo in $repos) { 
        Write-Info "Copying files for repository '$repo'..."
        $s = $repoSrc -f $repo
        $d = $repoDst -f $repo
        Write-Debug "[REPO]      Source : $s"
        Write-Debug "[REPO] Destination : $d"
        Write-Host "-------------------------------------"
    
        $file_exists = Test-Path $d
        if ($file_exists -eq $false) {
            Write-Info "Creating directory '$d'"
            mkdir $d
        }
    
        Copy-Item $s $d -Recurse -Force
    }
}
<#############################################################################
 # MAIN 
 #############################################################################>
Write-Host
Write-Host "######################################" -ForegroundColor Cyan
Write-Host "## PLUGINS                          ##" -ForegroundColor Cyan
Write-Host "######################################" -ForegroundColor Cyan

Add-Plugins

Write-Host
Write-Host "######################################" -ForegroundColor Cyan
Write-Host "## REPOSITORIES                     ##" -ForegroundColor Cyan
Write-Host "######################################" -ForegroundColor Cyan

Add-Repositories


