FROM ubuntu:16.04

RUN apt-get update
RUN apt-get install -y apt-transport-https ca-certificates gnupg \
                         software-properties-common wget

RUN apt-add-repository 'deb https://apt.kitware.com/ubuntu/ xenial main'
RUN apt-get update
RUN apt-get install -y --allow-unauthenticated gcc git python3 cmake g++ python


RUN git clone https://github.com/KhronosGroup/glslang.git
RUN pwd
RUN cd glslang \
    && python ./update_glslang_sources.py \
    && mkdir build && cd build \
    && cmake -DCMAKE_BUILD_TYPE=Release -DCMAKE_INSTALL_PREFIX="$(pwd)/install" .. \
    && make && make install


RUN git clone https://github.com/aardvark-community/GLSLangSharp.git
RUN cp -r glslang/build/install/* GLSLangSharp/src/GLSLangNative
RUN cd GLSLangSharp/src/GLSLangNative && make

RUN cd glslang && echo git rev-parse HEAD > /GLSLangSharp/libs/Native/GLSLangSharp/linux/AMD64/version


# docker build  -t  builder .
# docker run -d builder
# docker cp b5d496da9d77796460bb7b09d5b49f291f52c19b228bdefd7ab75d7aeb945039:/GLSLangSharp/libs/Native/GLSLangSharp/linux/AMD64/ ./build

# docker buildx build --platform linux/amd64,linux/arm64 .