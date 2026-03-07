using System;
using System.Drawing;
using System.Windows.Forms;

namespace NTQAAppLauncher
{
    /// <summary>
    /// Modern Windows 11 styled main launcher window
    /// </summary>
    public class MainFormModern : Form
    {
        private LauncherSettings _settings = null!;
        private ProcessManager _processManager = null!;
        private ServerLogger _logger = null!;
        private TrayIconManager _trayManager = null!;
        private Timer? _statusCheckTimer;

        // UI Controls
        private Panel? _headerPanel;
        private Label? _titleLabel;
        private Label? _statusIndicator;
        private Label? _statusLabel;
        private Panel? _infoPanel;
        private Label? _portLabel;
        private Label? _urlLabel;
        private Label? _uptimeLabel;
        private Panel? _buttonPanel;
        private Button? _startButton;
        private Button? _openBrowserButton;
        private Button? _stopButton;
        private Button? _settingsButton;
        private Panel? _messagePanel;
        private Label? _messageLabel;
        private LogViewerPanel? _logViewerPanel;
        private StatusStrip? _statusBar;
        private ToolStripStatusLabel? _statusBarLabel;

        private DateTime _serverStartTime = DateTime.MinValue;

        public MainFormModern()
        {
            _settings = LauncherSettings.Load();
            _processManager = new ProcessManager(_settings);
            _logger = new ServerLogger();
            _trayManager = new TrayIconManager(this);

            // Attach logger to process manager for output capture
            _processManager.AttachLogger(_logger);

            InitializeComponent();
            SetupEventHandlers();
            CheckServerStatus();

            if (_settings.AutoStartServer)
            {
                BeginInvoke(async () => await StartServerAsync());
            }
        }

        private void InitializeComponent()
        {
            // Form styling
            this.Text = LauncherConfig.UI.WINDOW_TITLE;
            this.Width = 700;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Icon = SystemIcons.Application;
            this.BackColor = ThemeManager.GetBackgroundColor();
            this.ForeColor = ThemeManager.GetForegroundColor();

            // Restore position if saved
            if (_settings.RememberWindowPosition && _settings.WindowX.HasValue)
            {
                this.Location = new Point(_settings.WindowX.Value, _settings.WindowY.Value);
            }

            // Main container
            var mainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0),
                BackColor = ThemeManager.GetBackgroundColor()
            };
            this.Controls.Add(mainContainer);

            CreateHeaderPanel(mainContainer);
            CreateInfoPanel(mainContainer);
            CreateButtonPanel(mainContainer);
            CreateMessagePanel(mainContainer);
            CreateLogViewerPanel(mainContainer);
            CreateStatusBar();

            this.FormClosing += MainForm_FormClosing;
        }

        private void CreateHeaderPanel(Panel parent)
        {
            _headerPanel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = ThemeManager.GetSurfaceColor(),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Title
            _titleLabel = new Label
            {
                Text = "NT Q&A Application Launcher",
                Location = new Point(20, 15),
                Font = ThemeManager.Fonts.TitleFont,
                AutoSize = true,
                ForeColor = ThemeManager.GetForegroundColor()
            };
            _headerPanel.Controls.Add(_titleLabel);

            // Status indicator (animated circle)
            _statusIndicator = new Label
            {
                Text = "●",
                Location = new Point(20, 45),
                Font = new Font("Arial", 20),
                ForeColor = Color.Red,
                Width = 30,
                Height = 30
            };
            _headerPanel.Controls.Add(_statusIndicator);

            // Status label
            _statusLabel = new Label
            {
                Text = "Stopped",
                Location = new Point(55, 45),
                Font = ThemeManager.Fonts.HeadingFont,
                AutoSize = true,
                ForeColor = ThemeManager.GetForegroundColor()
            };
            _headerPanel.Controls.Add(_statusLabel);

            parent.Controls.Add(_headerPanel);
        }

        private void CreateInfoPanel(Panel parent)
        {
            _infoPanel = new Panel
            {
                Height = 100,
                Dock = DockStyle.Top,
                BackColor = ThemeManager.GetBackgroundColor(),
                Padding = new Padding(20)
            };

            // Port
            _portLabel = new Label
            {
                Text = $"Port: {_settings.Port}",
                Location = new Point(20, 20),
                Font = ThemeManager.Fonts.NormalFont,
                AutoSize = true,
                ForeColor = ThemeManager.GetForegroundColor()
            };
            _infoPanel.Controls.Add(_portLabel);

            // URL
            _urlLabel = new Label
            {
                Text = $"URL: http://localhost:{_settings.Port}",
                Location = new Point(20, 50),
                Font = ThemeManager.Fonts.NormalFont,
                AutoSize = true,
                ForeColor = ThemeManager.Colors.Info
            };
            _infoPanel.Controls.Add(_urlLabel);

            // Uptime
            _uptimeLabel = new Label
            {
                Text = "Uptime: N/A",
                Location = new Point(20, 80),
                Font = ThemeManager.Fonts.NormalFont,
                AutoSize = true,
                ForeColor = ThemeManager.GetForegroundColor()
            };
            _infoPanel.Controls.Add(_uptimeLabel);

            parent.Controls.Add(_infoPanel);
        }

        private void CreateButtonPanel(Panel parent)
        {
            _buttonPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = ThemeManager.GetBackgroundColor(),
                Padding = new Padding(20)
            };

            // Start button
            _startButton = new Button
            {
                Text = "▶ Start Server",
                Location = new Point(20, 12),
                Width = 140,
                Height = 36,
                Font = ThemeManager.Fonts.NormalFont
            };
            _startButton.Click += async (s, e) => await StartServerAsync();
            ThemeManager.StyleButton(_startButton, true);
            _buttonPanel.Controls.Add(_startButton);

            // Open browser button
            _openBrowserButton = new Button
            {
                Text = "🌐 Open Browser",
                Location = new Point(170, 12),
                Width = 140,
                Height = 36,
                Font = ThemeManager.Fonts.NormalFont,
                Enabled = false
            };
            _openBrowserButton.Click += (s, e) => OpenBrowser();
            ThemeManager.StyleButton(_openBrowserButton, true);
            _buttonPanel.Controls.Add(_openBrowserButton);

            // Stop button
            _stopButton = new Button
            {
                Text = "⏹ Stop Server",
                Location = new Point(320, 12),
                Width = 140,
                Height = 36,
                Font = ThemeManager.Fonts.NormalFont,
                Enabled = false
            };
            _stopButton.Click += (s, e) => StopServer();
            ThemeManager.StyleButton(_stopButton, false);
            _buttonPanel.Controls.Add(_stopButton);

            // Settings button
            _settingsButton = new Button
            {
                Text = "⚙ Settings",
                Location = new Point(470, 12),
                Width = 140,
                Height = 36,
                Font = ThemeManager.Fonts.NormalFont
            };
            _settingsButton.Click += (s, e) => OpenSettings();
            ThemeManager.StyleButton(_settingsButton, false);
            _buttonPanel.Controls.Add(_settingsButton);

            parent.Controls.Add(_buttonPanel);
        }

        private void CreateMessagePanel(Panel parent)
        {
            _messagePanel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = ThemeManager.GetSurfaceColor(),
                Padding = new Padding(20),
                BorderStyle = BorderStyle.FixedSingle
            };

            _messageLabel = new Label
            {
                Text = "Ready",
                Dock = DockStyle.Fill,
                Font = ThemeManager.Fonts.NormalFont,
                ForeColor = ThemeManager.Colors.Success,
                TextAlign = ContentAlignment.TopLeft
            };
            _messagePanel.Controls.Add(_messageLabel);

            parent.Controls.Add(_messagePanel);
        }

        private void CreateLogViewerPanel(Panel parent)
        {
            _logViewerPanel = new LogViewerPanel();
            _logViewerPanel.AttachLogger(_logger);
            parent.Controls.Add(_logViewerPanel);
        }

        private void CreateStatusBar()
        {
            _statusBar = new StatusStrip();
            _statusBarLabel = new ToolStripStatusLabel("Ready");
            _statusBar.Items.Add(_statusBarLabel);
            this.Controls.Add(_statusBar);
        }

        private void SetupEventHandlers()
        {
            _trayManager.ShowWindowRequested += (s, e) => ShowWindow();
            _trayManager.HideWindowRequested += (s, e) => HideWindow();
            _trayManager.ExitRequested += (s, e) => Close();

            InitializeTimer();
        }

        private void InitializeTimer()
        {
            _statusCheckTimer = new Timer();
            _statusCheckTimer.Interval = LauncherConfig.SERVER_CHECK_INTERVAL_MS;
            _statusCheckTimer.Tick += (s, e) => CheckServerStatus();
            _statusCheckTimer.Start();
        }

        private void CheckServerStatus()
        {
            bool isRunning = _processManager.IsServerStillRunning();

            if (isRunning)
            {
                if (_statusIndicator?.ForeColor != Color.LimeGreen)
                {
                    SetRunningStatus();
                }
                UpdateUptime();
            }
            else
            {
                if (_statusIndicator?.ForeColor != Color.Red)
                {
                    SetStoppedStatus();
                }
            }
        }

        private void SetRunningStatus()
        {
            if (_statusIndicator != null) _statusIndicator.ForeColor = Color.LimeGreen;
            if (_statusLabel != null) _statusLabel.Text = "Running";
            if (_openBrowserButton != null) _openBrowserButton.Enabled = true;
            if (_startButton != null) _startButton.Enabled = false;
            if (_stopButton != null) _stopButton.Enabled = true;
            if (_statusBarLabel != null) _statusBarLabel.Text = $"✓ Server running on port {_settings.Port}";
            _trayManager.UpdateStatus(true);
            _logger.Log($"Server is running on port {_settings.Port}", ServerLogger.LogLevel.Info);
        }

        private void SetStoppedStatus()
        {
            if (_statusIndicator != null) _statusIndicator.ForeColor = Color.Red;
            if (_statusLabel != null) _statusLabel.Text = "Stopped";
            if (_openBrowserButton != null) _openBrowserButton.Enabled = false;
            if (_startButton != null) _startButton.Enabled = true;
            if (_stopButton != null) _stopButton.Enabled = false;
            if (_statusBarLabel != null) _statusBarLabel.Text = "Server stopped";
            _trayManager.UpdateStatus(false);
            _serverStartTime = DateTime.MinValue;
        }

        private void UpdateUptime()
        {
            if (_uptimeLabel != null && _serverStartTime != DateTime.MinValue)
            {
                var uptime = DateTime.Now - _serverStartTime;
                _uptimeLabel.Text = $"Uptime: {uptime.Hours:D2}:{uptime.Minutes:D2}:{uptime.Seconds:D2}";
            }
        }

        private async System.Threading.Tasks.Task StartServerAsync()
        {
            try
            {
                if (_startButton != null) _startButton.Enabled = false;
                if (_messageLabel != null) _messageLabel.Text = "Starting server...";
                if (_messageLabel != null) _messageLabel.ForeColor = ThemeManager.Colors.Info;
                if (_statusBarLabel != null) _statusBarLabel.Text = "Starting server...";

                _logger.Log("Starting server...", ServerLogger.LogLevel.Info);
                _processManager.StartServer();
                _serverStartTime = DateTime.Now;

                // Wait for server to be ready
                bool serverReady = await _processManager.WaitForServerAsync(LauncherConfig.SERVER_START_TIMEOUT_MS);

                if (serverReady)
                {
                    SetRunningStatus();
                    if (_messageLabel != null)
                    {
                        _messageLabel.Text = $"✓ Server started successfully on http://localhost:{_settings.Port}";
                        _messageLabel.ForeColor = ThemeManager.Colors.Success;
                    }
                    _logger.Log("Server started successfully", ServerLogger.LogLevel.Info);
                    _trayManager.ShowNotification("Server Started", "NT Q&A App server is running");
                }
                else
                {
                    SetStoppedStatus();
                    if (_messageLabel != null)
                    {
                        _messageLabel.Text = "⚠ Server started but port is not responding. Check npm output.";
                        _messageLabel.ForeColor = ThemeManager.Colors.Warning;
                    }
                    _logger.Log("Server startup timeout", ServerLogger.LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                SetStoppedStatus();
                if (_messageLabel != null)
                {
                    _messageLabel.Text = $"✗ Error: {ex.Message}";
                    _messageLabel.ForeColor = ThemeManager.Colors.Error;
                }
                _logger.Log($"Failed to start server: {ex.Message}", ServerLogger.LogLevel.Error);
                _trayManager.ShowNotification("Server Error", ex.Message, ToolTipIcon.Error);
            }
        }

        private void StopServer()
        {
            try
            {
                if (_stopButton != null) _stopButton.Enabled = false;
                if (_messageLabel != null) _messageLabel.Text = "Stopping server...";
                if (_messageLabel != null) _messageLabel.ForeColor = ThemeManager.Colors.Info;
                if (_statusBarLabel != null) _statusBarLabel.Text = "Stopping server...";

                _logger.Log("Stopping server...", ServerLogger.LogLevel.Info);
                _processManager.StopServer();

                SetStoppedStatus();
                if (_messageLabel != null)
                {
                    _messageLabel.Text = "✓ Server stopped";
                    _messageLabel.ForeColor = ThemeManager.Colors.Success;
                }
                _trayManager.ShowNotification("Server Stopped", "NT Q&A App server has stopped");
            }
            catch (Exception ex)
            {
                if (_messageLabel != null)
                {
                    _messageLabel.Text = $"✗ Error stopping server: {ex.Message}";
                    _messageLabel.ForeColor = ThemeManager.Colors.Error;
                }
                _logger.Log($"Failed to stop server: {ex.Message}", ServerLogger.LogLevel.Error);
                if (_stopButton != null) _stopButton.Enabled = true;
            }
        }

        private void OpenBrowser()
        {
            try
            {
                string url = $"http://localhost:{_settings.Port}";
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
                if (_messageLabel != null) _messageLabel.Text = "✓ Browser opened";
                if (_messageLabel != null) _messageLabel.ForeColor = ThemeManager.Colors.Success;
                _logger.Log($"Browser opened: {url}", ServerLogger.LogLevel.Info);
            }
            catch (Exception ex)
            {
                if (_messageLabel != null)
                {
                    _messageLabel.Text = $"✗ Failed to open browser: {ex.Message}";
                    _messageLabel.ForeColor = ThemeManager.Colors.Error;
                }
                _logger.Log($"Failed to open browser: {ex.Message}", ServerLogger.LogLevel.Error);
            }
        }

        private void OpenSettings()
        {
            using (LauncherSettingsDialog dialog = new LauncherSettingsDialog(_settings))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _settings = dialog.GetSettings();
                    _processManager = new ProcessManager(_settings);

                    // Update display
                    if (_portLabel != null) _portLabel.Text = $"Port: {_settings.Port}";
                    if (_urlLabel != null) _urlLabel.Text = $"URL: http://localhost:{_settings.Port}";
                    if (_messageLabel != null) _messageLabel.Text = "✓ Settings saved";
                    if (_messageLabel != null) _messageLabel.ForeColor = ThemeManager.Colors.Success;

                    CheckServerStatus();
                    _logger.Log("Settings updated", ServerLogger.LogLevel.Info);
                }
            }
        }

        private void ShowWindow()
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void HideWindow()
        {
            this.Visible = false;
            this.WindowState = FormWindowState.Minimized;
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Save window position
            if (_settings.RememberWindowPosition)
            {
                _settings.WindowX = this.Location.X;
                _settings.WindowY = this.Location.Y;
                _settings.Save();
            }

            // Stop timer
            _statusCheckTimer?.Stop();
            _statusCheckTimer?.Dispose();

            // Stop server if running
            try
            {
                if (_processManager.IsServerStillRunning())
                {
                    _processManager.StopServer();
                }
            }
            catch { }

            // Cleanup
            _logger?.Dispose();
            _trayManager?.Dispose();
        }
    }
}
