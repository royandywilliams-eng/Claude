@echo off
REM ProjectSpec GUI - Automatic Setup
REM Checks for .NET 6 and installs if needed

cls
echo ========================================
echo ProjectSpec GUI - First Time Setup
echo ========================================
echo.

REM Check if .NET 6 is installed
dotnet --version >nul 2>&1

if %errorlevel% neq 0 (
    echo .NET 6 Runtime not found. Installing...
    echo.

    if exist "dotnet-runtime-6.0.x-win-x64.exe" (
        echo Running installer. Please wait...
        dotnet-runtime-6.0.x-win-x64.exe /install /passive /norestart

        echo.
        echo ========================================
        echo Installation complete!
        echo ========================================
        echo.
        echo You can now use ProjectSpec GUI.
        echo.
        pause

        REM Launch the app
        cd ProjectSpecGUI
        call Launch.bat
    ) else (
        echo ERROR: .NET 6 installer not found!
        echo Please download from:
        echo https://dotnet.microsoft.com/download/dotnet/6.0
        echo.
        pause
    )
) else (
    echo .NET 6 is already installed!
    echo.
    echo Launching ProjectSpec GUI...
    cd ProjectSpecGUI
    call Launch.bat
)
