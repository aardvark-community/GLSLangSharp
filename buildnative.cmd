@echo off
cmake -S src\GLSLangNative -B bin\build
cmake --build bin\build --config Release
cmake --install bin\build --config Release