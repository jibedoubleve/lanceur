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
    Select-References
}
function ExtractName($prj) {
    $pattern = '.*\\(.*).csproj';
    $r = $prj -match $pattern
    return $matches[1]
}
function Select-References {
    param(
        [Parameter(ValueFromPipeline, Mandatory)]
        [System.IO.FileInfo]$project,
        [string[]]$excludeProjectsContaining
    )
    process {
        $content = Get-Content $_.FullName
        $projectName = $_.BaseName
        $pattern = '\<ProjectReference Include=".*\\(.*).csproj'

        $content | % { 
            if( $_ -match $pattern) { $matches[1]  }
        } | foreach { "[" + $projectName + "] -.-> [" + $_ + "]," }
    }
}

$excludedProjects = ".*UnitTest.*", "Probel.Lanceur", "*.SharedKernel*."#, ".*Plugin\..*", "*.Repository\..*"
$output = Get-ProjectReferences "..\src" -excludeProjectsContaining $excludedProjects 

$filteredOutput = $output | where {
    foreach ($e in $excludedProjects) {
        $pattern = "\[$e\]"

        if ($_ -match $pattern) { 
            Write-Host "$_ is NOT selected." -ForegroundColor Red 
            return $false
        }
    }

    Write-Host "$_ is selected" -ForegroundColor Green
    return $true;
} | Out-String

$filteredOutput | Out-File "$env:userprofile\Desktop\dependencies.txt"

Start-Process "https://yuml.me/diagram/boring/class/edit/$filteredOutput"

Write-Host ""
Write-Host "==> Go to  https://yuml.me to see the graph <==" -ForegroundColor White -BackgroundColor DarkBlue