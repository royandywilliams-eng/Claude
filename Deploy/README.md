# ProjectSpec GUI - Deployment Package

## Overview

**ProjectSpec GUI** is a Windows desktop application for configuring software projects and sending specifications directly to Claude AI for implementation planning.

**Version**: 1.0.0 (Phases 1-5 Complete)

## System Requirements

- **OS**: Windows 11 (or Windows 10 with .NET 6 Runtime)
- **.NET Runtime**: .NET 6.0 or later
- **RAM**: 512 MB minimum (2 GB recommended)
- **Disk Space**: 100 MB
- **Internet**: Required for Claude API integration

## Installation

### Option 1: Direct Run (Portable)

1. Extract all files to a folder (e.g., `C:\Program Files\ProjectSpecGUI`)
2. Double-click `ProjectSpecGUI.exe`
3. Application launches immediately - no installation required

### Option 2: Create Shortcut

1. Right-click `ProjectSpecGUI.exe`
2. Select "Create shortcut"
3. Move shortcut to Desktop or Start Menu

## Uninstallation

ProjectSpec GUI is **100% portable** with no system modifications. Uninstall is simple:

### Option 1: Automated Uninstall (Easiest)

1. Open the folder where you extracted ProjectSpecGUI
2. Double-click `UNINSTALL.bat`
3. Confirm the uninstall when prompted
4. Done. Application and settings completely removed.

### Option 2: Manual Removal

1. Delete the folder containing ProjectSpecGUI
2. (Optional) Delete `%APPDATA%\ProjectSpecGUI\` to remove settings and history
3. Done. No registry entries or system files to clean.

**Note**: No uninstall entries appear in Control Panel because this is a portable application with zero system modifications.

## Quick Start

1. **Launch Application**
   - Double-click `ProjectSpecGUI.exe`

2. **Configure Claude API**
   - Go to **Claude → Settings**
   - Enter your Claude API key (get from https://console.anthropic.com)
   - Select preferred model (Opus 4.6, Sonnet 3.5, or Haiku 3)
   - Click "Test Connection" to verify
   - Click OK

3. **Create Project Configuration**
   - Enter project details (name, type, complexity)
   - Select technology stack (language, frameworks, database)
   - Choose key features (auth, payments, search, etc.)
   - Configure UI/UX requirements
   - Set performance expectations
   - Specify deployment options

4. **Send to Claude**
   - Click **Claude → Send to Claude**
   - Wait for progress dialog to complete
   - Review generated implementation plan in response window
   - Copy code or save to file

5. **View History**
   - Click **Claude → View History** to see previous API calls
   - Max 100 entries retained (auto-trimmed oldest first)

## Features

### Configuration Management
- ✅ 6-step wizard for project setup
- ✅ 9 advanced configuration tabs
- ✅ 200+ customizable options
- ✅ Real-time specification preview (Markdown, JSON, Claude Prompt)
- ✅ Save/load configurations as JSON files

### Claude API Integration
- ✅ Multiple model selection (Opus, Sonnet, Haiku)
- ✅ Configurable parameters (timeout, max tokens, temperature)
- ✅ API call history tracking (up to 100 entries)
- ✅ Response display with token usage
- ✅ Copy to clipboard or save to file
- ✅ Error handling with user-friendly messages

### Data Persistence
- ✅ Settings stored in `%APPDATA%\ProjectSpecGUI\settings.json`
- ✅ History stored in `%APPDATA%\ProjectSpecGUI\history.json`
- ✅ Import/export project configurations

## File Structure

```
ProjectSpecGUI/
├── ProjectSpecGUI.exe          # Main application executable
├── ProjectSpecGUI.dll          # Core application library
├── ProjectSpecGUI.pdb          # Debug symbols
├── Newtonsoft.Json.dll         # JSON serialization library
├── ProjectSpecGUI.deps.json    # Dependency manifest
├── ProjectSpecGUI.runtimeconfig.json  # Runtime configuration
└── README.md                   # This file
```

## Configuration Files

**Location**: `%APPDATA%\ProjectSpecGUI\`

### settings.json
```json
{
  "ApiKey": "sk-...",
  "SelectedModel": "claude-opus-4.6",
  "TimeoutSeconds": 60,
  "MaxTokens": 4096,
  "Temperature": 0.7,
  "SaveHistory": true
}
```

### history.json
```json
[
  {
    "projectName": "My App",
    "sentAt": "2026-03-06T10:30:00",
    "success": true,
    "tokensUsed": 5432,
    "responsePreview": "..."
  }
]
```

## Troubleshooting

### Application Won't Start
- **Issue**: ".NET Runtime not found"
- **Fix**: Install .NET 6 Runtime from https://dotnet.microsoft.com/download/dotnet/6.0

### API Key Error
- **Issue**: "Invalid API key"
- **Fix**:
  1. Verify key at https://console.anthropic.com
  2. Ensure no extra spaces in API key field
  3. Click "Test Connection" in Settings

### Request Timeout
- **Issue**: "Request timed out (>60s)"
- **Fix**:
  1. Increase timeout in Settings (Claude → Settings)
  2. Simplify configuration (fewer frameworks/features)
  3. Check internet connection

### Settings/History Not Saving
- **Issue**: Files not appearing in %APPDATA%
- **Fix**:
  1. Right-click `%APPDATA%` and check folder is accessible
  2. Ensure no antivirus blocking file access
  3. Run application as Administrator

## API Key Security

⚠️ **Important**: API keys are stored in plain text in `%APPDATA%\ProjectSpecGUI\settings.json`

**Security Best Practices**:
- Never share your API key
- Restrict file permissions on settings.json
- Use environment-specific keys if possible
- Rotate key if compromised
- Consider encrypting settings file for production use

## Supported Languages & Frameworks

**Languages**: JavaScript/TypeScript, Python, C#, Java, Go, Rust, PHP, Ruby, Swift, Kotlin

**Frameworks**: React, Vue, Angular, Django, Flask, ASP.NET Core, Spring Boot, and 40+ others

**Databases**: PostgreSQL, MySQL, MongoDB, Firebase, SQLite, and 7+ others

**Hosting**: AWS, Azure, Google Cloud, Heroku, DigitalOcean, and 6+ others

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl+N | New Configuration |
| Ctrl+O | Open Configuration |
| Ctrl+S | Save Configuration |
| Alt+F4 | Exit Application |

## Support & Documentation

- **Code Repository**: https://github.com/royandywilliams-eng/Claude
- **Claude API Docs**: https://docs.anthropic.com
- **Report Issues**: Open an issue on GitHub

## Version History

### v1.0.0 (March 2026)
- ✅ Phase 1: UI Framework (1,147 lines)
- ✅ Phase 2: Wizard Screens (1,152 lines)
- ✅ Phase 3: Configuration Tabs (1,927 lines)
- ✅ Phase 4: Configuration Engine (400 lines)
- ✅ Phase 5: Claude API Integration (800 lines)
- **Total**: 7,300+ lines of production code

## License

MIT License - See repository for details

## Contact

For questions or feedback, please visit the GitHub repository.

---

**ProjectSpec GUI v1.0.0** | Windows 11 | .NET 6.0 | March 2026
