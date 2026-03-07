# NT Q&A Application Launcher

A Windows Forms (.NET 9.0) launcher for the NT Q&A Web Application.

## Overview

This launcher provides a simple, one-click interface to:
- ✅ Start the Node.js server for the NT Q&A App
- ✅ Automatically detect server status
- ✅ Open the app in your default browser
- ✅ Stop the server when done
- ✅ Persist user settings

## Features

### Main Window
- **Status Indicator** - Visual indicator showing if server is running (● Red/Green)
- **Server Controls** - Start, Stop, and Open Browser buttons
- **Auto-detection** - Automatically detects if server is already running
- **Status Messages** - Real-time feedback on what the launcher is doing
- **Settings** - Configure app path, port, and auto-start preference

### Automatic Features
- **Real-time Monitoring** - Checks server status every 2 seconds
- **Auto-start Option** - Optional auto-start when launcher opens
- **Window Position Memory** - Remembers where you positioned the window
- **Settings Persistence** - Saves preferences to APPDATA

## Requirements

- Windows 10 or Windows 11
- .NET 9.0 runtime or Visual Studio 2022
- Node.js (for running the NT Q&A App server)

## Project Structure

```
D:\NT-QA-App-Launcher\
├── Program.cs                      # Entry point
├── MainForm.cs                     # Main launcher window
├── LauncherConfig.cs              # Configuration constants
├── ProcessManager.cs              # Server process management
├── LauncherSettings.cs            # Settings persistence
├── LauncherSettingsDialog.cs      # Settings window
├── NT-QA-App-Launcher.csproj     # Project file
└── README.md                       # This file
```

## Building

### From Command Line
```bash
cd D:\NT-QA-App-Launcher
dotnet build
dotnet run
```

### From Visual Studio
1. Open `NT-QA-App-Launcher.csproj` in Visual Studio 2022
2. Press F5 to run
3. Or Build → Build Solution to create executable

## Running

### Debug Build
```bash
D:\NT-QA-App-Launcher\bin\Debug\net9.0-windows\NTQAAppLauncher.exe
```

### Release Build
```bash
dotnet build -c Release
D:\NT-QA-App-Launcher\bin\Release\net9.0-windows\NTQAAppLauncher.exe
```

## Usage

1. **Start Server**: Click "Start Server" button
   - Launcher will start Node.js server on port 3000
   - Status will change to "Running" when ready
   - Takes about 5-10 seconds to start

2. **Open Browser**: Click "Open Browser" button
   - Opens http://localhost:3000 in your default browser
   - Only enabled when server is running

3. **Stop Server**: Click "Stop Server" button
   - Gracefully terminates the Node.js process
   - Status changes to "Stopped"

4. **Settings**: Click "Settings" button
   - **App Path**: Change where the launcher looks for the NT-QA-App
   - **Port**: Change server port (default: 3000)
   - **Auto-start**: Enable automatic server startup

## Settings Storage

Settings are stored in:
```
%APPDATA%\NT-QA-Launcher\settings.json
```

Example settings.json:
```json
{
  "AppPath": "D:\\NT-QA-App-Project\\dist\\NT-QA-App-Setup\\app",
  "Port": 3000,
  "AutoStartServer": false,
  "RememberWindowPosition": true,
  "WindowX": 100,
  "WindowY": 100
}
```

## Troubleshooting

### Server Won't Start
- Check that Node.js is installed (`npm --version` in Command Prompt)
- Verify the App Path in Settings points to the correct directory
- Make sure port 3000 is not in use by another application

### Port Already in Use
- Change port in Settings (try 3001, 3002, etc.)
- Or find and stop the process using port 3000:
  ```bash
  netstat -ano | findstr :3000
  taskkill /PID <PID> /F
  ```

### Browser Won't Open
- Check internet connection (if API features are needed)
- Try opening http://localhost:3000 manually in your browser

### Launcher Won't Start
- Ensure .NET 9.0 runtime is installed
- Run from Command Prompt to see error details:
  ```bash
  D:\NT-QA-App-Launcher\bin\Debug\net9.0-windows\NTQAAppLauncher.exe
  ```

## Development

### Architecture
- **ProcessManager**: Handles Node.js process creation, monitoring, and termination
- **MainForm**: UI and main application logic
- **LauncherSettings**: JSON-based settings persistence
- **LauncherConfig**: Centralized configuration constants

### Key Technologies
- Windows Forms (UI framework)
- System.Diagnostics.Process (process management)
- System.Net.Sockets (port detection)
- System.Text.Json (settings serialization)

### Design Patterns
- Single Responsibility: Each class has one job
- Settings Pattern: Centralized configuration management
- Timer Pattern: Periodic status checks
- Dialog Pattern: Separate settings window

## Future Enhancements

Potential improvements:
- [ ] Tray icon support (minimize to system tray)
- [ ] Server log viewer (capture and display server output)
- [ ] Multiple app instances (launch different Node.js apps)
- [ ] Custom startup arguments (pass flags to Node.js)
- [ ] Auto-update functionality
- [ ] Dark mode support

## License

Part of the Claude Code Workspace project.

## Version

- **Version**: 1.0.0
- **Created**: March 2026
- **Platform**: Windows 10/11
- **Runtime**: .NET 9.0

---

**Built with**: Visual Studio 2022 | .NET 9.0 | Windows Forms | C#
