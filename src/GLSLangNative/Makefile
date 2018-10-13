
all: deploy

deploy: libGLSLangNative.so
	cp libGLSLangNative.so ../../libs/Native/GLSLangSharp/linux/AMD64/
	cp libGLSLangNative.so ../../bin/Debug/
	cp libGLSLangNative.so ../../bin/Release/
	cp libGLSLangNative.so ../../bin/Debug/netcoreapp2.0
	cp libGLSLangNative.so ../../bin/Release/netcoreapp2.0
	
glslang.o: glslang.cpp glslang.h
	g++ -I  /usr/local/include/SPIRV/ -I /usr/local/include/glslang/Public -I /usr/local/include/glslang/Include -std=c++11 -fPIC -c glslang.cpp -o glslang.o

libGLSLangNative.so: glslang.o
	g++ glslang.o -shared -o libGLSLangNative.so -lglslang -lHLSL -lOGLCompiler -lOSDependent -lSPIRV -lSPVRemapper -lSPIRV-Tools-opt -lSPIRV-Tools -L/home/hs/glslang/build/install/lib


.PHONY clean:
	rm -fr *.o libGLSLangNative.so
	rm ../../libs/Native/GLSLangSharp/linux/AMD64/libGLSLangNative.so
	rm ../../bin/Debug/libGLSLangNative.so
	rm ../../bin/Release/libGLSLangNative.so
