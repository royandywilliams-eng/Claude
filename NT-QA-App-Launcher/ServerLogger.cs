using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace NTQAAppLauncher
{
    /// <summary>
    /// Captures and manages server output logs
    /// </summary>
    public class ServerLogger : IDisposable
    {
        private readonly List<LogEntry> _logs = new();
        private readonly string _logDirectory;
        private readonly int _maxEntries = 1000;
        private StreamWriter? _fileWriter;

        public event EventHandler<LogEntry>? LogAdded;

        public class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Message { get; set; } = "";
            public LogLevel Level { get; set; }

            public override string ToString()
            {
                return $"[{Timestamp:HH:mm:ss}] [{Level}] {Message}";
            }
        }

        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error
        }

        public ServerLogger()
        {
            _logDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NT-QA-Launcher", "logs");

            Directory.CreateDirectory(_logDirectory);
            InitializeFileWriter();
        }

        private void InitializeFileWriter()
        {
            try
            {
                string logFile = Path.Combine(_logDirectory, $"launcher-{DateTime.Now:yyyy-MM-dd}.log");
                _fileWriter = new StreamWriter(logFile, true, Encoding.UTF8) { AutoFlush = true };
                Log("Logger initialized", LogLevel.Info);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to initialize file writer: {ex.Message}");
            }
        }

        /// <summary>
        /// Add a log entry
        /// </summary>
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Message = message,
                Level = level
            };

            _logs.Add(entry);

            // Keep only last N entries
            if (_logs.Count > _maxEntries)
            {
                _logs.RemoveAt(0);
            }

            // Write to file
            try
            {
                _fileWriter?.WriteLine(entry.ToString());
            }
            catch { }

            // Raise event
            LogAdded?.Invoke(this, entry);
        }

        /// <summary>
        /// Get all logs
        /// </summary>
        public List<LogEntry> GetLogs()
        {
            lock (_logs)
            {
                return new List<LogEntry>(_logs);
            }
        }

        /// <summary>
        /// Get logs filtered by level
        /// </summary>
        public List<LogEntry> GetLogs(LogLevel level)
        {
            lock (_logs)
            {
                return new List<LogEntry>(_logs.FindAll(l => l.Level == level));
            }
        }

        /// <summary>
        /// Clear all logs
        /// </summary>
        public void Clear()
        {
            _logs.Clear();
            Log("Logs cleared", LogLevel.Info);
        }

        /// <summary>
        /// Export logs to text file
        /// </summary>
        public void ExportLogs(string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    foreach (var entry in _logs)
                    {
                        writer.WriteLine(entry.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Failed to export logs: {ex.Message}", LogLevel.Error);
            }
        }

        /// <summary>
        /// Clean old log files (keep last 5)
        /// </summary>
        public void CleanupOldLogs()
        {
            try
            {
                var files = Directory.GetFiles(_logDirectory, "launcher-*.log");
                if (files.Length > 5)
                {
                    Array.Sort(files);
                    for (int i = 0; i < files.Length - 5; i++)
                    {
                        File.Delete(files[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Failed to cleanup logs: {ex.Message}", LogLevel.Warning);
            }
        }

        public void Dispose()
        {
            _fileWriter?.Dispose();
        }
    }
}
