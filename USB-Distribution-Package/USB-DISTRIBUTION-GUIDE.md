# USB Flash Drive Distribution Guide

## Overview

ProjectSpec GUI can be distributed via USB flash drive with automatic .NET 6 Runtime installation. This directory contains all the files needed to create a complete distribution package.

## Files Included

- **SETUP.bat** - Automatic setup script (checks for .NET 6, installs if needed)
- **README.txt** - User-friendly instructions and troubleshooting guide
- **INSTALL-DOTNET6-FIRST.txt** - Detailed .NET 6 installation instructions

## Creating a Distribution USB Drive

### Step 1: Gather Required Files

1. **Application Files**
   - Copy entire `Deploy/` folder from this repository
   - Rename to `ProjectSpecGUI`

2. **.NET 6 Runtime Installer**
   - Download from: https://dotnet.microsoft.com/download/dotnet/6.0
   - Select: ".NET 6.0 Runtime" → "x64"
   - File size: ~150 MB
   - File name: `dotnet-runtime-6.0.x-win-x64.exe`

3. **Setup Files**
   - Copy files from this directory:
     - SETUP.bat
     - README.txt
     - INSTALL-DOTNET6-FIRST.txt

### Step 2: Prepare USB Flash Drive

1. Insert blank USB flash drive (8 GB or larger recommended)
2. Create this folder structure:

```
USB Drive (E:\ or F:\, etc.)
├── ProjectSpecGUI/
│   ├── ProjectSpecGUI.exe
│   ├── Launch.bat
│   ├── README.md
│   ├── Newtonsoft.Json.dll
│   └── ...other files from Deploy/
├── dotnet-runtime-6.0.x-win-x64.exe
├── SETUP.bat
├── README.txt
└── INSTALL-DOTNET6-FIRST.txt
```

### Step 3: Copy Files

1. Copy `ProjectSpecGUI` folder to USB root
2. Copy `dotnet-runtime-6.0.x-win-x64.exe` to USB root
3. Copy `SETUP.bat` to USB root
4. Copy `README.txt` to USB root
5. Copy `INSTALL-DOTNET6-FIRST.txt` to USB root

### Step 4: Verify Contents

Check that all files are present:
- ✓ ProjectSpecGUI folder (1.4 MB)
- ✓ dotnet-runtime installer (150 MB)
- ✓ SETUP.bat (2 KB)
- ✓ README.txt (8 KB)
- ✓ INSTALL-DOTNET6-FIRST.txt (2 KB)

**Total size used**: ~151 MB (USB drive can be any size)

### Step 5: Test on Different Computer

1. Insert USB drive into another Windows PC
2. Double-click `SETUP.bat`
3. Verify installer runs and checks for .NET 6
4. If .NET 6 not installed, installer should run automatically
5. Verify ProjectSpec GUI launches successfully

## User Instructions (What Users See)

When a user receives the USB drive, they should:

1. **Insert USB into their Windows PC**
2. **Double-click SETUP.bat**
3. **Wait for installation** (if .NET 6 not installed)
4. **ProjectSpec GUI launches automatically**

## Alternative Setup Methods

### Method 1: Automatic (Recommended)
```
User double-clicks SETUP.bat
→ SETUP.bat checks for .NET 6
→ If not found, installs automatically
→ Launches ProjectSpec GUI
```

### Method 2: Manual
```
User double-clicks dotnet-runtime-6.0.x-win-x64.exe
→ Clicks "Install" button
→ Waits for installation
→ Goes to ProjectSpecGUI folder
→ Double-clicks Launch.bat
```

### Method 3: Pre-installed
```
If user already has .NET 6:
→ Go directly to ProjectSpecGUI folder
→ Double-click Launch.bat
```

## Distribution Methods

### USB Flash Drive
- Size: 8 GB or larger
- Speed: Reusable, fast distribution
- Cost: $1-5 per drive
- Use case: Handing out at events, mailing to clients

### DVD+R
1. Copy USB drive contents to DVD+R
2. Follow CREATE-DVD.txt instructions
3. Label: "ProjectSpec GUI v1.0.0"
4. Use case: Physical distribution, archival

### .ZIP Archive
1. Select all USB drive files
2. Right-click → "Send to" → "Compressed (zipped) folder"
3. Email or upload .zip file
4. User extracts and runs SETUP.bat
5. Use case: Digital distribution, email

### Network Drive
1. Copy USB drive contents to network share
2. Users access and run SETUP.bat from network
3. Use case: Organization-wide deployment

## Troubleshooting Distribution Issues

### Problem: SETUP.bat won't run
**Solution**:
- Ensure file is saved as .bat (not .txt)
- Try right-clicking → "Run as Administrator"
- Check antivirus isn't blocking execution

### Problem: .NET 6 installer fails
**Solution**:
- Verify installer file is complete (150 MB)
- Try manual installation: double-click .exe directly
- Check Windows Defender/antivirus settings
- Try on different USB port

### Problem: ProjectSpec GUI won't start after installation
**Solution**:
- Verify .NET 6 installation completed
- Check .NET 6 version: run `dotnet --version` in Command Prompt
- Verify all DLL files are in ProjectSpecGUI folder
- Check internet connection for API features

### Problem: USB drive not recognized
**Solution**:
- Try different USB port
- Insert drive into different computer
- Check USB drive in Disk Management

## Performance Notes

- **First launch**: May take 5-10 seconds while .NET 6 initializes
- **Subsequent launches**: 1-2 seconds
- **USB speed**: Faster on USB 3.0 drives
- **Network**: Can be slower on older network hardware

## Security Notes

- ✅ No hidden files or registry modifications
- ✅ Safe to distribute without concerns
- ✅ No personal data included
- ✅ Users maintain full control
- ⚠️ API keys stored locally in %APPDATA% (user's responsibility)

## Customization

### Add Your Logo/Branding
- Create `BRANDING.txt` with company information
- Include company logo as `LOGO.png`
- Update README.txt with company details

### Custom Setup Script
- Modify SETUP.bat for custom installation paths
- Add environment variable configuration
- Add registry entries if needed (keep zero-modification principle in mind)

### Pre-configured Settings
- Include `settings.json` with:
  - Default API key (optional)
  - Preferred model selection
  - Custom timeout settings
- Users can override in application

## Deployment Checklist

Before distributing:

- [ ] All files copied to USB drive
- [ ] SETUP.bat tested on different computer
- [ ] .NET 6 installer downloaded (correct version)
- [ ] ProjectSpec GUI launches successfully
- [ ] README.txt is readable
- [ ] INSTALL-DOTNET6-FIRST.txt covers troubleshooting
- [ ] API features tested (internet connection available)
- [ ] Uninstall tested (UNINSTALL.bat works)
- [ ] USB drive labeled with version number
- [ ] Backup copy created

## Distribution Statistics

### Typical Usage
- 5-10 minutes from USB insertion to working application
- 2-5 minutes for .NET 6 installation
- 1-2 minutes for ProjectSpec GUI setup

### Disk Space Requirements
- USB drive: 8 GB (only ~151 MB used)
- Installation location: 100 MB
- User settings: <1 MB

### System Requirements
- OS: Windows 10 or Windows 11
- RAM: 512 MB (2 GB recommended)
- Processor: Any modern processor
- Internet: Required for Claude API features

## Support Resources

For users:
- **README.txt** - Quick start guide
- **INSTALL-DOTNET6-FIRST.txt** - Installation help
- **ProjectSpecGUI/README.md** - Full application documentation
- **GitHub**: https://github.com/royandywilliams-eng/Claude

For distributors:
- **This file** - USB distribution guide
- **CREATE-DVD.txt** - DVD distribution guide
- **NO-AUTO-INSTALL.md** - System safety guarantee

## Version Notes

- **v1.0.0** (Current - March 2026)
  - 7,300+ lines of production code
  - 5 phases complete
  - Production-ready for distribution

## Next Steps

1. Gather files from this guide
2. Create test USB drive
3. Test on multiple computers
4. Create additional copies as needed
5. Label and distribute

Happy distributing! 🚀

---

**ProjectSpec GUI v1.0.0** | USB Distribution | Portable | Zero-Installation
