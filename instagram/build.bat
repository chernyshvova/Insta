"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" instarm.sln property:Configuration=Release -verbosity:quiet
call post_build_creator.bat
pause