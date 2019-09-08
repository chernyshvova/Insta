
echo off
set version_19=2019
set version_17=2017
set basePath_19=C:\Program Files (x86)\Microsoft Visual Studio\%version_19%\Community\MSBuild\
set basePath_17=C:\Program Files (x86)\Microsoft Visual Studio\%version_17%\Community\MSBuild\


IF exist "%basePath_19%Current\Bin\MSBuild.exe" (
"%basePath_19%Current\Bin\MSBuild.exe" "%~dp0\instarm.sln" /property:Configuration=Release -verbosity:diagnostic
)

IF exist "%basePath_17%15.0\Bin\MSBuild.exe" (
"%basePath_17%15.0\Bin\MSBuild.exe" "%~dp0\instarm.sln" /property:Configuration=Release -verbosity:minimal
)

call "%~dp0\post_build_creator.bat" >nul
echo done!
pause >nul