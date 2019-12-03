#!/bin/bash
OLDNAME='Tusk'
NEWNAME='Visages'
rm -Rf ./../$NEWNAME
cp -rf ./../tusk-ms ./../$NEWNAME
cd ./../$NEWNAME
rm -Rf .git
rm -Rf .vs
rm -Rf ./src/$OLDNAME.Story/obj
rm -Rf ./src/$OLDNAME.Story/bin
rm -Rf ./tests/$OLDNAME.Story.Tests/obj
rm -Rf ./tests/$OLDNAME.Story.Tests/bin
find . -type d -name '*'$OLDNAME'*' -print0 | xargs -0 -n1 bash -c 'mv "$0" "${0/'$OLDNAME'/'$NEWNAME}'"';
find . -type f -name '*'$OLDNAME'*' -print0 | xargs -0 -n1 bash -c 'mv "$0" "${0/'$OLDNAME'/'$NEWNAME}'"'
find . -type f -name '*' -exec sed -i "" 's/'$OLDNAME'/'$NEWNAME'/g' {} +