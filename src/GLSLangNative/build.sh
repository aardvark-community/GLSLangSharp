rm -dfr build
cmake -S . -B build
cd build
make
make install