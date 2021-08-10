
$ErrorActionPreference = 'Stop'; 
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url        = 'https://github.com/jibedoubleve/lanceur/releases/download/0.8.1/lanceur.0.8.1.setup.exe' 

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  fileType      =  'EXE' 
  url           = $url

  softwareName  = 'lanceur' 

  checksum      = '56C4499E14EEF6D378EE0D133C2A47D9D816A200EEF970E2C7B1196D34B680FE'
  checksumType  = 'sha256' 

  validExitCodes= @(0, 3010, 1641)
  silentArgs   = '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /SP-' 
}

Install-ChocolateyPackage @packageArgs 
