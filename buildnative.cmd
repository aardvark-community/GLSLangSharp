@echo off
cmake -S src/Native -B bin/build
cmake --build bin/build --config Release
cmake --install bin/build --config Release