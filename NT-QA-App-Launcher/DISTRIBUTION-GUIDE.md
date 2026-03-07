# NT Q&A App Launcher - Distribution Guide

## Overview

The NT Q&A Application Launcher is ready for distribution in multiple formats.

## Distribution Formats

### 1. Portable Package (Recommended)
**Location**: `D:\NT-QA-App-Launcher\Portable\`

**Files Included**:
- `NTQAAppLauncher.exe` (153 KB) - Main launcher executable
- `Launch-NT-QA-Launcher.bat` - Launch script with .NET verification
- `QUICK-START.txt` - User-friendly quick start guide

**Advantages**:
- ✅ No installation required
- ✅ Can be run from USB drive
- ✅ No registry modifications
- ✅ Easy to distribute via email or USB
- ✅ Can be placed on Desktop or anywhere

**Requirements for Users**:
- Windows 10 or Windows 11
- .NET 9.0 Runtime (or Visual Studio 2022)
- Node.js installed

**Distribution Method 1: USB Flash Drive**
```
USB Drive (e.g., E:\)
├── NT-QA-Launcher/
│   ├── NTQAAppLauncher.exe
│   ├── Launch-NT-QA-Launcher.bat
│   └── QUICK-START.txt
├── DOTNET-INSTALLER.txt (instructions)
└── START-HERE.txt
```

**Distribution Method 2: Email/Download**
```
1. Zip the Portable folder
2. Send: NT-QA-Launcher.zip (~160 KB)
3. User extracts and double-clicks Launch-NT-QA-Launcher.bat
```

**Distribution Method 3: Desktop Shortcut**
```
1. Copy NTQAAppLauncher.exe to Desktop
2. Create shortcut
3. Users can launch directly from Desktop
```

### 2. Release Build
**Location**: `D:\NT-QA-App-Launcher\bin\Release\net9.0-windows\`

**Files Included**:
- `NTQAAppLauncher.exe` (153 KB) - Executable
- `NTQAAppLauncher.dll` (23 KB) - Assembly
- `NTQAAppLauncher.deps.json` - Dependencies manifest
- `NTQAAppLauncher.pdb` (17 KB) - Debug symbols (optional for distribution)
- `NTQAAppLauncher.runtimeconfig.json` - Runtime configuration

**Total Size**: ~190 KB (with PDB) or ~155 KB (without PDB)

### 3. Source Code
**Location**: `D:\NT-QA-App-Launcher\`

**For Developers**:
```bash
# Build from source
cd D:\NT-QA-App-Launcher
dotnet build -c Release

# Or use Visual Studio
open NT-QA-App-Launcher.csproj in Visual Studio
Press F5 to run
```

## Installation Instructions for Users

### Quick Start (30 seconds)

**Option A: Batch Script (Recommended)**
```
1. Download/Extract NT-QA-Launcher.zip
2. Double-click: Launch-NT-QA-Launcher.bat
3. Click "Start Server"
4. Click "Open Browser"
```

**Option B: Direct Executable**
```
1. Download/Extract NTQAAppLauncher.exe
2. Double-click the .exe file
3. Click "Start Server"
4. Click "Open Browser"
```

### System Requirements Check

Before distribution, provide users with a checklist:

```
Required:
☐ Windows 10 or Windows 11
☐ .NET 9.0 Runtime installed
☐ Node.js installed
☐ ~100 MB free disk space

Optional:
☐ Default web browser (any modern browser)
☐ Launcher on Desktop (for convenience)
☐ Administrator access (usually not needed)
```

### .NET 9.0 Installation

If users don't have .NET 9.0 installed:

1. Visit: https://dotnet.microsoft.com/download/dotnet/9.0
2. Click "Run" next to ".NET 9.0 Runtime"
3. Download and execute installer
4. Follow installation wizard
5. Restart computer (if prompted)

### Node.js Installation

If users don't have Node.js installed:

1. Visit: https://nodejs.org/
2. Download "LTS" version (Long Term Support)
3. Execute installer
4. Follow installation wizard (default settings OK)
5. Restart Command Prompt (if already open)

### Verify Installation

Users can verify by opening Command Prompt:
```bash
dotnet --version    # Should show 9.0.x or higher
npm --version       # Should show 10.x or higher
node --version      # Should show 20.x or higher
```

## Customization for Distribution

### Option 1: Include with NT-QA-App

Bundle the launcher with the NT-QA App:
```
NT-QA-App-Complete.zip
├── NT-QA-App-Launcher/
│   ├── NTQAAppLauncher.exe
│   ├── Launch-NT-QA-Launcher.bat
│   └── QUICK-START.txt
├── NT-QA-App-Project/
│   └── dist/NT-QA-App-Setup/app/
│       ├── package.json
│       ├── server.js
│       └── ... (other files)
└── README.txt (master instructions)
```

### Option 2: Branded Distribution

Create branded version:
1. Change window title in LauncherConfig.cs
2. Add your company icon
3. Rebuild release version
4. Distribute with company branding

### Option 3: Pre-configured Settings

Create a pre-configured `settings.json` and include it:
```json
{
  "AppPath": "...",
  "Port": 3000,
  "AutoStartServer": true,
  "RememberWindowPosition": true
}
```

Place in: `%APPDATA%\NT-QA-Launcher\settings.json` before distributing

## Distribution Checklist

- [ ] Test launcher on clean Windows 10/11 machine
- [ ] Verify .NET 9.0 detection works
- [ ] Test Start Server functionality
- [ ] Test Open Browser functionality
- [ ] Test Stop Server functionality
- [ ] Test Settings dialog
- [ ] Verify settings persist after restart
- [ ] Test portable execution from USB drive
- [ ] Create ZIP archive for distribution
- [ ] Write clear README for users
- [ ] Include troubleshooting guide
- [ ] Create distribution package

## File Size Summary

| File | Size | Type |
|------|------|------|
| NTQAAppLauncher.exe | 153 KB | Executable |
| NTQAAppLauncher.dll | 23 KB | Assembly |
| NTQAAppLauncher.pdb | 17 KB | Debug Symbols |
| QUICK-START.txt | 4.3 KB | Documentation |
| Launch-NT-QA-Launcher.bat | 820 B | Batch Script |
| **Total (Portable)** | **~160 KB** | **Minimal** |
| **Total (with PDB)** | **~190 KB** | **Development** |

## Distribution Recommendations

### For End Users
- **Format**: Portable Package (ZIP)
- **Size**: ~160 KB
- **Method**: Email, USB drive, or cloud storage
- **Requirements**: .NET 9.0, Node.js
- **Time to Setup**: 2-5 minutes

### For Developers
- **Format**: Source Code + Compiled Release
- **Include**: README.md + DISTRIBUTION-GUIDE.md
- **Repository**: GitHub or internal server
- **Build**: `dotnet build -c Release`

### For Enterprise
- **Format**: MSI Installer (future enhancement)
- **Include**: .NET 9.0 bundled
- **Deployment**: Group Policy or software deployment tool
- **Configuration**: Domain-based settings

## Support Resources

**For Users**:
- QUICK-START.txt - Quick start guide
- Launcher Help menu (future feature)
- Troubleshooting section in QUICK-START.txt

**For Developers**:
- README.md - Technical documentation
- DISTRIBUTION-GUIDE.md - This file
- Source code with comments

## Future Enhancements

Potential improvements for future versions:
- [ ] MSI installer for enterprise deployment
- [ ] Digital signing for security
- [ ] Auto-update functionality
- [ ] Multi-language support
- [ ] Tray icon integration
- [ ] Server log viewer
- [ ] Error reporting

## Version Information

| Property | Value |
|----------|-------|
| Version | 1.0.0 |
| Release Date | March 2026 |
| Platform | Windows 10/11 |
| Framework | .NET 9.0 |
| Executable Size | 153 KB |
| Architecture | 64-bit (x64) |
| License | Claude Code Workspace |

## Quick Distribution Commands

### Create ZIP for Distribution
```bash
cd D:\NT-QA-App-Launcher
tar -czf NT-QA-Launcher-v1.0.0.zip Portable\
```

### Create Backup
```bash
xcopy "D:\NT-QA-App-Launcher\Portable\*" "E:\Backup\NT-QA-Launcher\" /E /I
```

### Deploy to USB (Windows)
```bash
xcopy "D:\NT-QA-App-Launcher\Portable\*" "E:\*" /E /I /Y
```

### Deploy to USB (PowerShell)
```powershell
Copy-Item "D:\NT-QA-App-Launcher\Portable\*" -Destination "E:\" -Recurse -Force
```

## Contact & Support

For issues or feature requests, refer to the main project documentation or contact the development team.

---

**NT Q&A Application Launcher v1.0.0**
Created: March 2026
Ready for Distribution ✓
