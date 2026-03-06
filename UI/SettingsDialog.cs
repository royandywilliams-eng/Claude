using System;
using System.Windows.Forms;
using ProjectSpecGUI.Integration;

namespace ProjectSpecGUI.UI
{
    /// <summary>
    /// Settings dialog for Claude API configuration
    /// </summary>
    public class SettingsDialog : Form
    {
        private TextBox apiKeyTextBox;
        private ComboBox modelComboBox;
        private NumericUpDown timeoutNumeric;
        private NumericUpDown maxTokensNumeric;
        private NumericUpDown temperatureNumeric;
        private CheckBox saveHistoryCheckBox;
        private Button testButton;
        private Button okButton;
        private Button cancelButton;
        private Label statusLabel;

        private ClaudeSettings _settings;

        public SettingsDialog()
        {
            _settings = ClaudeSettings.Load();
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.Text = "Claude API Settings";
            this.Width = 500;
            this.Height = 420;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new System.Drawing.Font("Segoe UI", 10f);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            int y = 20;
            const int labelWidth = 150;
            const int controlWidth = 300;
            const int margin = 10;

            // API Key label and textbox
            Label apiKeyLabel = new Label { Text = "API Key:", Width = labelWidth, Top = y, Left = margin };
            this.Controls.Add(apiKeyLabel);

            apiKeyTextBox = new TextBox
            {
                PasswordChar = '*',
                Width = controlWidth,
                Top = y,
                Left = margin + labelWidth + 10
            };
            this.Controls.Add(apiKeyTextBox);

            y += 40;

            // Model label and combobox
            Label modelLabel = new Label { Text = "Model:", Width = labelWidth, Top = y, Left = margin };
            this.Controls.Add(modelLabel);

            modelComboBox = new ComboBox
            {
                Width = controlWidth,
                Top = y,
                Left = margin + labelWidth + 10,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (string model in ClaudeSettings.GetAvailableModels())
            {
                modelComboBox.Items.Add(model);
            }
            this.Controls.Add(modelComboBox);

            y += 40;

            // Timeout label and numeric
            Label timeoutLabel = new Label { Text = "Timeout (seconds):", Width = labelWidth, Top = y, Left = margin };
            this.Controls.Add(timeoutLabel);

            timeoutNumeric = new NumericUpDown
            {
                Width = controlWidth,
                Top = y,
                Left = margin + labelWidth + 10,
                Minimum = 10,
                Maximum = 300,
                Value = 60
            };
            this.Controls.Add(timeoutNumeric);

            y += 40;

            // Max tokens label and numeric
            Label maxTokensLabel = new Label { Text = "Max Tokens:", Width = labelWidth, Top = y, Left = margin };
            this.Controls.Add(maxTokensLabel);

            maxTokensNumeric = new NumericUpDown
            {
                Width = controlWidth,
                Top = y,
                Left = margin + labelWidth + 10,
                Minimum = 100,
                Maximum = 16000,
                Value = 4096
            };
            this.Controls.Add(maxTokensNumeric);

            y += 40;

            // Temperature label and numeric
            Label tempLabel = new Label { Text = "Temperature:", Width = labelWidth, Top = y, Left = margin };
            this.Controls.Add(tempLabel);

            temperatureNumeric = new NumericUpDown
            {
                Width = controlWidth,
                Top = y,
                Left = margin + labelWidth + 10,
                Minimum = 0,
                Maximum = 2,
                DecimalPlaces = 2,
                Increment = 0.1m,
                Value = 0.7m
            };
            this.Controls.Add(temperatureNumeric);

            y += 40;

            // Save history checkbox
            saveHistoryCheckBox = new CheckBox
            {
                Text = "Save API call history",
                Width = controlWidth,
                Top = y,
                Left = margin + labelWidth + 10,
                Checked = true
            };
            this.Controls.Add(saveHistoryCheckBox);

            y += 40;

            // Test button
            testButton = new Button
            {
                Text = "Test Connection",
                Width = 120,
                Height = 30,
                Top = y,
                Left = margin
            };
            testButton.Click += async (s, e) => await TestConnection();
            this.Controls.Add(testButton);

            // Status label
            statusLabel = new Label
            {
                AutoSize = true,
                Top = y + 5,
                Left = margin + 130,
                ForeColor = System.Drawing.SystemColors.GrayText
            };
            this.Controls.Add(statusLabel);

            y += 50;

            // OK button
            okButton = new Button
            {
                Text = "OK",
                Width = 100,
                Height = 30,
                Top = y,
                Left = margin + controlWidth + labelWidth - 220
            };
            okButton.Click += (s, e) => SaveAndClose();
            this.Controls.Add(okButton);

            // Cancel button
            cancelButton = new Button
            {
                Text = "Cancel",
                Width = 100,
                Height = 30,
                Top = y,
                Left = margin + controlWidth + labelWidth - 100
            };
            cancelButton.Click += (s, e) => this.Close();
            this.Controls.Add(cancelButton);
        }

        private void LoadSettings()
        {
            apiKeyTextBox.Text = _settings.ApiKey;
            modelComboBox.SelectedItem = _settings.SelectedModel;
            timeoutNumeric.Value = _settings.TimeoutSeconds;
            maxTokensNumeric.Value = _settings.MaxTokens;
            temperatureNumeric.Value = (decimal)_settings.Temperature;
            saveHistoryCheckBox.Checked = _settings.SaveHistory;
        }

        private async System.Threading.Tasks.Task TestConnection()
        {
            // Validate API key
            if (string.IsNullOrWhiteSpace(apiKeyTextBox.Text))
            {
                MessageBox.Show("Please enter an API key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            testButton.Enabled = false;
            statusLabel.Text = "Testing...";
            statusLabel.ForeColor = System.Drawing.SystemColors.GrayText;

            try
            {
                // Create temporary settings with entered API key
                var testSettings = new ClaudeSettings
                {
                    ApiKey = apiKeyTextBox.Text,
                    SelectedModel = modelComboBox.SelectedItem?.ToString() ?? "claude-opus-4.6",
                    TimeoutSeconds = (int)timeoutNumeric.Value,
                    MaxTokens = (int)maxTokensNumeric.Value,
                    Temperature = (double)temperatureNumeric.Value,
                    SaveHistory = saveHistoryCheckBox.Checked
                };

                var client = new ClaudeAPIClient(testSettings);
                bool isValid = await client.ValidateApiKeyAsync();

                if (isValid)
                {
                    statusLabel.Text = "✓ Valid API key";
                    statusLabel.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    statusLabel.Text = "✗ Invalid API key";
                    statusLabel.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"✗ Error: {ex.Message}";
                statusLabel.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                testButton.Enabled = true;
            }
        }

        private void SaveAndClose()
        {
            // Validate API key
            if (string.IsNullOrWhiteSpace(apiKeyTextBox.Text))
            {
                MessageBox.Show("API key cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update settings
            _settings.ApiKey = apiKeyTextBox.Text;
            _settings.SelectedModel = modelComboBox.SelectedItem?.ToString() ?? "claude-opus-4.6";
            _settings.TimeoutSeconds = (int)timeoutNumeric.Value;
            _settings.MaxTokens = (int)maxTokensNumeric.Value;
            _settings.Temperature = (double)temperatureNumeric.Value;
            _settings.SaveHistory = saveHistoryCheckBox.Checked;

            // Save settings
            _settings.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
