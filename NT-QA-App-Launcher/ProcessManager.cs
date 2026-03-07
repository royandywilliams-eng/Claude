using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NTQAAppLauncher
{
    /// <summary>
    /// Manages the Node.js server process lifecycle (start, stop, monitor)
    /// </summary>
    public class ProcessManager
    {
        private Process? _serverProcess;
        private readonly LauncherSettings _settings;
        private ServerLogger? _logger;

        public ProcessManager(LauncherSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Attach a logger to capture server output
        /// </summary>
        public void AttachLogger(ServerLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Check if a server is running on the specified port
        /// </summary>
        public bool IsServerRunning(int port)
        {
            try
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect("127.0.0.1", port);
                    socket.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Start the Node.js server
        /// </summary>
        public void StartServer()
        {
            if (!Directory.Exists(_settings.AppPath))
            {
                throw new DirectoryNotFoundException($"App directory not found: {_settings.AppPath}");
            }

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                throw new InvalidOperationException("Server is already running");
            }

            try
            {
                _serverProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c npm start",
                        WorkingDirectory = _settings.AppPath,
                        UseShellExecute = false,
                        RedirectStandardOutput = _logger != null,
                        RedirectStandardError = _logger != null,
                        CreateNoWindow = true
                    }
                };

                if (!_serverProcess.Start())
                {
                    throw new Exception("Failed to start server process");
                }

                // Capture output if logger is attached
                if (_logger != null)
                {
                    CaptureProcessOutput(_serverProcess);
                }
            }
            catch (Exception ex)
            {
                _serverProcess?.Dispose();
                _serverProcess = null;
                throw new Exception($"Failed to start server: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Stop the Node.js server
        /// </summary>
        public void StopServer()
        {
            try
            {
                if (_serverProcess != null && !_serverProcess.HasExited)
                {
                    _serverProcess.Kill(entireProcessTree: true);
                    _serverProcess.WaitForExit(2000);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error stopping server: {ex.Message}");
                throw new Exception($"Failed to stop server: {ex.Message}", ex);
            }
            finally
            {
                _serverProcess?.Dispose();
                _serverProcess = null;
            }
        }

        /// <summary>
        /// Asynchronously wait for server to start (port becomes available)
        /// </summary>
        public async Task<bool> WaitForServerAsync(int maxWaitMs = 10000)
        {
            int elapsedMs = 0;
            const int checkIntervalMs = 500;

            while (elapsedMs < maxWaitMs)
            {
                if (IsServerRunning(_settings.Port))
                {
                    return true;
                }

                await Task.Delay(checkIntervalMs);
                elapsedMs += checkIntervalMs;
            }

            return false;
        }

        /// <summary>
        /// Check if server is still running and clean up if it crashed
        /// </summary>
        public bool IsServerStillRunning()
        {
            // First check the process object
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                return true;
            }

            // Fall back to port check
            return IsServerRunning(_settings.Port);
        }

        /// <summary>
        /// Capture process output (stdout/stderr)
        /// </summary>
        private void CaptureProcessOutput(Process process)
        {
            if (_logger == null) return;

            // Capture standard output
            Task.Run(() =>
            {
                try
                {
                    string? line;
                    while ((line = process.StandardOutput.ReadLine()) != null)
                    {
                        _logger.Log(line, ServerLogger.LogLevel.Info);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log($"Error reading stdout: {ex.Message}", ServerLogger.LogLevel.Error);
                }
            });

            // Capture standard error
            Task.Run(() =>
            {
                try
                {
                    string? line;
                    while ((line = process.StandardError.ReadLine()) != null)
                    {
                        _logger.Log(line, ServerLogger.LogLevel.Error);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log($"Error reading stderr: {ex.Message}", ServerLogger.LogLevel.Error);
                }
            });
        }
    }
}
