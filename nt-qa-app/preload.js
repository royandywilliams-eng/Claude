const { contextBridge, ipcRenderer } = require('electron');

// Expose protected methods that allow the renderer process to use
// the ipcRenderer without exposing the entire object
contextBridge.exposeInMainWorld(
    'electronAPI',
    {
        // File operations
        saveFile: async (data, filename, defaultPath) => {
            return await ipcRenderer.invoke('save-file', {
                data,
                filename,
                defaultPath
            });
        },

        // Notifications
        showNotification: async (title, body) => {
            return await ipcRenderer.invoke('show-notification', {
                title,
                body
            });
        },

        // Settings/Preferences (can be extended)
        getSetting: async (key) => {
            return await ipcRenderer.invoke('get-setting', key);
        },

        setSetting: async (key, value) => {
            return await ipcRenderer.invoke('set-setting', key, value);
        },

        // App version info
        getAppVersion: () => {
            return ipcRenderer.sendSync('get-app-version');
        }
    }
);

console.log('Preload script loaded - electronAPI exposed');
