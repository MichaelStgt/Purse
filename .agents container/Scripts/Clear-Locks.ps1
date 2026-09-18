# Clear-Locks.ps1
Write-Host "Forcefully terminating orphaned build processes..." -ForegroundColor Yellow

Stop-Process -Name "MSBuild" -Force -ErrorAction SilentlyContinue
Stop-Process -Name "dotnet" -Force -ErrorAction SilentlyContinue
Stop-Process -Name "VBCSCompiler" -Force -ErrorAction SilentlyContinue
Stop-Process -Name "aapt2" -Force -ErrorAction SilentlyContinue

Write-Host "Clearing bin and obj folders..." -ForegroundColor Yellow
$projectPath = "C:\Repos\040 MauiNet10\Purse\040 Projects\Purse"

If (Test-Path "$projectPath\bin") { Remove-Item -Path "$projectPath\bin" -Recurse -Force }
If (Test-Path "$projectPath\obj") { Remove-Item -Path "$projectPath\obj" -Recurse -Force }

Write-Host "Locks cleared. Ready for a clean rebuild." -ForegroundColor Green