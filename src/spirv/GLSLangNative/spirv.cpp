// spirv.cpp : Defines the exported functions for the DLL application.
//



#include "stdafx.h"
#include "spirv.h"

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
	if(shader) delete shader;
}

DllExport(bool) ShSetShaderSource(glslang::TShader* shader, const char* const* shaderStrings, int* shaderStringLength, int numSources)
{
	if (!shader || !shaderStrings || !shaderStringLength) return false;

	//printf("source: %s\n", shaderString);

	shader->setStringsWithLengths(shaderStrings, shaderStringLength, numSources);
	return true;
}

DllExport(bool) ShParseShader(glslang::TShader* shader, const TBuiltInResource* resources, int defaultVersion, bool forwardCompatible, EShMessages messages)
{
	if (!shader || !resources) return false;

	return shader->parse(resources, defaultVersion, forwardCompatible, messages);
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
	if(program) delete program;
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

	for (int i = 0; i < spirv.size(); i++)
		result[i] = spirv[i];

	*size = 4 * spirv.size();
	return (void*)result;
}

DllExport(void) ShFreeSpirV(void* data, size_t size)
{
	auto ptr = (unsigned int*)data;
	delete ptr;
}



DllExport(int) ShCompileSpirV(EShLanguage language, const char* shaderString, int shaderStringLength, const TBuiltInResource* resources, int defaultVersion, bool forwardCompatible, EShMessages messages, void* buffer, int* bufferSize)
{
	if (!shaderString)
		return -1;

	
	glslang::TShader* shader = new glslang::TShader(language);

	shader->setStringsWithLengths(&shaderString, &shaderStringLength, 1);

	if (!shader->parse(resources, defaultVersion, forwardCompatible, messages))
	{
		delete shader;
		return -1;
	}

	glslang::TProgram& program = *new glslang::TProgram;
	program.addShader(shader);


	if (!program.link(messages))
	{
		delete &program;
		delete shader;
		return -4;
	}


	for (int stage = 0; stage < EShLangCount; ++stage) 
	{
		if (program.getIntermediate((EShLanguage)stage)) 
		{
			std::vector<unsigned int> spirv;
			glslang::GlslangToSpv(*program.getIntermediate((EShLanguage)stage), spirv);


			if (spirv.size() > 0)
			{
				auto byteSize = 4 * spirv.size();
				auto errCode = 0;

				if (*bufferSize >= byteSize && buffer != nullptr)
				{
					unsigned int* bb = (unsigned int*) buffer;

					for (int i = 0; i < spirv.size(); i++)
						bb[i] = spirv[i];

				}
				else if(buffer != nullptr)
				{
					errCode = -3;
				}


				*bufferSize = (int)byteSize;

				delete &program;
				delete shader;
				return errCode;
			}
		}
	}





	delete &program;
	delete shader;
	return -2;
}