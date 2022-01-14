#!/bin/sh

os=$(uname -s)
if [ $os == "Darwin" ];
then
    cmake -DCMAKE_OSX_ARCHITECTURES=$1 -S src/GLSLangNative -B bin/$1
    cmake --build bin/$1 --config Release
    cmake --install bin/$1 --config Release

else
    cmake -S src/GLSLangNative -B bin/build
    cmake --build bin/build --config Release
    cmake --install bin/build --config Release
fi
