#!/bin/bash
dotnet restore
dotnet publish -c Release

RELEASE_PATH="./CompleteInformation.RecipeModule.AvaloniaApp/bin/Release/netcoreapp2.0/publish"
cp -r "CompleteInformation.RecipeModule.ConsoleApp/bin/Release/netcoreapp2.0/publish/." "$RELEASE_PATH"
cp "README.md" "$RELEASE_PATH"
cp "LICENSE.txt" "$RELEASE_PATH"
PRIOR_PATH=$PWD
cd $RELEASE_PATH
zip -r "$PRIOR_PATH/release.zip" .
cd $PRIOR_PATH
