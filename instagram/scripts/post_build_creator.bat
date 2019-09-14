

set rootFolder=%~dp0..\

DEL "%rootFolder%build" /F /Q /S

IF Not exist "%rootFolder%build"  (
mkdir "%rootFolder%build"
)

IF Not exist "%rootFolder%build\bin" (
mkdir "%rootFolder%build\bin"
)

copy "%rootFolder%instarm\bin\Release\instagram.db" "%rootFolder%build\instagram.db"

xcopy "%rootFolder%instarm\bin\Release" "%rootFolder%build\bin" /E 

%~dp0/CommonTools_postBuilder.bat
