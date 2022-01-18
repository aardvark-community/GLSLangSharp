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
    cmake --build bin/$arch -j4
    cmake --install bin/$arch 

else
    cmake -S src/GLSLangNative -B bin/build
    cmake --build bin/build -j4
    cmake --install bin/build
fi
