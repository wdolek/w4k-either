dotnet clean -c Debug
dotnet clean -c Release

$rootDirectory = $PWD.Path

# Remove obj directories
$objDirectories = Get-ChildItem -Path $rootDirectory -Filter "obj" -Recurse -Directory
foreach ($objDir in $objDirectories) {
    Write-Host "Removing: $($objDir.FullName)"
    Remove-Item -Path $objDir.FullName -Force -Recurse
}

# Remove bin directories
$binDirectories = Get-ChildItem -Path $rootDirectory -Filter "bin" -Recurse -Directory
foreach ($binDir in $binDirectories) {
    Write-Host "Removing: $($binDir.FullName)"
    Remove-Item -Path $binDir.FullName -Force -Recurse
}

# Remove .idea folder
$ideaFolder = Join-Path -Path $rootDirectory -ChildPath ".idea"
if (Test-Path -Path $ideaFolder) {
    Write-Host "Removing: $ideaFolder"
    Remove-Item -Path $ideaFolder -Force -Recurse
}
