
echo off
set version_19=2019
set version_17=2017
set basePath_19=C:\Program Files (x86)\Microsoft Visual Studio\%version_19%\Community\MSBuild\
set basePath_17=C:\Program Files (x86)\Microsoft Visual Studio\%version_17%\Community\MSBuild\


IF exist "%basePath_19%Current\Bin\MSBuild.exe" (
"%basePath_19%Current\Bin\MSBuild.exe" "%~dp0\instarm.sln" /property:Configuration=Release -verbosity:diagnostic
)

IF exist "%basePath_17%15.0\Bin\MSBuild.exe" (
"%basePath_17%15.0\Bin\MSBuild.exe" "%~dp0\instarm.sln" property:Configuration=Release -verbosity:minimal
)

call "%~dp0\post_build_creator.bat" >nul

@echo off


                       
echo   ^(                                 ^_
echo   ^)                               ^/^=^>
echo  ^(  ^+____________________/\/\___ / /^|
echo   .''._____________'._____      / /^|/\
echo  : () :              :\ ----\^|    \ )
echo   '..'______________.'0^|----^|      \
echo                    0_0/____/        \
echo                        ^|----    /----\
echo                       ^|^| -\\ --^|      \
echo                       ^|^|   ^|^| ^|^|\      \
echo                        \\____// '^|      \
echo Bang! Bang!                    .'/       ^|
echo                               .:/        ^|
echo                               :/_________^|

ping 127.0.0.1 -n 2 > nul
color 4
ping 127.0.0.1 -n 2 > nul
color 7
ping 127.0.0.1 -n 2 > nul
color 4
ping 127.0.0.1 -n 2 > nul
color 7
ping 127.0.0.1 -n 2 > nul
color 4
ping 127.0.0.1 -n 2 > nul
color 7
echo done!
pause >nul