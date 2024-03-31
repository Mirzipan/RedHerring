#!/bin/bash

rm -rf Template/Libraries

mkdir Template/Libraries

cp src/RedHerring/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Alexandria/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Assets/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Clues/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Deduction/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Inputs/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Incident/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Infusion/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Motive/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Numbers/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Platforms/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Render/bin/Release/net8.0/*.dll Template/Libraries/
cp src/RedHerring.Serialization/bin/Release/net8.0/*.dll Template/Libraries/

echo "=============== DONE ==================="
read -p "Press enter to close"