# GLSLangSharp

[![Publish](https://github.com/aardvark-community/GLSLangSharp/actions/workflows/publish.yml/badge.svg)](https://github.com/aardvark-community/GLSLangSharp/actions/workflows/publish.yml)
[![Nuget](https://badgen.net/nuget/v/GLSLangSharp/pre)](https://www.nuget.org/packages/GLSLangSharp/)
[![Downloads](https://badgen.net/nuget/dt/GLSLangSharp)](https://www.nuget.org/packages/GLSLangSharp/)

A simple wrapper around GLSLang/SpirV-Tools to compile GLSL to SpirV using [Aardvark](https://github.com/aardvark-platform)-style native depdencies.

## Building
### Prerequisites
* `cmake` and standard C/C++ build tools
* `dotnet` >= 8.0

### Compiling
* `buildnative.(sh|cmd)` (this might take a while)
* `dotnet tool restore`
* `dotnet build src/GLSLang.sln`

### Publish

Simply edit `RELEASE_NOTES.md` and push to master. The github actions will take care of building/publishing on NuGet.
