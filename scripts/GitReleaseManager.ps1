param(
    $token,
    $owner,
    $name,
    $milestone,
    $assets,
    $repo,
    $isPrerelease
)

$assetsAsString = '"' + $($assets -join '","') + '"'
$cmd = "grm create --milestone $milestone --token $token --owner $owner --repository $repo --name $name --assets $assetsAsString"

if($isPrerelease -eq $true){
    $cmd = $cmd + " --pre"
}
else{
    Write-Host "Not a PRE-RELEASE"
}

iex $cmd