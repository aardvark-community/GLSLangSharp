


deploy: libspirv.so
	mkdir -p ../../libs/Native/GLSLangSharp/linux/AMD64
	cp libspirv.so ../../libs/Native/GLSLangSharp/linux/AMD64/libspirv.so

libspirv.so: spirv.o
	g++ spirv.o -L glslang/lib -shared -lSPIRV -lglslang -lOGLCompiler -lOSDependent -lpthread -o libspirv.so

spirv.o: spirv/spirv.h spirv/spirv.cpp
	g++ -c -std=c++11 spirv/spirv.cpp -fPIC -o spirv.o -I glslang/include

.PHONY clean:
	rm -fr spirv.o libspirv.so
