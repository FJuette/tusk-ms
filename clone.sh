#!/bin/bash
#Replace Magnus.Api with desired name and the following foldername
FOLDERNAME='portal-auth'
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
mv Tusk.sln Portal.Auth.sln
find . -depth -name '*Tusk.Story*' -type d -execdir rename 'y/Tusk\.Story/Portal\.Auth/' {} +
find . -depth -type d -name '*Tusk.Story*' -exec mv {} 'Portal.Auth' \;
find . -type f -name '*Tusk.Story*' -print0 | xargs -0 -n1 bash -c 'mv "$0" "${0/Portal.Auth/Portal.Auth.Api}"'
find . -type f -name '*' -exec sed -i "" 's/Tusk\.Story/Portal\.Auth\.Api/g' {} +
find . -type f -name '*' -exec sed -i "" 's/Tusk/Portal\.Auth/g' {} +
