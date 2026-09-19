@echo off
echo Killing locked background compilers...
taskkill /F /IM MSBuild.exe /T 2>nul
taskkill /F /IM VBCSCompiler.exe /T 2>nul
taskkill /F /IM dotnet.exe /T 2>nul

echo Wiping bin and obj folders...
FOR /D /R %%G in (bin,obj) DO @IF EXIST "%%G" rd /s /q "%%G"

echo Wiping corrupted IntelliSense cache...
rd /s /q .vs 2>nul

echo Clean complete. You can now build in Visual Studio.
pause