#
# build_all.ps1
#

$workspace = ($PSScriptRoot).ToString()
if (Test-Path -Path $workspace\publish) {
	Remove-Item -Recurse -Force $workspace\publish
}

dotnet publish -c Release -o publish

if (!(Test-Path -Path $workspace\publish\wwwroot)) {
	New-Item -ItemType Directory -Force -Path $workspace\publish\wwwroot
}

cd $workspace\client
ng build -e prod -op $workspace\publish\wwwroot
