rem call "$(SolutionDir)ON-Build-Release" $(OutDir) "$(ProjectDir)" "$(SolutionDir)" $(PlatformName)
@echo off
for /F "usebackq tokens=1 " %%i in ("C:\Ember Media Manager\svn_rev.txt") do  @set EMM_REV=%%i
cd %3%
del /f /q EMM_r*%4%.zip  > nul 2> nul
cd %2%
cd %1%
echo %CD%
IF EXIST "Ember Media Manager.exe" (
copy "Ember Media Manager.exe" %3%  > nul 2> nul
copy "Ember Media Manager.exe.manifest" %3%  > nul 2> nul
cd %3%
copy "Release Files\License.txt" %3%  > nul 2> nul
copy "Release Files\README.rtf" %3%  > nul 2> nul
7za a "EMM_r%EMM_REV%_exeonly_%4%.zip" "Ember Media Manager.exe" > nul 2> nul
7za a "EMM_r%EMM_REV%_exeonly_%4%.zip" "License.txt" > nul 2> nul
7za a "EMM_r%EMM_REV%_exeonly_%4%.zip" "README.rtf" > nul 2> nul
7za a "EMM_r%EMM_REV%_exeonly_%4%.zip" "Ember Media Manager.exe.manifest" > nul 2> nul
del /Q "License.txt"
del /Q "README.rtf"
del /Q "Ember Media Manager.exe.manifest" 
del /Q "Ember Media Manager.exe"
echo Package %4% OK
) ELSE (
echo Error Package %4%
)

