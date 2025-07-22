cmake_minimum_required(VERSION 3.10)
project(cast LANGUAGES CXX)

# Use Clang++ as the compiler
set(CMAKE_CXX_COMPILER clang++)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

# C++ standard and warning flags
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)
add_compile_options(-Wall)

# ANTLR Generated CPP Files
file(GLOB_RECURSE GENERATED_SOURCES
    ${CMAKE_SOURCE_DIR}/generated/*.cpp
)

message(STATUS "GENERATED_SOURCES = ${GENERATED_SOURCES}")

# ANTLR4 runtime configuration
set(ANTLR4_RUNTIME_INCLUDE_DIR "/opt/homebrew/opt/antlr4-cpp-runtime/include/antlr4-runtime")
set(ANTLR4_RUNTIME_LIBRARY_DIR "/opt/homebrew/opt/antlr4-cpp-runtime/lib")

include_directories(${ANTLR4_RUNTIME_INCLUDE_DIR})
link_directories(${ANTLR4_RUNTIME_LIBRARY_DIR})

# Main application source
set(SOURCE_FILES
    ${CMAKE_SOURCE_DIR}/src/main.cpp
    ${GENERATED_SOURCES}
)

# Define the executable target
add_executable(cast ${SOURCE_FILES})

# Link against the ANTLR4 runtime library
target_link_libraries(cast PRIVATE antlr4-runtime)

# Include directories
target_include_directories(cast PUBLIC
    ${ANTLR4_RUNTIME_INCLUDE_DIR}
    ${CMAKE_SOURCE_DIR}/generated  # Generated grammar headers
)
