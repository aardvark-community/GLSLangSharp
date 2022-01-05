rm -dfr build
cmake -DCMAKE_BUILD_TYPE=MinSizeRel -S . -B build
cd build
make
make install