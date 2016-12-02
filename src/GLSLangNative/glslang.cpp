// spirv.cpp : Defines the exported functions for the DLL application.
//


#include "stdafx.h"
#include "glslang.h"

DllExport(bool) ShInitializeProcess()
{
	return ShInitialize() != 0;
}

DllExport(void) ShFinalizeProcess()
{
	ShFinalize();
}



DllExport(glslang::TShader*) ShCreateShader(EShLanguage language)
{
	if (language < 0 || language >= EShLangCount) return nullptr;
	return new glslang::TShader(language);
}

DllExport(void) ShDestroyShader(glslang::TShader* shader)
{
	if (shader) delete shader;
}

DllExport(bool) ShSetShaderSource(glslang::TShader* shader, const char* const* shaderStrings, int* shaderStringLength, int numSources)
{
	if (!shader || !shaderStrings || !shaderStringLength) return false;

	shader->setStringsWithLengths(shaderStrings, shaderStringLength, numSources);
	return true;
}

DllExport(bool) ShParseShader(glslang::TShader* shader, const TBuiltInResource* resources, int defaultVersion, EProfile defaultProfile, int forceDefault, int forwardCompatible, EShMessages messages)
{
	if (!shader || !resources) return false;
	return shader->parse(resources, defaultVersion, defaultProfile, (forceDefault != 0), (forwardCompatible != 0), messages);
	//return shader->parse(resources, defaultVersion, forwardCompatible, messages);
}

DllExport(const char*) ShGetShaderInfoLog(glslang::TShader* shader)
{
	if (!shader) return nullptr;

	return shader->getInfoLog();
}

DllExport(const char*) ShGetShaderInfoDebugLog(glslang::TShader* shader)
{
	if (!shader) return nullptr;

	return shader->getInfoDebugLog();
}



DllExport(glslang::TProgram*) ShCreateProgram()
{
	return new glslang::TProgram;
}

DllExport(void) ShDestroyProgram(glslang::TProgram* program)
{
	if (program) delete program;
}

DllExport(bool) ShAddShader(glslang::TProgram* program, glslang::TShader* shader)
{
	if (!program || !shader) return false;

	program->addShader(shader);
	return true;
}

DllExport(bool) ShLinkProgram(glslang::TProgram* program, EShMessages messages)
{
	if (!program)return false;

	return program->link(messages);
}

DllExport(const char*) ShGetProgramInfoLog(glslang::TProgram* program)
{
	if (!program)return nullptr;
	return program->getInfoLog();
}

DllExport(const char*) ShGetProgramInfoDebugLog(glslang::TProgram* program)
{
	if (!program)return nullptr;
	return program->getInfoDebugLog();
}


DllExport(void*) ShGetSpirVForProgramStage(glslang::TProgram* program, EShLanguage stage, size_t* size)
{
	if (!program) {
		*size = 0;  return nullptr;
	}

	auto intermediate = program->getIntermediate(stage);
	if (!intermediate) {
		*size = 0; return nullptr;
	}

	std::vector<unsigned int> spirv;
	glslang::GlslangToSpv(*intermediate, spirv);

	if (spirv.size() == 0) {
		*size = 0; return nullptr;
	}

	auto result = new unsigned int[spirv.size()];

	for (int i = 0; i < (int)spirv.size(); i++)
		result[i] = spirv[i];

	*size = 4 * spirv.size();
	return (void*)result;
}

DllExport(void) ShFreeSpirV(void* data, size_t size)
{
	auto ptr = (unsigned int*)data;
	delete ptr;
}
