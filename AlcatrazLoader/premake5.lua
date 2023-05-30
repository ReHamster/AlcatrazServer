-- premake5.lua

require "premake_modules/usage"

workspace "AlcatrazUPlay"
	location "project_%{_ACTION}_%{os.target()}"
	targetdir "bin/%{cfg.buildcfg}"
	
	language "C++"
	cppdialect "C++20"
	
	configurations { "Debug", "Release" }
	linkgroups 'On'
	characterset "ASCII"
	
	startproject "ubiorbitapi_r2_loader"
	
	includedirs {
		"./"
	}	

	disablewarnings { "4251", "4996", "4554", "4244", "4101", "4838", "4309" }
	
	defines { "_CRT_SECURE_NO_WARNINGS" }

	filter "configurations:Debug"
		defines { 
			"DEBUG", 
		}
		symbols "On"

	filter "configurations:Release"
		defines {
			"NDEBUG",
		}
		optimize "Full"

group "Dependencies"

include "dependencies/HF/premake5.lua"

-- NoSTD
project "libnstd"
	kind "StaticLib"
		
	includedirs {
		"dependencies/libnstd/include"
	}
	
	files {
		"dependencies/libnstd/src/**.cpp",
		"dependencies/libnstd/src/**.h",
	}
	
usage "libnstd"
	includedirs {
		"dependencies/libnstd/include"
	}
	links "libnstd"

-- Discord
usage "discord"
	includedirs {
		"dependencies/discord"
	}
	links {
		"dependencies/discord/lib/discord-rpc.lib"
	}
	files {
		"dependencies/discord/**.cpp",
		"dependencies/discord/**.h",
	}
				

group "Main"

project "ubiorbitapi_r2_loader"
	kind "SharedLib"

	uses { 
		"libnstd", 
		"HackingFramework",
		"discord"
	}

	includedirs {
		"./",
	}

	files {
		"orbit_src/**.cpp",
		"orbit_src/**.h",
		"orbit_src/**.hpp",
	}
	
project "DFEngine"
	kind "SharedLib"

	includedirs {
		"./",
	}

	files {
		"noads_src/**.cpp",
		"noads_src/**.def",
	}
	