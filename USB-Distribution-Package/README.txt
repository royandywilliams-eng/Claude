ProjectSpec GUI v1.0.0 - Flash Drive/USB Distribution Package
===============================================================

QUICK START
-----------
1. Insert this USB drive into any Windows PC
2. Double-click SETUP.bat
3. Follow on-screen instructions
4. ProjectSpec GUI launches when ready!

WHAT'S ON THIS DRIVE
---------------------
• ProjectSpecGUI/              - Application files
• SETUP.bat                   - Automatic installer
• dotnet-runtime-6.0.x-win-x64.exe  - .NET 6 Runtime installer
• INSTALL-DOTNET6-FIRST.txt   - Detailed installation instructions
• README.txt                  - This file

REQUIREMENTS
-----------
• Windows 10 or Windows 11
• 500 MB RAM (2 GB recommended)
• 100 MB free disk space
• Internet connection (for Claude API features)

STEP-BY-STEP SETUP
------------------

First Time Setup:
1. Insert USB drive into Windows PC
2. Double-click SETUP.bat
3. Script will check if .NET 6 is installed
4. If not installed: installer will run automatically
5. If installed: ProjectSpec GUI launches immediately

Manual Setup (if SETUP.bat doesn't work):
1. Double-click dotnet-runtime-6.0.x-win-x64.exe
2. Click "Install" button
3. Wait for installation to complete
4. Click "Close"
5. Go to ProjectSpecGUI folder
6. Double-click Launch.bat

GETTING STARTED WITH THE APPLICATION
-------------------------------------
1. Application launches with main configuration window
2. Go to Claude → Settings
3. Enter your Claude API key (get from https://console.anthropic.com)
4. Select preferred model (Opus 4.6, Sonnet 3.5, or Haiku 3)
5. Click "Test Connection" to verify
6. Click OK
7. Start configuring your projects!

FEATURES
--------
✅ 6-step wizard for project configuration
✅ 9 advanced configuration tabs
✅ 200+ customizable options
✅ Real-time specification preview (Markdown/JSON)
✅ Claude API integration
✅ API call history tracking
✅ 100% portable (no system installation)
✅ One-click uninstall

TROUBLESHOOTING
---------------

Problem: "Application won't start"
Solution: Run SETUP.bat first to install .NET 6

Problem: ".NET 6 installation fails"
Solution:
- Try manual install of dotnet-runtime-6.0.x-win-x64.exe
- See INSTALL-DOTNET6-FIRST.txt for details

Problem: "API key error"
Solution:
- Verify key at https://console.anthropic.com
- Ensure no extra spaces in API key field
- Click "Test Connection" in Settings

Problem: "Request timed out"
Solution:
- Increase timeout in Claude → Settings
- Simplify configuration (fewer frameworks)
- Check internet connection

Problem: "Want to remove ProjectSpec GUI"
Solution:
- Go to ProjectSpecGUI folder
- Double-click UNINSTALL.bat
- Confirm uninstall
- Complete removal with one click

TECHNICAL DETAILS
-----------------
• Platform: Windows Forms (.NET 6.0)
• Language: C#
• Architecture: Multi-tier (Presentation, Configuration, Integration)
• Database: Local JSON files in %APPDATA%\ProjectSpecGUI\
• API: Anthropic Claude API v1
• Models Supported: Opus 4.6, Sonnet 3.5, Haiku 3

FILE LOCATIONS
--------------
Application: Runs from USB/local folder (completely portable)

Settings & History: Stored in %APPDATA%\ProjectSpecGUI\
- settings.json     (API key, model selection, preferences)
- history.json      (API call history - max 100 entries)

Can be backed up, migrated, or deleted without affecting installation.

FOR MORE INFORMATION
--------------------
Full Documentation: See ProjectSpecGUI/README.md
Troubleshooting Guide: See ProjectSpecGUI/DEPLOYMENT.txt (if included)
GitHub Repository: https://github.com/royandywilliams-eng/Claude
Claude API Docs: https://docs.anthropic.com

DISTRIBUTION
-----------
This package can be:
✓ Copied to USB flash drives
✓ Burned to DVD+R discs
✓ Emailed as .zip archive
✓ Shared via file transfer
✓ Deployed across networked computers

No license restrictions. Free to distribute.

VERSION HISTORY
---------------
v1.0.0 - March 2026 (Current)
- Phase 1: UI Framework (1,147 lines)
- Phase 2: Wizard Screens (1,152 lines)
- Phase 3: Configuration Tabs (1,927 lines)
- Phase 4: Configuration Engine (400 lines)
- Phase 5: Claude API Integration (800 lines)
- Total: 7,300+ lines of production code

SUPPORT
-------
Issues or questions?
1. Check the troubleshooting section above
2. Read ProjectSpecGUI/README.md
3. Visit GitHub: https://github.com/royandywilliams-eng/Claude
4. Review INSTALL-DOTNET6-FIRST.txt for setup help

Thank you for using ProjectSpec GUI!

===============================================================
ProjectSpec GUI v1.0.0 | Portable | Non-Intrusive | Zero-Installation
===============================================================
