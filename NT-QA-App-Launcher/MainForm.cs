using System;
using System.Drawing;
using System.Windows.Forms;

namespace NTQAAppLauncher
{
    /// <summary>
    /// Main launcher window for the NT Q&A Application
    /// Manages UI and server lifecycle
    /// </summary>
    public class MainForm : Form
    {
        private LauncherSettings _settings = null!;
        private ProcessManager _processManager = null!;
        private Timer? _statusCheckTimer;

        // UI Controls
        private Panel? _mainPanel;
        private Label? _titleLabel;
        private Label? _statusIndicator;
        private Label? _statusLabel;
        private Label? _portLabel;
        private Label? _urlLabel;
        private Button? _startButton;
        private Button? _openBrowserButton;
        private Button? _stopButton;
        private Button? _settingsButton;
        private Label? _messageLabel;
        private StatusStrip? _statusBar;
        private ToolStripStatusLabel? _statusBarLabel;

        public MainForm()
        {
            _settings = LauncherSettings.Load();
            _processManager = new ProcessManager(_settings);
            InitializeComponent();
            InitializeTimer();
            CheckServerStatus();

            if (_settings.AutoStartServer)
            {
                BeginInvoke(async () => await StartServerAsync());
            }
        }

        private void InitializeComponent()
        {
            this.Text = LauncherConfig.UI.WINDOW_TITLE;
            this.Width = LauncherConfig.UI.WINDOW_WIDTH;
            this.Height = LauncherConfig.UI.WINDOW_HEIGHT;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Icon = SystemIcons.Application;

            // Restore window position if saved
            if (_settings.RememberWindowPosition && _settings.WindowX.HasValue && _settings.WindowY.HasValue)
            {
                this.Location = new Point(_settings.WindowX.Value, _settings.WindowY.Value);
            }

            // Main Panel
            _mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(LauncherConfig.UI.PADDING)
            };
            this.Controls.Add(_mainPanel);

            CreateTitleLabel();
            CreateStatusSection();
            CreateButtonSection();
            CreateMessageLabel();
            CreateStatusBar();

            this.FormClosing += MainForm_FormClosing;
        }

        private void CreateTitleLabel()
        {
            _titleLabel = new Label
            {
                Text = LauncherConfig.SERVER_NAME,
                Location = new Point(LauncherConfig.UI.PADDING, LauncherConfig.UI.PADDING),
                Font = new Font(this.Font.FontFamily, 14, FontStyle.Bold),
                AutoSize = true
            };
            _mainPanel?.Controls.Add(_titleLabel);
        }

        private void CreateStatusSection()
        {
            int yPos = 50;

            // Status Indicator
            _statusIndicator = new Label
            {
                Text = "●",
                Location = new Point(LauncherConfig.UI.PADDING, yPos),
                Size = new Size(20, LauncherConfig.UI.LABEL_HEIGHT),
                ForeColor = Color.Red,
                Font = new Font(this.Font.FontFamily, 16)
            };
            _mainPanel?.Controls.Add(_statusIndicator);

            // Status Label
            _statusLabel = new Label
            {
                Text = "Stopped",
                Location = new Point(LauncherConfig.UI.PADDING + 25, yPos),
                Size = new Size(150, LauncherConfig.UI.LABEL_HEIGHT),
                AutoSize = false
            };
            _mainPanel?.Controls.Add(_statusLabel);

            yPos += 35;

            // Port Label
            _portLabel = new Label
            {
                Text = $"Port: {_settings.Port}",
                Location = new Point(LauncherConfig.UI.PADDING, yPos),
                AutoSize = true
            };
            _mainPanel?.Controls.Add(_portLabel);

            yPos += 25;

            // URL Label
            _urlLabel = new Label
            {
                Text = $"URL: http://localhost:{_settings.Port}",
                Location = new Point(LauncherConfig.UI.PADDING, yPos),
                AutoSize = true
            };
            _mainPanel?.Controls.Add(_urlLabel);
        }

        private void CreateButtonSection()
        {
            int yPos = 140;
            int xPos = LauncherConfig.UI.PADDING;
            int buttonSpacing = 15;

            // Start Button
            _startButton = new Button
            {
                Text = "Start Server",
                Location = new Point(xPos, yPos),
                Size = new Size(LauncherConfig.UI.BUTTON_WIDTH, LauncherConfig.UI.BUTTON_HEIGHT)
            };
            _startButton.Click += async (s, e) => await StartServerAsync();
            _mainPanel?.Controls.Add(_startButton);

            // Open Browser Button
            _openBrowserButton = new Button
            {
                Text = "Open Browser",
                Location = new Point(xPos + LauncherConfig.UI.BUTTON_WIDTH + buttonSpacing, yPos),
                Size = new Size(LauncherConfig.UI.BUTTON_WIDTH, LauncherConfig.UI.BUTTON_HEIGHT),
                Enabled = false
            };
            _openBrowserButton.Click += (s, e) => OpenBrowser();
            _mainPanel?.Controls.Add(_openBrowserButton);

            // Stop Button
            _stopButton = new Button
            {
                Text = "Stop Server",
                Location = new Point(xPos + (LauncherConfig.UI.BUTTON_WIDTH + buttonSpacing) * 2, yPos),
                Size = new Size(LauncherConfig.UI.BUTTON_WIDTH, LauncherConfig.UI.BUTTON_HEIGHT),
                Enabled = false
            };
            _stopButton.Click += (s, e) => StopServer();
            _mainPanel?.Controls.Add(_stopButton);

            // Settings Button
            _settingsButton = new Button
            {
                Text = "Settings",
                Location = new Point(xPos + (LauncherConfig.UI.BUTTON_WIDTH + buttonSpacing) * 3, yPos),
                Size = new Size(LauncherConfig.UI.BUTTON_WIDTH, LauncherConfig.UI.BUTTON_HEIGHT)
            };
            _settingsButton.Click += (s, e) => OpenSettings();
            _mainPanel?.Controls.Add(_settingsButton);
        }

        private void CreateMessageLabel()
        {
            _messageLabel = new Label
            {
                Text = "",
                Location = new Point(LauncherConfig.UI.PADDING, 190),
                Size = new Size(460, 80),
                AutoSize = false,
                ForeColor = Color.DarkGreen,
                Font = new Font(this.Font, FontStyle.Regular)
            };
            _mainPanel?.Controls.Add(_messageLabel);
        }

        private void CreateStatusBar()
        {
            _statusBar = new StatusStrip();
            _statusBarLabel = new ToolStripStatusLabel("Ready");
            _statusBar.Items.Add(_statusBarLabel);
            this.Controls.Add(_statusBar);
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
                if (_statusIndicator?.ForeColor != Color.Green)
                {
                    SetRunningStatus();
                }
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
            if (_statusIndicator != null) _statusIndicator.ForeColor = Color.Green;
            if (_statusLabel != null) _statusLabel.Text = "Running";
            if (_openBrowserButton != null) _openBrowserButton.Enabled = true;
            if (_startButton != null) _startButton.Enabled = false;
            if (_stopButton != null) _stopButton.Enabled = true;
            if (_statusBarLabel != null) _statusBarLabel.Text = $"✓ Server running on port {_settings.Port}";
        }

        private void SetStoppedStatus()
        {
            if (_statusIndicator != null) _statusIndicator.ForeColor = Color.Red;
            if (_statusLabel != null) _statusLabel.Text = "Stopped";
            if (_openBrowserButton != null) _openBrowserButton.Enabled = false;
            if (_startButton != null) _startButton.Enabled = true;
            if (_stopButton != null) _stopButton.Enabled = false;
            if (_statusBarLabel != null) _statusBarLabel.Text = "Server stopped";
        }

        private async System.Threading.Tasks.Task StartServerAsync()
        {
            try
            {
                if (_startButton != null) _startButton.Enabled = false;
                if (_messageLabel != null) _messageLabel.Text = "Starting server...";
                if (_statusBarLabel != null) _statusBarLabel.Text = "Starting server...";

                _processManager.StartServer();

                // Wait for server to be ready
                bool serverReady = await _processManager.WaitForServerAsync(LauncherConfig.SERVER_START_TIMEOUT_MS);

                if (serverReady)
                {
                    SetRunningStatus();
                    if (_messageLabel != null) _messageLabel.Text = $"✓ Server started successfully\nListening on {LauncherConfig.BASE_URL.Replace("3000", _settings.Port.ToString())}";
                }
                else
                {
                    SetStoppedStatus();
                    if (_messageLabel != null) _messageLabel.Text = "⚠ Server started but port is not responding.\nTry again or check npm output.";
                    if (_statusBarLabel != null) _statusBarLabel.Text = "Server startup timeout";
                }
            }
            catch (Exception ex)
            {
                SetStoppedStatus();
                if (_messageLabel != null) _messageLabel.Text = $"✗ Failed to start server:\n{ex.Message}";
                if (_statusBarLabel != null) _statusBarLabel.Text = $"Error: {ex.Message}";
            }
        }

        private void StopServer()
        {
            try
            {
                if (_stopButton != null) _stopButton.Enabled = false;
                if (_messageLabel != null) _messageLabel.Text = "Stopping server...";
                if (_statusBarLabel != null) _statusBarLabel.Text = "Stopping server...";

                _processManager.StopServer();

                SetStoppedStatus();
                if (_messageLabel != null) _messageLabel.Text = "✓ Server stopped";
            }
            catch (Exception ex)
            {
                if (_messageLabel != null) _messageLabel.Text = $"✗ Error stopping server:\n{ex.Message}";
                if (_statusBarLabel != null) _statusBarLabel.Text = $"Error: {ex.Message}";
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
                if (_statusBarLabel != null) _statusBarLabel.Text = "Browser opened";
            }
            catch (Exception ex)
            {
                if (_messageLabel != null) _messageLabel.Text = $"✗ Failed to open browser:\n{ex.Message}";
                if (_statusBarLabel != null) _statusBarLabel.Text = $"Error: {ex.Message}";
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

                    CheckServerStatus();
                }
            }
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
        }
    }
}
