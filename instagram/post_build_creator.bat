
DEL "%~dp0\build" /F /Q /S
mkdir "%~dp0\build"
mkdir "%~dp0\build\bin"
mkdir "%~dp0\build\avatars"
mkdir "%~dp0\build\images"
copy "%~dp0%instarm\bin\Release\instagram.db" build\instagram.db

xcopy "%~dp0\instarm\bin\Release" "%~dp0\build\bin"

