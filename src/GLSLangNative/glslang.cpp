// spirv.cpp : Defines the exported functions for the DLL application.
//

#ifndef __GNUC__
#include "stdafx.h"
#endif
#include "glslang.h"


static TLimits defLimits =
	{
		/* nonInductiveForLoops */ true,
		/* whileLoops */ true,
		/* doWhileLoops */ true,
		/* generalUniformIndexing */ true,
		/* generalAttributeMatrixVectorIndexing */ true,
		/* generalVaryingIndexing */ true,
		/* generalSamplerIndexing */ true,
		/* generalVariableIndexing */ true,
		/* generalConstantMatrixVectorIndexing */ true
	};

static TBuiltInResource defRes =
	{
		/* maxLights */ 32,
		/* maxClipPlanes */ 6,
		/* maxTextureUnits */ 32,
		/* maxTextureCoords */ 32,
		/* maxVertexAttribs */ 64,
		/* maxVertexUniformComponents */ 4096,
		/* maxVaryingFloats */ 64,
		/* maxVertexTextureImageUnits */ 32,
		/* maxCombinedTextureImageUnits */ 80,
		/* maxTextureImageUnits */ 32,
		/* maxFragmentUniformComponents */ 4096,
		/* maxDrawBuffers */ 32,
		/* maxVertexUniformVectors */ 128,
		/* maxVaryingVectors */ 8,
		/* maxFragmentUniformVectors */ 16,
		/* maxVertexOutputVectors */ 16,
		/* maxFragmentInputVectors */ 15,
		/* minProgramTexelOffset */ 8,
		/* maxProgramTexelOffset */ 7,
		/* maxClipDistances */ 8,
		/* maxComputeWorkGroupCountX */ 65535,
		/* maxComputeWorkGroupCountY */ 65535,
		/* maxComputeWorkGroupCountZ */ 65535,
		/* maxComputeWorkGroupSizeX */ 1024,
		/* maxComputeWorkGroupSizeY */ 1024,
		/* maxComputeWorkGroupSizeZ */ 64,
		/* maxComputeUniformComponents */ 1024,
		/* maxComputeTextureImageUnits */ 16,
		/* maxComputeImageUniforms */ 8,
		/* maxComputeAtomicCounters */ 8,
		/* maxComputeAtomicCounterBuffers */ 1,
		/* maxVaryingComponents */ 60 ,
		/* maxVertexOutputComponents */ 64,
		/* maxGeometryInputComponents */ 64,
		/* maxGeometryOutputComponents */ 128,
		/* maxFragmentInputComponents */ 128,
		/* maxImageUnits */ 8,
		/* maxCombinedImageUnitsAndFragmentOutputs */ 8,
		/* maxCombinedShaderOutputResources */ 8,
		/* maxImageSamples */ 0,
		/* maxVertexImageUniforms */ 0,
		/* maxTessControlImageUniforms */ 0,
		/* maxTessEvaluationImageUniforms */ 0,
		/* maxGeometryImageUniforms */ 0,
		/* maxFragmentImageUniforms */ 8,
		/* maxCombinedImageUniforms */ 8,
		/* maxGeometryTextureImageUnits */ 16,
		/* maxGeometryOutputVertices */ 256,
		/* maxGeometryTotalOutputComponents */ 1024,
		/* maxGeometryUniformComponents */ 1024,
		/* maxGeometryVaryingComponents */ 64,
		/* maxTessControlInputComponents */ 128,
		/* maxTessControlOutputComponents */ 128,
		/* maxTessControlTextureImageUnits */ 16,
		/* maxTessControlUniformComponents */ 1024,
		/* maxTessControlTotalOutputComponents */ 4096,
		/* maxTessEvaluationInputComponents */ 128,
		/* maxTessEvaluationOutputComponents */ 128,
		/* maxTessEvaluationTextureImageUnits */ 16,
		/* maxTessEvaluationUniformComponents */ 1024,
		/* maxTessPatchComponents */ 120,
		/* maxPatchVertices */ 32,
		/* maxTessGenLevel */ 64,
		/* maxViewports */ 16,
		/* maxVertexAtomicCounters */ 0,
		/* maxTessControlAtomicCounters */ 0,
		/* maxTessEvaluationAtomicCounters */ 0,
		/* maxGeometryAtomicCounters */ 0,
		/* maxFragmentAtomicCounters */ 8,
		/* maxCombinedAtomicCounters */ 8,
		/* maxAtomicCounterBindings */ 1,
		/* maxVertexAtomicCounterBuffers */ 0,
		/* maxTessControlAtomicCounterBuffers */ 0,
		/* maxTessEvaluationAtomicCounterBuffers */ 0,
		/* maxGeometryAtomicCounterBuffers */ 0,
		/* maxFragmentAtomicCounterBuffers */ 1,
		/* maxCombinedAtomicCounterBuffers */ 1,
		/* maxAtomicCounterBufferSize */ 16384,
		/* maxTransformFeedbackBuffers */ 4,
		/* maxTransformFeedbackInterleavedComponents */ 64,
		/* maxCullDistances */ 8,
		/* maxCombinedClipAndCullDistances */ 8,
		/* maxSamples */ 4,
		/* .maxMeshOutputVerticesNV = */ 256,
        /* .maxMeshOutputPrimitivesNV = */ 512,
        /* .maxMeshWorkGroupSizeX_NV = */ 32,
        /* .maxMeshWorkGroupSizeY_NV = */ 1,
        /* .maxMeshWorkGroupSizeZ_NV = */ 1,
        /* .maxTaskWorkGroupSizeX_NV = */ 32,
        /* .maxTaskWorkGroupSizeY_NV = */ 1,
        /* .maxTaskWorkGroupSizeZ_NV = */ 1,
        /* .maxMeshViewCountNV = */ 4,
		/* maxDualSourceDrawBuffersEXT */ 4,
		defLimits
	};


static int failProgram(glslang::TProgram& program, int* size, char** logBuffer)
{
	if (logBuffer)
	{
		auto log = program.getInfoLog();
		auto logLen = strlen(log);
		*logBuffer = new char[logLen];
		*size = (int)logLen;
		memcpy(*logBuffer, log, logLen);
	}
	return 1;
}

static int failShader(glslang::TShader& shader, int* size, char** logBuffer)
{
	if (logBuffer)
	{
		auto log = shader.getInfoLog();
		auto logLen = strlen(log);
		*logBuffer = new char[logLen];
		*size = (int)logLen;
		memcpy(*logBuffer, log, logLen);
	}
	return 1;
}


DllExport(bool) ShInitializeProcess()
{
	return ShInitialize() != 0;
}

DllExport(void) ShFinalizeProcess()
{
	ShFinalize();
}

DllExport(int) ShCompileShader(EShLanguage language, glslang::EShTargetLanguageVersion version,
							   const char* entryName, const char* code, int nDefines, const char* defines[],
							   size_t* outputSize, void** output, int* logLength, char** log)
{
	glslang::TShader shader(language);
	shader.setStrings(&code, 1);
	shader.setEnvTarget(glslang::EShTargetSpv, version);

	std::string preamble;
	if (nDefines > 0)
	{
		for (int i = 0; i < nDefines; i++)
		{
			preamble.append("#define ");
			preamble.append(defines[i]);
			preamble.append("\r\n");
		}
		shader.setPreamble(preamble.c_str());
	}
	shader.setEntryPoint(entryName);


	auto msg = (EShMessages) (EShMsgVulkanRules | EShMsgSpvRules);
	if (shader.parse(&defRes, 140, ECompatibilityProfile, false, false, msg))
	{
		glslang::TProgram program;
		program.addShader(&shader);

		if (program.link(EShMsgDefault))
		{
			auto intermediate = program.getIntermediate(language);
			if (!intermediate) {
				*output = nullptr;
				*outputSize = 0;
				return failProgram(program, logLength, log);
			}

			std::vector<unsigned int> spirv;
			glslang::GlslangToSpv(*intermediate, spirv);

			auto intSize = spirv.size();

			if (intSize <= 0) {
				*output = nullptr;
				*outputSize = 0;
				return failProgram(program, logLength, log);
			}

			auto result = new int[intSize];
			std::copy(spirv.begin(), spirv.end(), result);

			*output = (void*)result;
			*outputSize = 4 * intSize;

			if(log)
			{
				std::string str;

				auto shaderLog = std::string(shader.getInfoLog());
				auto programLog = std::string(program.getInfoLog());

				if (!shaderLog.empty()) { str += "#SHADER\r\n" + shaderLog; }
				if (!programLog.empty()) { str += "#PROGRAM\r\n" + programLog; }

				auto len = str.length() + 1;
				auto temp = new char[len];
				strncpy(temp, str.c_str(), len);
				*logLength = (int)(len - 1);
				*log = temp;
			}


			return 0;
		}
		else
		{
			*output = nullptr;
			*outputSize = 0;
			return failProgram(program, logLength, log);
		}
	}
	else
	{
		*output = nullptr;
		*outputSize = 0;
		return failShader(shader, logLength, log);
	}

}

DllExport(void) ShFree(void* memory)
{
	delete[] (int*)memory;
}

std::string CanonicalizeFlag(const char* const* argv, int argc, int* argi) {
	const char* cur_arg = argv[*argi];
	const char* next_arg = (*argi + 1 < argc) ? argv[*argi + 1] : nullptr;
	std::ostringstream canonical_arg;
	canonical_arg << cur_arg;

	// NOTE: DO NOT ADD NEW FLAGS HERE.
	//
	// These flags are supported for backwards compatibility.  When adding new
	// passes that need extra arguments in its command-line flag, please make them
	// use the syntax "--pass_name[=pass_arg].
	if (0 == strcmp(cur_arg, "--set-spec-const-default-value") ||
		0 == strcmp(cur_arg, "--loop-fission") ||
		0 == strcmp(cur_arg, "--loop-fusion") ||
		0 == strcmp(cur_arg, "--loop-unroll-partial") ||
		0 == strcmp(cur_arg, "--loop-peeling-threshold")) {
		if (next_arg) {
			canonical_arg << "=" << next_arg;
			++(*argi);
		}
	}

	return canonical_arg.str();
}

DllExport(void) ShOptimize(const void* input, const size_t inputLength, void** output, size_t* outputLength, const char* const* passNames, int32_t passCount)
{
	spvtools::Optimizer opt(SPV_ENV_UNIVERSAL_1_3);
	if (passNames)
	{
		std::vector<std::string> passes;
		int i = 0;
		while (i < passCount) {
			auto n = CanonicalizeFlag(passNames, passCount, &i);
			passes.push_back(n);
			i++;
		}
		opt.RegisterPassesFromFlags(passes);
	}
	else
	{
		opt.RegisterPerformancePasses();
	}

	std::vector<uint32_t> result;
	if (opt.Run((const uint32_t*)input, inputLength / sizeof(uint32_t), &result))
	{
		auto res = new uint32_t[result.size()];
		std::copy(result.begin(), result.end(), res);
		*output = (void*)res;
		*outputLength = result.size() * sizeof(uint32_t);
	}
	else
	{
		auto res = new char[inputLength];
		memcpy(res, input, inputLength);
		*output = (void*)res;
		*outputLength = inputLength;
	}

}
