#!/bin/bash

rm -f Studio/*
rm -rf Studio/runtimes
rm -rf Studio/Template

mkdir Studio

cp src/RedHerring.Studio/bin/Release/net8.0/*.dll Studio/
cp src/RedHerring.Studio/bin/Release/net8.0/*.exe Studio/
cp src/RedHerring.Studio/bin/Release/net8.0/*.json Studio/

mkdir Studio/runtimes
cp -r src/RedHerring.Studio/bin/Release/net8.0/runtimes/* Studio/runtimes/

mkdir Studio/Template

cp Template/*.sln Studio/Template/
cp Template/*.csproj Studio/Template/
cp Template/*.cs Studio/Template/
cp Template/Project.json Studio/Template/

mkdir Studio/Template/Assets
mkdir Studio/Template/Resources
mkdir Studio/Template/Libraries

rm -rf Studio/Template/GameExecutable/obj
rm -rf Studio/Template/GameLibrary/obj

cp -r Template/Libraries/* Studio/Template/Libraries/

echo "=============== DONE ==================="
read -p "Press enter to close"