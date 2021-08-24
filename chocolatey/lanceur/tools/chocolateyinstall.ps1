
$ErrorActionPreference = 'Stop'; 
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$url        = 'https://github.com/jibedoubleve/lanceur/releases/download/0.8.1/lanceur.0.8.1.setup.exe' 

$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  unzipLocation = $toolsDir
  fileType      =  'EXE' 
  url           = $url

  softwareName  = 'lanceur' 

  checksum      = '988A3FF9A3C4E313019BEDC74BD46D21A00CB5901A4AB7E4C281CFDBDD8299C2'
  checksumType  = 'sha256' 

  validExitCodes= @(0, 3010, 1641)
  silentArgs   = '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /SP-' 
}

Install-ChocolateyPackage @packageArgs 
