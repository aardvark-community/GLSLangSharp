#pragma once

#include<vector>
#include <ShaderLang.h>
#include <GlslangToSpv.h>


#if _WIN32
#define DllExport(t) extern "C" __declspec(dllexport) t
#else
#define DllExport(t) extern "C" t
#endif




// general functions
DllExport(bool) ShInitializeProcess();
DllExport(void) ShFinalizeProcess();
DllExport(int) ShCompileShader(EShLanguage lang, const char* entryName, const char* code, int nDefines, const char* defines[], size_t* outputSize, void** output, int* logLength, char** log);
DllExport(void) ShFree(void* memory);