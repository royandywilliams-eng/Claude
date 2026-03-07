using System;
using System.Drawing;
using System.Windows.Forms;

namespace NTQAAppLauncher
{
    /// <summary>
    /// Panel for displaying server logs with filtering and search
    /// </summary>
    public class LogViewerPanel : Panel
    {
        private RichTextBox? _logTextBox;
        private Panel? _controlPanel;
        private Button? _clearButton;
        private Button? _exportButton;
        private Button? _collapseButton;
        private ComboBox? _filterCombo;
        private Label? _countLabel;

        private ServerLogger? _logger;
        private bool _isCollapsed = false;

        public LogViewerPanel()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Height = 200;
            this.Dock = DockStyle.Bottom;
            this.BorderStyle = BorderStyle.FixedSingle;
            ThemeManager.StylePanel(this);

            // Control Panel
            _controlPanel = new Panel
            {
                Height = 40,
                Dock = DockStyle.Top,
                BackColor = ThemeManager.GetSurfaceColor()
            };

            // Collapse Button
            _collapseButton = new Button
            {
                Text = "▲ Hide Logs",
                Width = 100,
                Height = 32,
                Location = new Point(10, 4),
                Font = ThemeManager.Fonts.SmallFont
            };
            _collapseButton.Click += OnCollapse;
            ThemeManager.StyleButton(_collapseButton, false);
            _controlPanel.Controls.Add(_collapseButton);

            // Filter Combo
            _filterCombo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 120,
                Height = 32,
                Location = new Point(120, 4),
                Font = ThemeManager.Fonts.SmallFont
            };
            _filterCombo.Items.AddRange(new[] { "All", "Info", "Warning", "Error", "Debug" });
            _filterCombo.SelectedIndex = 0;
            _filterCombo.SelectedIndexChanged += OnFilterChanged;
            _controlPanel.Controls.Add(_filterCombo);

            // Count Label
            _countLabel = new Label
            {
                Text = "Logs: 0",
                Width = 80,
                Height = 32,
                Location = new Point(250, 4),
                Font = ThemeManager.Fonts.SmallFont,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = ThemeManager.GetForegroundColor()
            };
            _controlPanel.Controls.Add(_countLabel);

            // Export Button
            _exportButton = new Button
            {
                Text = "Export",
                Width = 80,
                Height = 32,
                Location = new Point(340, 4),
                Font = ThemeManager.Fonts.SmallFont
            };
            _exportButton.Click += OnExport;
            ThemeManager.StyleButton(_exportButton, false);
            _controlPanel.Controls.Add(_exportButton);

            // Clear Button
            _clearButton = new Button
            {
                Text = "Clear",
                Width = 80,
                Height = 32,
                Location = new Point(430, 4),
                Font = ThemeManager.Fonts.SmallFont
            };
            _clearButton.Click += OnClear;
            ThemeManager.StyleButton(_clearButton, false);
            _controlPanel.Controls.Add(_clearButton);

            this.Controls.Add(_controlPanel);

            // Log TextBox
            _logTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = ThemeManager.Fonts.MonospaceFont,
                ReadOnly = true,
                BackColor = ThemeManager.GetBackgroundColor(),
                ForeColor = ThemeManager.GetForegroundColor(),
                BorderStyle = BorderStyle.None
            };

            this.Controls.Add(_logTextBox);
        }

        /// <summary>
        /// Attach a logger to this viewer
        /// </summary>
        public void AttachLogger(ServerLogger logger)
        {
            _logger = logger;
            _logger.LogAdded += OnLogAdded;

            // Display existing logs
            RefreshLogs();
        }

        private void OnLogAdded(object? sender, ServerLogger.LogEntry entry)
        {
            if (_logTextBox == null) return;

            // Format log entry
            string prefix = GetColorPrefix(entry.Level);
            string logLine = $"{prefix}{entry}\n";

            _logTextBox.Invoke(() =>
            {
                _logTextBox.AppendText(logLine);

                // Auto-scroll to bottom
                _logTextBox.SelectionStart = _logTextBox.Text.Length;
                _logTextBox.ScrollToCaret();

                // Update count
                UpdateLogCount();

                // Limit text size to prevent memory issues
                if (_logTextBox.Text.Length > 500000) // 500KB limit
                {
                    _logTextBox.Text = _logTextBox.Text.Substring(
                        _logTextBox.Text.Length - 250000);
                }
            });
        }

        private string GetColorPrefix(ServerLogger.LogLevel level)
        {
            return level switch
            {
                ServerLogger.LogLevel.Error => "[ERR]  ",
                ServerLogger.LogLevel.Warning => "[WRN]  ",
                ServerLogger.LogLevel.Info => "[INF]  ",
                ServerLogger.LogLevel.Debug => "[DBG]  ",
                _ => "[???]  "
            };
        }

        private void OnFilterChanged(object? sender, EventArgs e)
        {
            RefreshLogs();
        }

        private void RefreshLogs()
        {
            if (_logTextBox == null || _logger == null) return;

            _logTextBox.Clear();

            string filter = _filterCombo?.SelectedItem?.ToString() ?? "All";
            var logs = filter switch
            {
                "Error" => _logger.GetLogs(ServerLogger.LogLevel.Error),
                "Warning" => _logger.GetLogs(ServerLogger.LogLevel.Warning),
                "Info" => _logger.GetLogs(ServerLogger.LogLevel.Info),
                "Debug" => _logger.GetLogs(ServerLogger.LogLevel.Debug),
                _ => _logger.GetLogs()
            };

            foreach (var entry in logs)
            {
                string prefix = GetColorPrefix(entry.Level);
                _logTextBox.AppendText($"{prefix}{entry}\n");
            }

            UpdateLogCount();
        }

        private void UpdateLogCount()
        {
            if (_countLabel != null && _logger != null)
            {
                _countLabel.Text = $"Logs: {_logger.GetLogs().Count}";
            }
        }

        private void OnClear(object? sender, EventArgs e)
        {
            _logger?.Clear();
            if (_logTextBox != null)
                _logTextBox.Clear();
        }

        private void OnExport(object? sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                dialog.FileName = $"launcher-logs-{DateTime.Now:yyyy-MM-dd-HHmmss}.txt";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _logger?.ExportLogs(dialog.FileName);
                    MessageBox.Show("Logs exported successfully!", "Export",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OnCollapse(object? sender, EventArgs e)
        {
            _isCollapsed = !_isCollapsed;

            if (_isCollapsed)
            {
                this.Height = 40;
                if (_collapseButton != null)
                    _collapseButton.Text = "▼ Show Logs";
                if (_logTextBox != null)
                    _logTextBox.Visible = false;
            }
            else
            {
                this.Height = 200;
                if (_collapseButton != null)
                    _collapseButton.Text = "▲ Hide Logs";
                if (_logTextBox != null)
                    _logTextBox.Visible = true;
            }
        }
    }
}
