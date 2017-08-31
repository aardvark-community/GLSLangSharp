
all: deploy

deploy: libGLSLangNative.so
	cp libGLSLangNative.so ../../libs/Native/GLSLangSharp/linux/AMD64/
	cp libGLSLangNative.so ../../bin/Debug/
	cp libGLSLangNative.so ../../bin/Release/
	
glslang.o: glslang.cpp glslang.h
	g++ -I ../../libs/include -std=c++11 -fPIC -c glslang.cpp -o glslang.o

libGLSLangNative.so: glslang.o
	g++ glslang.o -shared -o libGLSLangNative.so -lglslang -lHLSL -lOGLCompiler -lOSDependent -lSPIRV -lSPVRemapper

.PHONY clean:
	rm -fr *.o libGLSLangNative.so
	rm ../../libs/Native/GLSLangSharp/linux/AMD64/libGLSLangNative.so
	rm ../../bin/Debug/libGLSLangNative.so
	rm ../../bin/Release/libGLSLangNative.so
