cmake_minimum_required(VERSION 3.5)
include("cmake/CPM.cmake")
include("cmake/Aardvark.cmake")

project(GLSLangNative)

CPMAddPackage(
    NAME glslang
    GIT_TAG 11.7.1
    GITHUB_REPOSITORY "KhronosGroup/glslang" 
    OPTIONS "SKIP_GLSLANG_INSTALL YES" 
)

CPMAddPackage(
    NAME spirv-headers
    GIT_TAG sdk-1.2.198.0
    GITHUB_REPOSITORY "KhronosGroup/SPIRV-Headers" 
    OPTIONS "SPIRV_HEADERS_SKIP_INSTALL YES"
)

CPMAddPackage(
    NAME spirv-tools
    GIT_TAG v2021.4
    GITHUB_REPOSITORY "KhronosGroup/SPIRV-Tools" 
    OPTIONS "SKIP_SPIRV_TOOLS_INSTALL YES" "SPIRV_SKIP_EXECUTABLES YES"
)

set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

SET(CMAKE_BUILD_TYPE "MinSizeRel" CACHE INTERNAL "release mode")

add_library(GLSLangNative SHARED glslang.h glslang.cpp)

set_target_properties(GLSLangNative
    PROPERTIES
        C_VISIBILITY_PRESET hidden
        CXX_VISIBILITY_PRESET hidden
        CMAKE_C_VISIBILITY_PRESET hidden
        CMAKE_CXX_VISIBILITY_PRESET hidden
)

include_directories(${glslang_SOURCE_DIR})
include_directories(${spirv-headers_SOURCE_DIR})
include_directories(${spirv-tools_SOURCE_DIR})
target_link_libraries(GLSLangNative PRIVATE GenericCodeGen HLSL OGLCompiler OSDependent SPIRV MachineIndependent SPIRV-Tools-opt SPIRV-Tools )


SET(CMAKE_INSTALL_PREFIX "${CMAKE_CURRENT_SOURCE_DIR}/../../libs/Native/GLSLangSharp/") 
install(TARGETS GLSLangNative DESTINATION ${OS}/${ARCH})
