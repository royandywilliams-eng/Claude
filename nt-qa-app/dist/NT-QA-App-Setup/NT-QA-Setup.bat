@echo off
REM NT Q&A Application Setup Launcher
REM This script checks for Python and launches the setup wizard

setlocal enabledelayedexpansion

echo.
echo ========================================
echo NT Q&A Application Setup
echo ========================================
echo.

REM Check if Python is installed
where python >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    where python3 >nul 2>nul
    if !ERRORLEVEL! NEQ 0 (
        echo ERROR: Python is not installed or not in PATH
        echo.
        echo Please install Python 3.7 or later from https://www.python.org
        echo Make sure to check "Add Python to PATH" during installation
        echo.
        pause
        exit /b 1
    )
    set PYTHON=python3
) else (
    set PYTHON=python
)

REM Get the directory where this batch file is located
set SCRIPT_DIR=%~dp0
set APP_DIR=%SCRIPT_DIR%..

echo Launching NT Q&A Setup Wizard...
echo.

REM Run the Python setup wizard
%PYTHON% "%SCRIPT_DIR%NT-QA-Setup-Wizard.py" "%APP_DIR%"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Setup encountered an error. Please try again.
    pause
    exit /b 1
)

echo.
echo Setup complete!
pause
