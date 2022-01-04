#pragma once

#include <glslang/Public/ShaderLang.h>
#include <SPIRV/GlslangToSpv.h>
#include <spirv-tools/optimizer.hpp>
#include <sstream>
#include <vector>
#include <algorithm>
#include <string.h>


#if _WIN32
#define DllExport(t) extern "C" __declspec(dllexport) t
#else
#define DllExport(t) extern "C" t
#endif




// general functions
DllExport(bool) ShInitializeProcess();
DllExport(void) ShFinalizeProcess();
DllExport(int) ShCompileShader(EShLanguage language, glslang::EShTargetLanguageVersion version,
							   const char* entryName, const char* code, int nDefines, const char* defines[],
							   size_t* outputSize, void** output, int* logLength, char** log);
DllExport(void) ShFree(void* memory);
DllExport(void) ShOptimize(const void* input, const size_t inputLength, void** output, size_t* outputLength, const char* const* passNames, int32_t passCount);
