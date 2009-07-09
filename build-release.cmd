@echo off
for /F "usebackq tokens=1 " %%i in ("C:\Ember Media Manager\svn_rev.txt") do  @set EMM_REV=%%i
echo Building Release package
@call "c:\Program Files\Microsoft Visual Studio 9.0\VC\vcvarsall.bat" > nul

del /f /s /q packages\x64  > nul 2> nul
del /f /s /q packages\x86  > nul 2> nul
del /f /s /q EMM_r*.zip  > nul 2> nul

mkdir packages\x64 > nul 2> nul
mkdir packages\x86 > nul 2> nul
 

msbuild "Ember Media Manager.sln" /p:Configuration=Release;platform=x64;OutDir=..\packages\x64\ /noconlog /nologo
msbuild "Ember Media Manager.sln" /p:Configuration=Release;platform=x86;OutDir=..\packages\x86\ /noconlog /nologo

IF EXIST "packages\x64\Ember Media Manager.exe" (
copy "Release Files\*" packages\x64\ > nul 2> nul
copy /Y "Release Files\x64\System.Data.SQLite.dll" packages\x64\ > nul 2> nul

pushd packages\x64\
..\..\7za a  ..\EMM_r%EMM_REV%_exeonly_x64.zip "Ember Media Manager.exe" > nul 2> nul
..\..\7za a  ..\EMM_r%EMM_REV%_exeonly_x64.zip "License.txt" > nul 2> nul
..\..\7za a  ..\EMM_r%EMM_REV%_exeonly_x64.zip "README.rtf" > nul 2> nul
..\..\7za a  ..\EMM_r%EMM_REV%_exeonly_x64.zip "System.Data.SQLite.dll" > nul 2> nul
popd
echo Pack x64
) ELSE (
echo Error Pack x64
)
IF EXIST "packages\x86\Ember Media Manager.exe" (
copy "Release Files\*" packages\x86\ > nul 2> nul
copy /Y "Release Files\x86\System.Data.SQLite.dll" packages\x86\ > nul 2> nul

pushd packages\x86\
..\..\7za a  ..\EMM_r%EMM_REV%_exeonly_x86.zip "Ember Media Manager.exe" > nul 2> nul
..\..\7za a  ..\EMM_r%EMM_REV%_exeonly_x86.zip "License.txt" > nul 2> nul
..\..\7za a  ..\EMM_r%EMM_REV%_exeonly_x86.zip "README.rtf" > nul 2> nul
..\..\7za a  ..\EMM_r%EMM_REV%_exeonly_x86.zip "System.Data.SQLite.dll" > nul 2> nul
popd
echo Pack x86
) ELSE (
echo Error Pack x86
)

del /f /s /q packages\x64  > nul 2> nul
del /f /s /q packages\x86  > nul 2> nul
rmdir packages\x64 
rmdir packages\x86 
pause