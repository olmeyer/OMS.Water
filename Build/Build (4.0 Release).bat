setlocal EnableDelayedExpansion
echo Locating msbuild.exe
set VSWHERE="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
for /f "tokens=1 delims=;" %%i in ('"!VSWHERE!" -nologo -latest -property installationPath') do SET "MSBUILDROOT=%%i"
REM for /f "tokens=1 delims=;" %%i in ('"!VSWHERE!" -nologo -version [16.0,17.0) -property installationPath') do SET "MSBUILDROOT=%%i"
for /f "tokens=1" %%i in ('"!VSWHERE!" -property installationVersion') do SET "MSBUILDVER=%%i"
set MSBUILDPATH="!MSBUILDROOT!\MSBuild\!MSBUILDVER:~0,2!.0\Bin\MSBuild.exe"
rem if not exist "!MSBUILDPATH!" set MSBUILDPATH="!MSBUILDROOT!\MSBuild\Current\Bin\amd64\MSBuild.exe"
if not exist "!MSBUILDPATH!" set MSBUILDPATH="!MSBUILDROOT!\MSBuild\Current\Bin\MSBuild.exe"
echo Found MSBuild at !MSBUILDPATH!

set Platform=

%MSBUILDPATH% "BuildAll.proj" /t:BuildAll /p:Configuration=Release

pause

