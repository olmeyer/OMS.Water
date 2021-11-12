@call "%VS140COMNTOOLS%vsvars32.bat"

@msbuild "BuildAll.proj" /t:BuildAll /p:Configuration=Release;TargetVersion=3.5

pause