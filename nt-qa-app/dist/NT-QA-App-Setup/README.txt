================================================================================
NT Q&A APPLICATION - SETUP INSTRUCTIONS
================================================================================

QUICK START
-----------
1. Double-click "NT-QA-Setup.bat"
2. Follow the setup wizard prompts
3. Wait for installation to complete
4. Use the shortcut to launch the app

SYSTEM REQUIREMENTS
-------------------
- Windows 7 or later (Windows 10/11 recommended)
- Node.js v14.0 or higher (Download: https://nodejs.org)
- Python 3.7 or later for setup wizard
- ~100 MB free disk space
- Port 3000 available (or another port you choose)

BEFORE YOU START
----------------
If you don't have Node.js installed:
1. Visit https://nodejs.org
2. Download the LTS (Long Term Support) version
3. Run the installer and accept defaults
4. Make sure to CHECK "Add to PATH"
5. Restart your computer
6. Then run NT-QA-Setup.bat again

IF YOU DON'T HAVE PYTHON
------------------------
The setup wizard needs Python. If you get an error:
1. Visit https://www.python.org
2. Download Python 3.7 or later
3. Run the installer
4. CHECK "Add Python to PATH"
5. Complete installation
6. Try running NT-QA-Setup.bat again

SETUP PROCESS
-------------
The setup wizard will guide you through:
1. Welcome screen
2. System check (verifies Node.js)
3. Configuration (port, theme, API keys)
4. Installation (npm packages)
5. Shortcut creation
6. Quick start guide

MANUAL SETUP (IF WIZARD FAILS)
------------------------------
If you prefer to install manually:

1. Open Command Prompt or PowerShell
2. Navigate to the app folder:
   cd "path\to\nt-qa-app"
3. Install dependencies:
   npm install
4. Start the app:
   npm start
5. Open browser to:
   http://localhost:3000

TROUBLESHOOTING
---------------

Q: "Python is not installed or not in PATH"
A: Install Python from https://www.python.org
   During installation, CHECK "Add Python to PATH"
   Then restart your computer

Q: "Node.js is not installed"
A: Install Node.js from https://nodejs.org (LTS version)
   During installation, CHECK "Add to PATH"
   Restart your computer and try again

Q: "npm install failed"
A: Try these steps:
   - Close other npm windows
   - Clear npm cache: npm cache clean --force
   - Delete node_modules folder and package-lock.json
   - Run npm install again
   - If still failing, check antivirus isn't blocking npm

Q: "Port 3000 is already in use"
A: Choose a different port in the setup wizard
   (For example: 3001, 3002, 8000, 8080)

Q: "Can't create shortcuts"
A: The app will still work. You can:
   - Create shortcuts manually
   - Use NT-QA-App.bat (created during setup)
   - Run 'npm start' from command line

MORE HELP
---------
For more information, visit the app documentation or check:
- README.md in the app folder
- INSTALLATION.md for detailed setup help
- Project repository for updates

CONTACT
-------
For issues or questions, please check the project documentation
or community forums.

================================================================================
