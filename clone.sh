#!/bin/bash
#Replace Magnus.Api with desired name and the following foldername
FOLDERNAME='Projects'
rm -Rf ./../$FOLDERNAME
cp -rf ./../tusk-ms ./../$FOLDERNAME
cd ./../$FOLDERNAME
pwd
rm -Rf ./.git
rm -Rf ./.vs
rm -Rf ./src/Tusk.Story/obj
rm -Rf ./src/Tusk.Story/bin
rm -Rf ./tests/Tusk.Story.Tests/obj
rm -Rf ./tests/Tusk.Story.Tests/bin
mv Tusk.sln Projects.sln
find . -type d -name '*Tusk.Story*' -print0 | xargs -0 -n1 bash -c 'mv "$0" "${0/Tusk.Story/Projects.Api}"'
find . -type f -name '*Tusk.Story*' -print0 | xargs -0 -n1 bash -c 'mv "$0" "${0/Tusk.Story/Projects.Api}"'
find . -type f -name '*' -exec sed -i "" 's/Tusk\.Story/Projects\.Api/g' {} +
find . -type f -name '*' -exec sed -i "" 's/Tusk/Projects/g' {} +
