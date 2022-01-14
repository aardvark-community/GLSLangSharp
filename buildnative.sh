#!/bin/sh

os=$(uname -s)
echo $os
if [ $os == "Darwin" ];
then
    cmake -DCMAKE_OSX_ARCHITECTURES=arm64 -S src/GLSLangNative -B bin/arm64
    cmake --build bin/arm64 --config Release
    cmake --install bin/arm64 --config Release

    cmake -DCMAKE_OSX_ARCHITECTURES=x86_64 -S src/GLSLangNative -B bin/x86_64
    cmake --build bin/x86_64 --config Release
    cmake --install bin/x86_64 --config Release
else
    cmake -S src/Native -B bin/build
    cmake --build bin/build --config Release
    cmake --install bin/build --config Release
fi
