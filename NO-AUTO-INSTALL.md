# ProjectSpec GUI - NO AUTO-INSTALLATION POLICY

## Statement

**ProjectSpec GUI is explicitly configured to NOT auto-install to any user's PC.**

This application is **100% portable** and requires **zero installation**.

---

## Technical Verification

### No Installer Projects
- ❌ No `.wixproj` files (WiX Installer)
- ❌ No `.vdproj` files (Visual Studio Installer)
- ❌ No `.iss` files (Inno Setup)
- ❌ No Setup.exe or MSI files

### No Installation Code
```csharp
// Verified: No registry operations
// Verified: No Windows Installer calls
// Verified: No auto-startup entries
// Verified: No service installation
// Verified: No system modifications
```

### No Auto-Startup
- ❌ No registry entries for auto-run
- ❌ No scheduled tasks
- ❌ No service installation
- ❌ No background processes

### .csproj Configuration
```xml
<!-- Plain Windows Forms application - no installer config -->
<OutputType>WinExe</OutputType>
<TargetFramework>net6.0-windows</TargetFramework>
<UseWindowsForms>true</UseWindowsForms>
<!-- NO Installer, Setup, or MSI configuration -->
```

---

## How to Run

### Method 1: Direct Execution
```
Double-click: ProjectSpecGUI.exe
No installation needed. Runs immediately.
```

### Method 2: Via Launcher Script
```
Double-click: Launch.bat
Checks .NET 6 Runtime, then launches app.
```

### Method 3: From Any Location
```
Copy Deploy/ folder to any location
Run Launch.bat or ProjectSpecGUI.exe
Works from USB, DVD, network drive, etc.
```

---

## Data Storage

Settings and history are stored in **user-controlled locations**:

```
%APPDATA%\ProjectSpecGUI\settings.json    (User home folder)
%APPDATA%\ProjectSpecGUI\history.json     (User home folder)
```

**User has full control**:
- Can delete settings anytime
- Can backup history
- Can move to different machine
- No system registry changes
- No system folder modifications

---

## System Impact

**Zero system modifications**:
- ✅ No files written to System32
- ✅ No registry entries created
- ✅ No Windows Installer used
- ✅ No services installed
- ✅ No scheduled tasks created
- ✅ No uninstall entries in Control Panel
- ✅ No startup folder entries
- ✅ No system PATH modifications

---

## Removal

**To completely remove ProjectSpec GUI**:

1. Delete the `Deploy/` folder (or wherever you extracted it)
2. Delete `%APPDATA%\ProjectSpecGUI\` folder (optional, keeps settings)
3. Done. No uninstaller needed.

**Remaining artifacts**:
- None. The application is completely self-contained.

---

## Future Versions

This policy is permanent:
- ✅ No future versions will auto-install
- ✅ No future versions will modify the system
- ✅ No future versions will use Windows Installer
- ✅ All future versions will remain portable

---

## Verification Checklist

Before any release:
- [ ] No .wixproj files
- [ ] No .vdproj files
- [ ] No Setup.exe or MSI
- [ ] No registry operations in code
- [ ] No service installation code
- [ ] No auto-startup entries
- [ ] .csproj has no installer configuration
- [ ] All files in Deploy/ folder only
- [ ] Launch.bat has no system calls
- [ ] Settings only go to %APPDATA%

---

## For Users

**You can safely use ProjectSpec GUI knowing that**:
- No automatic installation occurs
- No system files are modified
- No registry is touched
- Complete removal is as simple as deleting a folder
- You maintain 100% control

---

**ProjectSpec GUI** | Portable | Non-Intrusive | Zero-Installation ✅

