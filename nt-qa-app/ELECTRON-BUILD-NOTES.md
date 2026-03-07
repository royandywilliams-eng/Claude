# NT-QA App - Electron Windows Desktop Application

**Version**: 2.0.0
**Status**: ✅ Production Ready
**Platform**: Windows 11 Pro (64-bit)
**Framework**: Electron 33.4.11
**Node.js Runtime**: Included

---

## Quick Start

### Run the App
```bash
npm start
```

This will:
1. Start the Express.js backend server (port 3000)
2. Launch the Electron window
3. Display the NT-QA application

---

## Build & Distribution

### Build Unpacked App
```bash
npm run build
```

The unpacked app is created in `dist/win-unpacked/` and contains:
- **NT-QA App.exe** (181 MB) - Fully functional standalone executable
- **resources/app.asar** - Application code bundle
- **locales/** - Language files
- All necessary Electron dependencies

### Distribution Package
The portable app is available in `dist/NT-QA-App-Portable/`

Simply copy the entire `NT-QA-App-Portable/` folder and distribute it.
Users can run `NT-QA App.exe` directly - no installation needed!

---

## Features Implemented

### ✅ Core Functionality
- [x] All 27 New Testament books with 135+ Q&A items
- [x] Full-text search across all content
- [x] Browse by book and chapter
- [x] Random question feature
- [x] Export all Q&A
- [x] Export selected items
- [x] Summarization (extractive and AI-powered via OpenAI API)

### ✅ Windows 11 Integration
- [x] System Tray icon (click to show/hide window)
- [x] Desktop notifications (export complete, search results, etc.)
- [x] Context menu integration (right-click copy/export)
- [x] Windows 11 Fluent Design theme (modern UI with accent colors)
- [x] Native file save dialogs

### ✅ Technical Features
- [x] IPC-based file operations (secure Electron pattern)
- [x] Express.js backend for API
- [x] Preload script with context isolation
- [x] Graceful window management (minimize to tray)
- [x] Auto-start server on app launch
- [x] Proper cleanup on app exit

---

## Architecture

### Project Structure
```
D:\nt-qa-app\
├── electron-main.js          # Main process, window management, IPC
├── preload.js                # Context bridge, secure API exposure
├── public/                   # Frontend and backend files
│   ├── index.html           # Main UI
│   ├── app.js               # Frontend logic (with IPC integration)
│   ├── styles.css           # Styling (including Fluent Design theme)
│   ├── server.js            # Express.js backend
│   └── data/
│       └── nt-data.json     # Q&A database
├── package.json             # npm config with Electron build setup
├── dist/
│   ├── win-unpacked/        # Unpacked Electron app
│   └── NT-QA-App-Portable/  # Distribution package
└── node_modules/            # Dependencies
```

### Key Files

| File | Purpose | Lines |
|------|---------|-------|
| electron-main.js | App initialization, window mgmt, IPC handlers | 220 |
| preload.js | Secure context bridge | 120 |
| public/app.js | Frontend with IPC integration | 19,800+ |
| public/server.js | Express backend (unchanged) | 8,900+ |
| public/styles.css | UI with Fluent Design (updated) | 800+ |
| public/data/nt-data.json | Q&A database | 21 KB |

---

## Development

### Running in Dev Mode
```bash
npm start
```

Press `Ctrl+Shift+D` in the app to open Developer Tools for debugging.

### Building Production
```bash
npm run build
```

The build process:
1. Packages the app with Electron
2. Embeds Node.js runtime
3. Creates unpacked app directory
4. Output: `dist/win-unpacked/NT-QA App.exe`

---

## Windows 11 Fluent Design Theme

The app uses Windows 11 native styling:
- **Color Variables**: CSS custom properties for system colors
- **Rounded Corners**: Modern 8-16px border radius
- **Animations**: Fluent easing curves (cubic-bezier)
- **Focus States**: System accent color on focus
- **Backdrop Effects**: Frosted glass modal styling

Dark mode is automatically detected via `prefers-color-scheme` media query.

---

## Code Signing Note

The electron-builder configuration has code signing disabled (`"sign": null`) because we're distributing an unsigned app. This is fine for internal/organizational use.

For public distribution with automatic updates, consider:
1. Getting an Extended Validation (EV) code signing certificate
2. Enabling code signing in build configuration
3. Setting up auto-update infrastructure

---

## Troubleshooting

### App won't start
- Ensure Node.js and npm are installed
- Run `npm install` to install dependencies
- Check that port 3000 is available

### Server won't start
- Verify `public/server.js` exists
- Check that `public/data/nt-data.json` is present
- Look at console output for error messages

### IPC errors
- Check that preload.js is loaded (see console)
- Verify `window.electronAPI` is available in DevTools

---

## Future Enhancements

Possible additions:
- [ ] Application settings/preferences GUI
- [ ] Dark mode toggle
- [ ] Custom accent color selection
- [ ] Cloud sync for selections
- [ ] Offline package with Scripture references
- [ ] Multi-language support
- [ ] Auto-updater integration
- [ ] Windows Store distribution

---

## Support & Development

For development questions or enhancements:
1. Review the Electron documentation: https://www.electronjs.org/docs
2. Check IPC patterns in electron-main.js and preload.js
3. Modify UI in public/app.js and public/styles.css
4. Update backend APIs in public/server.js

---

**Built with**: Electron 33 + Express.js + Vanilla JavaScript + Windows 11 APIs
**Package Date**: March 6, 2026
**Status**: Production Ready ✅
