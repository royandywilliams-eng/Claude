@echo off
REM ========================================
REM ProjectSpec GUI - Uninstall Script
REM ========================================
REM This script removes ProjectSpec GUI completely
REM from your PC, including application folder
REM and user settings/history data.

setlocal enabledelayedexpansion

echo.
echo ========================================
echo ProjectSpec GUI - Uninstall
echo ========================================
echo.

REM Check if running from Deploy folder
if not exist "ProjectSpecGUI.exe" (
    echo Error: Please run this script from the Deploy folder
    echo where ProjectSpecGUI.exe is located.
    pause
    exit /b 1
)

REM Get the Deploy folder path
set "DEPLOY_PATH=%~dp0"
set "PARENT_PATH=%DEPLOY_PATH:~0,-1%"
for %%A in ("!PARENT_PATH!") do set "PARENT_PATH=%%~dpA"

echo Uninstall Location: !PARENT_PATH!
echo.

REM Ask for confirmation
echo This will remove:
echo   1. Application folder: !PARENT_PATH!
echo   2. Settings and history: %%APPDATA%%\ProjectSpecGUI\
echo.
set /p CONFIRM="Are you sure you want to uninstall? (yes/no): "

if /i NOT "%CONFIRM%"=="yes" (
    echo Uninstall cancelled.
    pause
    exit /b 0
)

echo.
echo Uninstalling...
echo.

REM Remove settings and history from APPDATA
echo [1/2] Removing settings and history from %%APPDATA%%...
set "APPDATA_PATH=%APPDATA%\ProjectSpecGUI"
if exist "!APPDATA_PATH!" (
    rmdir /s /q "!APPDATA_PATH!" >nul 2>&1
    if !errorlevel! equ 0 (
        echo         ✓ Settings and history removed
    ) else (
        echo         ✗ Failed to remove settings (may be in use)
        echo         You can manually delete: !APPDATA_PATH!
    )
) else (
    echo         ✓ No settings found (already clean)
)

echo.
echo [2/2] Removing application folder...
REM Remove the entire Deploy parent folder
cd /d "!PARENT_PATH!"
cd ..
if exist "!PARENT_PATH!" (
    rmdir /s /q "!PARENT_PATH!" >nul 2>&1
    if !errorlevel! equ 0 (
        echo         ✓ Application folder removed
    ) else (
        echo         ✗ Failed to remove application folder
        echo         You can manually delete: !PARENT_PATH!
    )
) else (
    echo         ✓ Application folder already removed
)

echo.
echo ========================================
echo Uninstall Complete
echo ========================================
echo.
echo ProjectSpec GUI has been completely removed from your PC.
echo No registry entries, services, or system modifications remain.
echo.
echo Thank you for using ProjectSpec GUI!
echo.
pause
