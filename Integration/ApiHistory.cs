using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectSpecGUI.Integration
{
    /// <summary>
    /// History entry for a sent configuration and its response
    /// </summary>
    public class HistoryEntry
    {
        [JsonPropertyName("projectName")]
        public string ProjectName { get; set; } = "";

        [JsonPropertyName("sentAt")]
        public DateTime SentAt { get; set; }

        [JsonPropertyName("configurationJson")]
        public string ConfigurationJson { get; set; } = "";

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("tokensUsed")]
        public int TokensUsed { get; set; }

        [JsonPropertyName("responsePreview")]
        public string ResponsePreview { get; set; } = "";

        public override string ToString()
        {
            string status = Success ? "✓" : "✗";
            return $"{status} {ProjectName} - {SentAt:yyyy-MM-dd HH:mm}";
        }
    }

    /// <summary>
    /// Manages history of API calls and responses
    /// Stored in %APPDATA%\ProjectSpecGUI\history.json
    /// </summary>
    public class ApiHistory
    {
        private List<HistoryEntry> _entries = new List<HistoryEntry>();
        private const int MaxEntries = 100;

        /// <summary>
        /// Load history from file
        /// </summary>
        public static ApiHistory Load()
        {
            var history = new ApiHistory();

            try
            {
                string historyPath = GetHistoryPath();

                if (File.Exists(historyPath))
                {
                    string json = File.ReadAllText(historyPath);
                    var entries = JsonSerializer.Deserialize<List<HistoryEntry>>(json, GetJsonOptions());
                    history._entries = entries ?? new List<HistoryEntry>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading history: {ex.Message}");
            }

            return history;
        }

        /// <summary>
        /// Add entry to history and save
        /// </summary>
        public void AddEntry(HistoryEntry entry)
        {
            if (entry == null)
                return;

            _entries.Add(entry);

            // Trim to max entries (keep newest)
            if (_entries.Count > MaxEntries)
            {
                _entries = _entries.Skip(_entries.Count - MaxEntries).ToList();
            }

            Save();
        }

        /// <summary>
        /// Get all history entries
        /// </summary>
        public List<HistoryEntry> GetHistory()
        {
            return new List<HistoryEntry>(_entries.OrderByDescending(e => e.SentAt));
        }

        /// <summary>
        /// Get history entries for a specific project
        /// </summary>
        public List<HistoryEntry> GetHistoryForProject(string projectName)
        {
            return _entries
                .Where(e => e.ProjectName == projectName)
                .OrderByDescending(e => e.SentAt)
                .ToList();
        }

        /// <summary>
        /// Clear all history
        /// </summary>
        public void ClearHistory()
        {
            _entries.Clear();
            Save();
        }

        /// <summary>
        /// Save history to file
        /// </summary>
        public void Save()
        {
            try
            {
                string historyPath = GetHistoryPath();
                string directory = Path.GetDirectoryName(historyPath);

                // Create directory if needed
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(_entries, GetJsonOptions());
                File.WriteAllText(historyPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving history: {ex.Message}");
            }
        }

        /// <summary>
        /// Export history as JSON string
        /// </summary>
        public string ExportAsJson()
        {
            return JsonSerializer.Serialize(_entries, GetJsonOptions());
        }

        /// <summary>
        /// Get the full path to the history file
        /// </summary>
        public static string GetHistoryPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, "ProjectSpecGUI", "history.json");
        }

        /// <summary>
        /// Get the number of entries in history
        /// </summary>
        public int Count => _entries.Count;

        /// <summary>
        /// Get JSON serializer options
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
