
$ErrorActionPreference = 'Stop'; 
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url        = 'https://github.com/jibedoubleve/lanceur/releases/download/0.8.1/lanceur.0.8.1.setup.exe' 

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  fileType      =  'EXE' 
  url           = $url

  softwareName  = 'lanceur' 

  checksum      = 'E9D049645C7A3BCA0EB6337DC44E35E837AC2308E6F6EB1EFEF0EBDD5B8197EA'
  checksumType  = 'sha256' 

  validExitCodes= @(0, 3010, 1641)
  silentArgs   = '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /SP-' 
}

Install-ChocolateyPackage @packageArgs 
