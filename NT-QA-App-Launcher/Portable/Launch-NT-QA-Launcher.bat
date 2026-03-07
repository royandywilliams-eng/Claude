@echo off
REM NT Q&A Application Launcher Batch Script
REM This script checks for .NET 9.0 runtime and launches the launcher

echo.
echo ========================================
echo NT Q&A Application Launcher
echo ========================================
echo.

REM Check if .NET is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET runtime is not installed!
    echo.
    echo Please download and install .NET 9.0 from:
    echo https://dotnet.microsoft.com/download/dotnet/9.0
    echo.
    pause
    exit /b 1
)

REM Get .NET version
for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo .NET version detected: %DOTNET_VERSION%
echo.

REM Launch the application
echo Starting NT Q&A Application Launcher...
echo.
start "" "%~dp0NTQAAppLauncher.exe"

exit /b 0
