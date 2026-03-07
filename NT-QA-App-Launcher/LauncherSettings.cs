using System;
using System.IO;
using System.Text.Json;

namespace NTQAAppLauncher
{
    /// <summary>
    /// Manages application settings persistence in APPDATA
    /// </summary>
    public class LauncherSettings
    {
        public string AppPath { get; set; } = LauncherConfig.APP_PATH;
        public int Port { get; set; } = LauncherConfig.DEFAULT_PORT;
        public bool AutoStartServer { get; set; } = false;
        public bool RememberWindowPosition { get; set; } = true;
        public int? WindowX { get; set; }
        public int? WindowY { get; set; }
        public int? WindowWidth { get; set; }
        public int? WindowHeight { get; set; }

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Load settings from APPDATA, return defaults if file doesn't exist
        /// </summary>
        public static LauncherSettings Load()
        {
            try
            {
                string settingsPath = GetSettingsPath();
                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    return JsonSerializer.Deserialize<LauncherSettings>(json, JsonOptions) ?? CreateDefaults();
                }
            }
            catch
            {
                // On error, fall through to defaults
            }
            return CreateDefaults();
        }

        /// <summary>
        /// Save settings to APPDATA
        /// </summary>
        public void Save()
        {
            try
            {
                string settingsPath = GetSettingsPath();
                string directory = Path.GetDirectoryName(settingsPath) ?? "";

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(this, JsonOptions);
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                // Log error but don't crash - settings are optional
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Get the full path to settings.json in APPDATA
        /// </summary>
        public static string GetSettingsPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataPath, "NT-QA-Launcher", "settings.json");
        }

        /// <summary>
        /// Create default settings object
        /// </summary>
        private static LauncherSettings CreateDefaults()
        {
            return new LauncherSettings
            {
                AppPath = LauncherConfig.APP_PATH,
                Port = LauncherConfig.DEFAULT_PORT,
                AutoStartServer = false,
                RememberWindowPosition = true
            };
        }
    }
}
