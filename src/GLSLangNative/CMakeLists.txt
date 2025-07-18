cmake_minimum_required(VERSION 3.10)
include("cmake/CPM.cmake")
include("cmake/Aardvark.cmake")

project(GLSLangNative)

set(CMAKE_BUILD_TYPE, "Release")
if(UNIX)
    set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -O3 -ffunction-sections -fdata-sections -fvisibility=hidden")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -O3 -ffunction-sections -fdata-sections -fvisibility=hidden")
endif()

CPMAddPackage(
    NAME glslang
    GIT_TAG vulkan-sdk-1.4.321.0
    GITHUB_REPOSITORY "KhronosGroup/glslang" 
    OPTIONS 
        "SKIP_GLSLANG_INSTALL YES" 
        "BUILD_SHARED_LIBS NO" 
        "ENABLE_GLSLANG_BINARIES NO" 
        "ENABLE_OPT NO"
        "ENABLE_HLSL NO"
)

CPMAddPackage(
    NAME spirv-headers
    GIT_TAG vulkan-sdk-1.4.321.0
    GITHUB_REPOSITORY "KhronosGroup/SPIRV-Headers" 
    OPTIONS 
        "SPIRV_HEADERS_SKIP_INSTALL YES" 
)

CPMAddPackage(
    NAME spirv-tools
    GIT_TAG vulkan-sdk-1.4.321.0
    GITHUB_REPOSITORY "KhronosGroup/SPIRV-Tools" 
    OPTIONS 
        "SKIP_SPIRV_TOOLS_INSTALL YES" 
        "SPIRV_SKIP_EXECUTABLES ON"
        "SPIRV_WERROR OFF"
)

set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
if(APPLE)
    set(CMAKE_SHARED_LINKER_FLAGS "-O3 -flto=thin -dead_strip")
elseif(UNIX)
    set(CMAKE_SHARED_LINKER_FLAGS "-O3 -Wl,--gc-sections -Wl,--strip-all")
endif()


add_library(GLSLangNative SHARED glslang.h glslang.cpp)

include_directories(${glslang_SOURCE_DIR})
include_directories(${spirv-headers_SOURCE_DIR})
include_directories(${spirv-tools_SOURCE_DIR})
target_link_libraries(GLSLangNative PRIVATE GenericCodeGen OSDependent SPIRV MachineIndependent SPIRV-Tools-opt SPIRV-Tools )

SET(CMAKE_INSTALL_PREFIX "${CMAKE_CURRENT_SOURCE_DIR}/../../libs/Native/GLSLangSharp/") 
install(TARGETS GLSLangNative DESTINATION ${OS}/${ARCH})
