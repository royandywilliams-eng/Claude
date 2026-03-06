@echo off
REM ProjectSpec GUI Launcher
REM This batch file launches the ProjectSpec GUI application

echo Starting ProjectSpec GUI...
echo.

REM Check if .NET 6 runtime is installed
dotnet --info >nul 2>&1
if errorlevel 1 (
    echo.
    echo ERROR: .NET 6 Runtime not found!
    echo Please install it from: https://dotnet.microsoft.com/download/dotnet/6.0
    echo.
    pause
    exit /b 1
)

REM Launch the application
start ProjectSpecGUI.exe

REM Exit silently
exit /b 0
