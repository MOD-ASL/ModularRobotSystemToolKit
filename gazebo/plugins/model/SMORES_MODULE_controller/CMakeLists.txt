cmake_minimum_required(VERSION 2.8 FATAL_ERROR)
find_package( Boost COMPONENTS system log REQUIRED )
set(CMAKE_CXX_FLAGS "-g -Wall --std=c++11")

include(FindPkgConfig)
if(PKG_CONFIG_FOUND)
  pkg_check_modules(GAZEBO gazebo)
endif()

include_directories(${GAZEBO_INCLUDE_DIRS})
link_directories(${GAZEBO_LIBRARY_DIRS})
add_definitions(-DBOOST_LOG_DYN_LINK)

add_library(SMORES_MODULE_controller SHARED SMORES_MODULE_controller.cc)
target_link_libraries(SMORES_MODULE_controller ${GAZEBO_LIBRARIES}
    ${Boost_LIBRARIES})

