# Building on Linux #
* Get and build `glslang` (https://github.com/KhronosGroup/glslang)
    * `./update_glslang_sources.py`
    * `mkdir build && cd build`
    * `cmake -DCMAKE_BUILD_TYPE=Release -DCMAKE_INSTALL_PREFIX="$(pwd)/install" ..`
    * `make -j8 && make install`
* Copy the contents of the glslang `install` folder (should contain `include` and `lib`) to the GLSLangNative source folder
* Build with `make`