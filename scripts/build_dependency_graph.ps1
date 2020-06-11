<#
 # Get all the dependencies and displays it in https://yuml.me
 # https://blog.dantup.com/2012/05/free-dependency-graph-generation-using-powershell-and-yuml/
 #>

function Get-ProjectReferences {
    param(
        [Parameter(Mandatory)]
        [string]$rootFolder,
        [string[]]$excludeProjectsContaining
    )
    dir $rootFolder -Filter *.csproj -Recurse |
    # Exclude any files matching our rules
    where { $excludeProjectsContaining -notlike "*$($_.BaseName)*" } |
    Select-References
}
function Select-References {
    param(
        [Parameter(ValueFromPipeline, Mandatory)]
        [System.IO.FileInfo]$project,
        [string[]]$excludeProjectsContaining
    )
    process {
        $projectName = $_.BaseName
        [xml]$projectXml = Get-Content $_.FullName
        $ns = @{ defaultNamespace = "http://schemas.microsoft.com/developer/msbuild/2003" }
        $projectXml |
        # Find the references xml nodes
        Select-Xml '//defaultNamespace:ProjectReference/defaultNamespace:Name' -Namespace $ns |
        # Get the node values
        foreach { $_.node.InnerText } |
        # Exclude any references pointing to projects that match our rules
        where { $excludeProjectsContaining -notlike "*$_*" } |
        # Output in yuml.me format
        foreach { "[" + $projectName + "] -> [" + $_ + "]," }
    }
}

$excludedProjects = "UnitTest", "Plugin."
$output = Get-ProjectReferences "..\src" -excludeProjectsContaining $excludedProjects 

$filteredOutput = $output | where {
    # $result = $excludedProjects -notlike "*$_*"
    foreach ($e in $excludedProjects) {
        if ($_ -like "*$e*") { 
            Write-Host "$_ is NOT selected." -ForegroundColor Red 
            return $false
        }
    }

    Write-Host "$_ is selected" -ForegroundColor Green
    return $true;
} | Out-String

$filteredOutput | Out-File "$env:userprofile\Desktop\dependencies.txt"

Start-Process "https://yuml.me/diagram/boring/class/$filteredOutput"

Write-Host ""
Write-Host "==> Go to  https://yuml.me to see the graph <==" -ForegroundColor White -BackgroundColor DarkBlue