#!/usr/bin/env bash

set -e
set -o pipefail
set -o nounset

dotnet new -i Templates/StreamDeck.PluginTemplate.Csharp

dotnet new streamdeck-plugin -o testPlugin -pn IntegrationTestPlugin -uu test.plugin.integrationtest
cd testPlugin

dotnet restore -s $1

dotnet build
