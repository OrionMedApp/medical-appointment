cmake_minimum_required(VERSION 4.1)
project(Project)

set(CMAKE_CXX_STANDARD 20)

add_executable(Project
        App.cpp
        main.cpp
        Doctor.cpp
        Doctor.hpp
        HospitalManager.cpp
        HospitalManager.hpp
        Validator.cpp
        Validator.hpp
        Patient.cpp
        Patient.hpp
)
target_link_libraries(Project ole32 uuid)