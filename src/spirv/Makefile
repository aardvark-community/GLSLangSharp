


deploy: libspirv.so
	mkdir -p ../../libs/Native/GLSLangSharp/linux/AMD64
	cp libspirv.so ../../libs/Native/GLSLangSharp/linux/AMD64/libGLSLangNative.so

libspirv.so: spirv.o
	g++ spirv.o -L glslang/lib -shared -lSPIRV -lglslang -lOGLCompiler -lOSDependent -lHLSL -lpthread -o libspirv.so

spirv.o: GLSLangNative/spirv.h GLSLangNative/spirv.cpp
	g++ -c -std=c++11 GLSLangNative/spirv.cpp -fPIC -o spirv.o -I glslang/include

.PHONY clean:
	rm -fr spirv.o libspirv.so
