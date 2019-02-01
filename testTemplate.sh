#!/usr/bin/env bash

## Supported by
## Cheer 100 svavablount February 1, 2019
## Cheer 1000 Auth0bobby February 1, 2019
## Cheer 100 devlead February 1, 2019
## Cheer 100 ramblinggeek February 1, 2019

set -e
set -o pipefail 
set -o nounset

dotnet new -i Templates/StreamDeck.PluginTemplate.Csharp

dotnet new streamdeck-plugin -o testPlugin -pn IntegrationTestPlugin -uu test.plugin.integrationtest
cd testPlugin

dotnet restore -s $1

dotnet build
