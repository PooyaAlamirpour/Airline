#!/usr/bin/env bash
set -ex
#cd $(dirname $0)/..
FOLDER=$BUILD_REPOSITORY_LOCALPATH

for PROJECT in $(ls -1 ./tests | grep -e RestAirline | grep -v TestsHelper)
do
  docker run --rm -v $FOLDER:/project -w /project microsoft/dotnet:2.2.103-sdk dotnet test tests/$PROJECT -v minimal
done