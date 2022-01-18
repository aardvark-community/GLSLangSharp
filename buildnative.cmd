@echo off
cmake -S src\GLSLangNative -B bin\build
cmake --build bin\build --config MinSizeRel
cmake --install bin\build --config MinSizeRel