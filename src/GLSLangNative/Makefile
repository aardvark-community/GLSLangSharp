
all: deploy

deploy: libGLSLangNative.so
	cp libGLSLangNative.so ../../libs/Native/GLSLangSharp/linux/AMD64/

glslang.o: glslang.cpp glslang.h
	g++ -I ./include -std=c++11 -fPIC -c glslang.cpp -o glslang.o

libGLSLangNative.so: glslang.o
	gcc glslang.o -shared -o libGLSLangNative.so -lglslang -lHLSL -Wl,--whole-archive -lOGLCompiler lib/libOSDependent.a -Wl,--no-whole-archive -lSPIRV -lMachineIndependent -lSPVRemapper -lSPIRV-Tools-opt -lSPIRV-Tools -L./lib 


.PHONY clean:
	rm -fr *.o libGLSLangNative.so
	rm ../../libs/Native/GLSLangSharp/linux/AMD64/libGLSLangNative.so
	rm ../../bin/Debug/libGLSLangNative.so
	rm ../../bin/Release/libGLSLangNative.so
