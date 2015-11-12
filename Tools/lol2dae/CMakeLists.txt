cmake_minimum_required (VERSION 2.8)
project (lol2dae)
	
add_subdirectory(IlmBase)


#external includes
include_directories("IlmBase")
include_directories("IlmBase/Half")
include_directories("IlmBase/Iex")
include_directories("IlmBase/IexMath")
include_directories("IlmBase/Imath")
include_directories("IlmBase/IlmThread")
include_directories(${CMAKE_CURRENT_BINARY_DIR}/IlmBase/config)

	
#variable setting
FILE(GLOB MainFiles src/*.h src/*.cpp)

							
					
#add_library(lol2dae STATIC
add_executable(lol2dae ${MainFiles})
				
target_link_libraries(lol2dae Half Iex IexMath Imath IlmThread)
