using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectSpecGUI.Integration
{
    /// <summary>
    /// Manages Claude API settings including credentials and preferences
    /// Stores settings in %APPDATA%\ProjectSpecGUI\settings.json
    /// </summary>
    public class ClaudeSettings
    {
        public string ApiKey { get; set; } = "";
        public string SelectedModel { get; set; } = "claude-opus-4.6";
        public int TimeoutSeconds { get; set; } = 60;
        public int MaxTokens { get; set; } = 4096;
        public double Temperature { get; set; } = 0.7;
        public bool SaveHistory { get; set; } = true;

        /// <summary>
        /// Available Claude models (production-ready)
        /// </summary>
        public static readonly List<string> AvailableModels = new List<string>
        {
            "claude-opus-4.6",      // Latest, most capable
            "claude-3.5-sonnet",    // Balanced speed/quality
            "claude-3-haiku"        // Fast, cost-effective
        };

        /// <summary>
        /// Load settings from file, or create with defaults if file doesn't exist
        /// </summary>
        public static ClaudeSettings Load()
        {
            try
            {
                string settingsPath = GetSettingsPath();

                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    var settings = JsonSerializer.Deserialize<ClaudeSettings>(json, GetJsonOptions());
                    return settings ?? CreateDefaults();
                }
            }
            catch (Exception ex)
            {
                // Log error or show warning if desired
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
            }

            return CreateDefaults();
        }

        /// <summary>
        /// Save current settings to file
        /// </summary>
        public void Save()
        {
            try
            {
                string settingsPath = GetSettingsPath();
                string directory = Path.GetDirectoryName(settingsPath);

                // Create directory if it doesn't exist
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(this, GetJsonOptions());
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save settings: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Check if API key is configured and valid
        /// </summary>
        public bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(ApiKey) && ApiKey.Length > 0;
        }

        /// <summary>
        /// Get the full path to the settings file
        /// </summary>
        public static string GetSettingsPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, "ProjectSpecGUI", "settings.json");
        }

        /// <summary>
        /// Get available Claude models
        /// </summary>
        public static List<string> GetAvailableModels()
        {
            return new List<string>(AvailableModels);
        }

        /// <summary>
        /// Create settings with default values
        /// </summary>
        private static ClaudeSettings CreateDefaults()
        {
            return new ClaudeSettings
            {
                ApiKey = "",
                SelectedModel = "claude-opus-4.6",
                TimeoutSeconds = 60,
                MaxTokens = 4096,
                Temperature = 0.7,
                SaveHistory = true
            };
        }

        /// <summary>
        /// Get JSON serializer options with proper formatting
        /// </summary>
        private static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }
    }
}
