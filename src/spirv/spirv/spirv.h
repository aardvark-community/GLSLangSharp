#pragma once

#include "ShaderLang.h"

#if _WIN32
#define DllExport(t) extern "C" __declspec(dllexport) t
#else
#define DllExport(t) extern "C" t
#endif



// general functions
DllExport(bool) ShInitializeProcess();
DllExport(void) ShFinalizeProcess();


// shader functions
DllExport(glslang::TShader*) ShCreateShader(EShLanguage language);
DllExport(void) ShDestroyShader(glslang::TShader* shader);
DllExport(bool) ShSetShaderSource(glslang::TShader* shader, const char* const* shaderStrings, int* shaderStringLength, int numSources);
DllExport(bool) ShParseShader(glslang::TShader* shader, const TBuiltInResource* resources, int defaultVersion, bool forwardCompatible, EShMessages messages);
DllExport(const char*) ShGetShaderInfoLog(glslang::TShader* shader);
DllExport(const char*) ShGetShaderInfoDebugLog(glslang::TShader* shader);

// program functions
DllExport(glslang::TProgram*) ShCreateProgram();
DllExport(void) ShDestroyProgram(glslang::TProgram* program);
DllExport(bool) ShAddShader(glslang::TProgram* program, glslang::TShader* shader);
DllExport(bool) ShLinkProgram(glslang::TProgram* program, EShMessages messages);
DllExport(const char*) ShGetProgramInfoLog(glslang::TProgram* program);
DllExport(const char*) ShGetProgramInfoDebugLog(glslang::TProgram* program);


// spirv functions
DllExport(glslang::TIntermediate*) ShGetIntermediate(glslang::TProgram* program, EShLanguage stage);
DllExport(void*) ShGetSpirVForProgramStage(glslang::TProgram* program, EShLanguage stage, size_t* size);
DllExport(void) ShFreeSpirV(void* data, size_t size);


// deprecated
DllExport(int) ShCompileSpirV(EShLanguage language, const char* shaderString, int shaderStringLength, const TBuiltInResource* resources, int defaultVersion, bool forwardCompatible, EShMessages messages, void* buffer, int* bufferSize);
