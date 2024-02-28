:: Temporary, remove this batch when shader compilation is integrated into studio
..\Tools\win-x64\glslc -fentry-point=main -fshader-stage=vertex -o "..\Resources\diffuse.vsh.spirv" diffuse.vsh.hlsl
..\Tools\win-x64\glslc -fentry-point=main -fshader-stage=fragment -o "..\Resources\diffuse.psh.spirv" diffuse.psh.hlsl

