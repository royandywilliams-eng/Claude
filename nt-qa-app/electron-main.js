const { app, BrowserWindow, ipcMain, Tray, Menu, dialog, nativeImage } = require('electron');
const path = require('path');
const fs = require('fs');
const { spawn } = require('child_process');

// Keep a global reference of the window object, if you don't, the window will
// be closed when the JavaScript object is garbage collected
let mainWindow;
let tray = null;
let expressServer = null;

// Create the browser window
function createWindow() {
    mainWindow = new BrowserWindow({
        width: 1400,
        height: 900,
        minWidth: 800,
        minHeight: 600,
        webPreferences: {
            preload: path.join(__dirname, 'preload.js'),
            contextIsolation: true,
            enableRemoteModule: false,
            nodeIntegration: false
        },
        icon: path.join(__dirname, 'assets/icon.png') // Optional: add icon
    });

    // Load the app
    mainWindow.loadFile(path.join(__dirname, 'public/index.html'));

    // Open DevTools in development
    // mainWindow.webContents.openDevTools();

    // Handle window closed
    mainWindow.on('closed', () => {
        mainWindow = null;
    });

    // Handle minimize to tray
    mainWindow.on('minimize', (event) => {
        event.preventDefault();
        mainWindow.hide();
    });

    mainWindow.on('close', (event) => {
        if (!app.isQuitting) {
            event.preventDefault();
            mainWindow.hide();
            return false;
        }
    });

    // Focus window when user interacts
    mainWindow.webContents.on('before-input-event', (event, input) => {
        if (input.control && input.shift && input.key.toLowerCase() === 'd') {
            mainWindow.webContents.openDevTools();
        }
    });
}

// Create system tray
function createTray() {
    // Create a simple icon (can be replaced with actual icon file)
    const iconPath = path.join(__dirname, 'assets/tray-icon.png');
    let trayIcon;

    if (fs.existsSync(iconPath)) {
        trayIcon = nativeImage.createFromPath(iconPath);
    } else {
        // Fallback: create a basic icon programmatically
        trayIcon = nativeImage.createEmpty();
    }

    tray = new Tray(trayIcon);

    const contextMenu = Menu.buildFromTemplate([
        {
            label: 'Open NT-QA App',
            click: () => {
                if (mainWindow) {
                    mainWindow.show();
                    mainWindow.focus();
                }
            }
        },
        {
            type: 'separator'
        },
        {
            label: 'Settings',
            click: () => {
                // TODO: Open settings dialog
                if (mainWindow) {
                    mainWindow.show();
                    mainWindow.focus();
                }
            }
        },
        {
            type: 'separator'
        },
        {
            label: 'Quit NT-QA App',
            click: () => {
                app.isQuitting = true;
                app.quit();
            }
        }
    ]);

    tray.setContextMenu(contextMenu);

    // Click on tray icon to show/hide window
    tray.on('click', () => {
        if (mainWindow && mainWindow.isVisible()) {
            mainWindow.hide();
        } else if (mainWindow) {
            mainWindow.show();
            mainWindow.focus();
        }
    });

    // Tooltip
    tray.setToolTip('NT-QA App - Click to toggle window');
}

// Start Express server
function startExpressServer() {
    return new Promise((resolve, reject) => {
        try {
            // Get the server.js path
            const serverPath = path.join(__dirname, 'public/server.js');

            // Check if server.js exists
            if (!fs.existsSync(serverPath)) {
                reject(new Error('server.js not found at ' + serverPath));
                return;
            }

            // Start the server process
            expressServer = spawn('node', [serverPath], {
                cwd: path.join(__dirname, 'public'),
                stdio: 'ignore'
            });

            expressServer.on('error', (err) => {
                console.error('Express server error:', err);
                reject(err);
            });

            // Give server time to start
            setTimeout(() => {
                resolve();
            }, 1000);
        } catch (error) {
            reject(error);
        }
    });
}

// Stop Express server
function stopExpressServer() {
    if (expressServer) {
        expressServer.kill();
        expressServer = null;
    }
}

// IPC Handlers

// Save file dialog and write file
ipcMain.handle('save-file', async (event, { data, filename, defaultPath }) => {
    try {
        const result = await dialog.showSaveDialog(mainWindow, {
            defaultPath: defaultPath || filename,
            filters: [
                { name: 'Text Files', extensions: ['txt'] },
                { name: 'All Files', extensions: ['*'] }
            ]
        });

        if (!result.canceled) {
            fs.writeFileSync(result.filePath, data, 'utf-8');
            return {
                success: true,
                filePath: result.filePath,
                filename: path.basename(result.filePath)
            };
        }
        return { success: false };
    } catch (error) {
        console.error('Error saving file:', error);
        throw error;
    }
});

// Show notification
ipcMain.handle('show-notification', async (event, { title, body }) => {
    try {
        const { Notification } = require('electron');
        new Notification({
            title: title,
            body: body,
            icon: path.join(__dirname, 'assets/icon.png')
        });
        return { success: true };
    } catch (error) {
        console.error('Error showing notification:', error);
        throw error;
    }
});

// App event handlers
app.on('ready', async () => {
    try {
        // Start Express server
        await startExpressServer();

        // Create window
        createWindow();

        // Create tray icon
        createTray();
    } catch (error) {
        console.error('Error starting app:', error);
        app.quit();
    }
});

app.on('window-all-closed', () => {
    // On macOS, apps stay active until user quits explicitly
    if (process.platform !== 'darwin') {
        app.quit();
    }
});

app.on('activate', () => {
    // On macOS, re-create window when dock icon is clicked
    if (mainWindow === null) {
        createWindow();
    } else {
        mainWindow.show();
        mainWindow.focus();
    }
});

app.on('will-quit', () => {
    stopExpressServer();
});

// Security: Disable navigation to external sites
app.on('web-contents-created', (event, contents) => {
    contents.on('will-navigate', (event, navigationUrl) => {
        const parsedUrl = new URL(navigationUrl);

        // Allow only local app navigation
        if (parsedUrl.origin !== 'http://localhost:3000') {
            event.preventDefault();
        }
    });

    // Prevent opening new windows
    contents.setWindowOpenHandler(({ url }) => {
        if (url.startsWith('http')) {
            // Allow opening external links in default browser
            require('electron').shell.openExternal(url);
        }
        return { action: 'deny' };
    });
});

// Handle any uncaught exceptions
process.on('uncaughtException', (error) => {
    console.error('Uncaught exception:', error);
});
