#
# Apworks Examples Build Script
# Task List
# Copyright (C) by daxnet, all rights reserved.
# Licensed under Apache 2.0
#

$workspace = ($PSScriptRoot).ToString()
if (Test-Path -Path $workspace/publish) {
	Remove-Item -Recurse -Force $workspace/publish
}
dotnet restore -s https://api.nuget.org/v3/index.json -s https://www.myget.org/F/daxnet-utils/api/v3/index.json -s https://www.myget.org/F/daxnet-apworks-pre/api/v3/index.json
dotnet publish -c Release -o publish

if (!(Test-Path -Path $workspace/publish/wwwroot)) {
	New-Item -ItemType Directory -Force -Path $workspace/publish/wwwroot
}

cd $workspace/client
npm install
ng build -e docker -op $workspace/publish/wwwroot
