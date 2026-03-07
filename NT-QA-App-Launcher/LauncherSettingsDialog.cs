using System;
using System.Windows.Forms;

namespace NTQAAppLauncher
{
    /// <summary>
    /// Settings dialog for configuring launcher preferences
    /// </summary>
    public class LauncherSettingsDialog : Form
    {
        private readonly LauncherSettings _settings;
        private TextBox? _appPathTextBox;
        private NumericUpDown? _portNumericUpDown;
        private CheckBox? _autoStartCheckBox;
        private Button? _browseButton;
        private Button? _okButton;
        private Button? _cancelButton;

        public LauncherSettingsDialog(LauncherSettings settings)
        {
            _settings = new LauncherSettings
            {
                AppPath = settings.AppPath,
                Port = settings.Port,
                AutoStartServer = settings.AutoStartServer
            };
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Launcher Settings";
            this.Width = 500;
            this.Height = 250;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;

            const int padding = 10;
            const int labelWidth = 100;
            const int controlWidth = 350;
            int yPos = padding;

            // App Path Label
            Label appPathLabel = new Label
            {
                Text = "App Path:",
                Location = new System.Drawing.Point(padding, yPos),
                Size = new System.Drawing.Size(labelWidth, 20),
                AutoSize = false
            };
            this.Controls.Add(appPathLabel);

            // App Path TextBox
            _appPathTextBox = new TextBox
            {
                Text = _settings.AppPath,
                Location = new System.Drawing.Point(labelWidth + padding * 2, yPos),
                Size = new System.Drawing.Size(controlWidth - 40, 20),
                ReadOnly = true
            };
            this.Controls.Add(_appPathTextBox);

            // Browse Button
            _browseButton = new Button
            {
                Text = "...",
                Location = new System.Drawing.Point(labelWidth + controlWidth + padding, yPos),
                Size = new System.Drawing.Size(30, 20)
            };
            _browseButton.Click += OnBrowseClick;
            this.Controls.Add(_browseButton);

            yPos += 30;

            // Port Label
            Label portLabel = new Label
            {
                Text = "Port:",
                Location = new System.Drawing.Point(padding, yPos),
                Size = new System.Drawing.Size(labelWidth, 20),
                AutoSize = false
            };
            this.Controls.Add(portLabel);

            // Port NumericUpDown
            _portNumericUpDown = new NumericUpDown
            {
                Value = _settings.Port,
                Minimum = 1024,
                Maximum = 65535,
                Location = new System.Drawing.Point(labelWidth + padding * 2, yPos),
                Size = new System.Drawing.Size(100, 20)
            };
            this.Controls.Add(_portNumericUpDown);

            yPos += 30;

            // Auto-Start Checkbox
            _autoStartCheckBox = new CheckBox
            {
                Text = "Auto-start server on launcher open",
                Checked = _settings.AutoStartServer,
                Location = new System.Drawing.Point(padding, yPos),
                Size = new System.Drawing.Size(350, 20)
            };
            this.Controls.Add(_autoStartCheckBox);

            yPos += 40;

            // OK Button
            _okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(280, yPos),
                Size = new System.Drawing.Size(100, 30)
            };
            _okButton.Click += OnOkClick;
            this.Controls.Add(_okButton);

            // Cancel Button
            _cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(390, yPos),
                Size = new System.Drawing.Size(100, 30)
            };
            this.Controls.Add(_cancelButton);

            this.AcceptButton = _okButton;
            this.CancelButton = _cancelButton;
        }

        private void OnBrowseClick(object? sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select NT-QA-App directory";
                dialog.SelectedPath = _settings.AppPath;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _settings.AppPath = dialog.SelectedPath;
                    if (_appPathTextBox != null)
                    {
                        _appPathTextBox.Text = dialog.SelectedPath;
                    }
                }
            }
        }

        private void OnOkClick(object? sender, EventArgs e)
        {
            if (_portNumericUpDown != null)
            {
                _settings.Port = (int)_portNumericUpDown.Value;
            }

            if (_autoStartCheckBox != null)
            {
                _settings.AutoStartServer = _autoStartCheckBox.Checked;
            }

            _settings.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public LauncherSettings GetSettings() => _settings;
    }
}
