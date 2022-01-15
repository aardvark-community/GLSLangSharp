#!/bin/sh

os=$(uname -s)
if [ $os == "Darwin" ];
then
    arch=$1;
    if [ "$arch" == "" ];
    then
        arch=$(uname -m)
    fi
    cmake -DCMAKE_OSX_ARCHITECTURES=$arch -S src/GLSLangNative -B bin/$arch
    cmake --build bin/$arch --config Release
    cmake --install bin/$arch --config Release

else
    cmake -S src/GLSLangNative -B bin/build
    cmake --build bin/build --config Release
    cmake --install bin/build --config Release
fi
