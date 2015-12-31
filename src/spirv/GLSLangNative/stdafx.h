// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#if _WIN32
#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>
#endif

#include <ShaderLang.h>
#include <GlslangToSpv.h>
#include <GLSL.std.450.h>
#include <doc.h>
#include <disassemble.h>
// TODO: reference additional headers your program requires here