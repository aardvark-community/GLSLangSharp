// spirv.cpp : Defines the exported functions for the DLL application.
//

#ifndef __GNUC__
#include "stdafx.h"
#endif
#include "glslang.h"


const TBuiltInResource defRes = {
    /* .MaxLights = */ 32,
    /* .MaxClipPlanes = */ 6,
    /* .MaxTextureUnits = */ 32,
    /* .MaxTextureCoords = */ 32,
    /* .MaxVertexAttribs = */ 64,
    /* .MaxVertexUniformComponents = */ 4096,
    /* .MaxVaryingFloats = */ 64,
    /* .MaxVertexTextureImageUnits = */ 32,
    /* .MaxCombinedTextureImageUnits = */ 80,
    /* .MaxTextureImageUnits = */ 32,
    /* .MaxFragmentUniformComponents = */ 4096,
    /* .MaxDrawBuffers = */ 32,
    /* .MaxVertexUniformVectors = */ 128,
    /* .MaxVaryingVectors = */ 8,
    /* .MaxFragmentUniformVectors = */ 16,
    /* .MaxVertexOutputVectors = */ 16,
    /* .MaxFragmentInputVectors = */ 15,
    /* .MinProgramTexelOffset = */ -8,
    /* .MaxProgramTexelOffset = */ 7,
    /* .MaxClipDistances = */ 8,
    /* .MaxComputeWorkGroupCountX = */ 65535,
    /* .MaxComputeWorkGroupCountY = */ 65535,
    /* .MaxComputeWorkGroupCountZ = */ 65535,
    /* .MaxComputeWorkGroupSizeX = */ 1024,
    /* .MaxComputeWorkGroupSizeY = */ 1024,
    /* .MaxComputeWorkGroupSizeZ = */ 64,
    /* .MaxComputeUniformComponents = */ 1024,
    /* .MaxComputeTextureImageUnits = */ 16,
    /* .MaxComputeImageUniforms = */ 8,
    /* .MaxComputeAtomicCounters = */ 8,
    /* .MaxComputeAtomicCounterBuffers = */ 1,
    /* .MaxVaryingComponents = */ 60,
    /* .MaxVertexOutputComponents = */ 64,
    /* .MaxGeometryInputComponents = */ 64,
    /* .MaxGeometryOutputComponents = */ 128,
    /* .MaxFragmentInputComponents = */ 128,
    /* .MaxImageUnits = */ 8,
    /* .MaxCombinedImageUnitsAndFragmentOutputs = */ 8,
    /* .MaxCombinedShaderOutputResources = */ 8,
    /* .MaxImageSamples = */ 0,
    /* .MaxVertexImageUniforms = */ 0,
    /* .MaxTessControlImageUniforms = */ 0,
    /* .MaxTessEvaluationImageUniforms = */ 0,
    /* .MaxGeometryImageUniforms = */ 0,
    /* .MaxFragmentImageUniforms = */ 8,
    /* .MaxCombinedImageUniforms = */ 8,
    /* .MaxGeometryTextureImageUnits = */ 16,
    /* .MaxGeometryOutputVertices = */ 256,
    /* .MaxGeometryTotalOutputComponents = */ 1024,
    /* .MaxGeometryUniformComponents = */ 1024,
    /* .MaxGeometryVaryingComponents = */ 64,
    /* .MaxTessControlInputComponents = */ 128,
    /* .MaxTessControlOutputComponents = */ 128,
    /* .MaxTessControlTextureImageUnits = */ 16,
    /* .MaxTessControlUniformComponents = */ 1024,
    /* .MaxTessControlTotalOutputComponents = */ 4096,
    /* .MaxTessEvaluationInputComponents = */ 128,
    /* .MaxTessEvaluationOutputComponents = */ 128,
    /* .MaxTessEvaluationTextureImageUnits = */ 16,
    /* .MaxTessEvaluationUniformComponents = */ 1024,
    /* .MaxTessPatchComponents = */ 120,
    /* .MaxPatchVertices = */ 32,
    /* .MaxTessGenLevel = */ 64,
    /* .MaxViewports = */ 16,
    /* .MaxVertexAtomicCounters = */ 0,
    /* .MaxTessControlAtomicCounters = */ 0,
    /* .MaxTessEvaluationAtomicCounters = */ 0,
    /* .MaxGeometryAtomicCounters = */ 0,
    /* .MaxFragmentAtomicCounters = */ 8,
    /* .MaxCombinedAtomicCounters = */ 8,
    /* .MaxAtomicCounterBindings = */ 1,
    /* .MaxVertexAtomicCounterBuffers = */ 0,
    /* .MaxTessControlAtomicCounterBuffers = */ 0,
    /* .MaxTessEvaluationAtomicCounterBuffers = */ 0,
    /* .MaxGeometryAtomicCounterBuffers = */ 0,
    /* .MaxFragmentAtomicCounterBuffers = */ 1,
    /* .MaxCombinedAtomicCounterBuffers = */ 1,
    /* .MaxAtomicCounterBufferSize = */ 16384,
    /* .MaxTransformFeedbackBuffers = */ 4,
    /* .MaxTransformFeedbackInterleavedComponents = */ 64,
    /* .MaxCullDistances = */ 8,
    /* .MaxCombinedClipAndCullDistances = */ 8,
    /* .MaxSamples = */ 4,
    /* .maxMeshOutputVerticesNV = */ 256,
    /* .maxMeshOutputPrimitivesNV = */ 512,
    /* .maxMeshWorkGroupSizeX_NV = */ 32,
    /* .maxMeshWorkGroupSizeY_NV = */ 1,
    /* .maxMeshWorkGroupSizeZ_NV = */ 1,
    /* .maxTaskWorkGroupSizeX_NV = */ 32,
    /* .maxTaskWorkGroupSizeY_NV = */ 1,
    /* .maxTaskWorkGroupSizeZ_NV = */ 1,
    /* .maxMeshViewCountNV = */ 4,

    /* .limits = */ {
        /* .nonInductiveForLoops = */ 1,
        /* .whileLoops = */ 1,
        /* .doWhileLoops = */ 1,
        /* .generalUniformIndexing = */ 1,
        /* .generalAttributeMatrixVectorIndexing = */ 1,
        /* .generalVaryingIndexing = */ 1,
        /* .generalSamplerIndexing = */ 1,
        /* .generalVariableIndexing = */ 1,
        /* .generalConstantMatrixVectorIndexing = */ 1,
}};

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

DllExport(int) ShCompileShader(EShLanguage lang, const char* entryName, const char* code, int nDefines, const char* defines[], size_t* outputSize, void** output, int* logLength, char** log)
{
	glslang::TShader shader(lang);
	shader.setStrings(&code, 1);
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
			auto intermediate = program.getIntermediate(lang);
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
				str.append("#SHADER\r\n");
				str.append(shader.getInfoLog());
				str.append("#PROGRAM\r\n");
				str.append(program.getInfoLog());

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
	delete memory;
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