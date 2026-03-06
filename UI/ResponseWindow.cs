using System;
using System.Windows.Forms;
using ProjectSpecGUI.Integration;

namespace ProjectSpecGUI.UI
{
    /// <summary>
    /// Window for displaying Claude API response
    /// </summary>
    public class ResponseWindow : Form
    {
        private TextBox responseTextBox;
        private Label tokenLabel;
        private Label timestampLabel;
        private Button copyButton;
        private Button saveButton;
        private Button closeButton;
        private Label errorLabel;

        public ResponseWindow(ApiResponse response)
        {
            InitializeComponent();
            DisplayResponse(response);
        }

        private void InitializeComponent()
        {
            this.Text = "Claude API Response";
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Icon = null;
            this.Font = new System.Drawing.Font("Segoe UI", 10f);

            // Response text box
            responseTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                Top = 50,
                Left = 10,
                Width = 770,
                Height = 450,
                BackColor = System.Drawing.Color.White
            };
            this.Controls.Add(responseTextBox);

            // Token usage label
            tokenLabel = new Label
            {
                AutoSize = true,
                Top = 10,
                Left = 10,
                ForeColor = System.Drawing.SystemColors.GrayText
            };
            this.Controls.Add(tokenLabel);

            // Timestamp label
            timestampLabel = new Label
            {
                AutoSize = true,
                Top = 30,
                Left = 10,
                ForeColor = System.Drawing.SystemColors.GrayText
            };
            this.Controls.Add(timestampLabel);

            // Error label (hidden by default)
            errorLabel = new Label
            {
                AutoSize = true,
                Top = 50,
                Left = 10,
                ForeColor = System.Drawing.Color.Red,
                Visible = false
            };
            this.Controls.Add(errorLabel);

            // Copy button
            copyButton = new Button
            {
                Text = "Copy to Clipboard",
                Width = 120,
                Height = 30,
                Top = 510,
                Left = 10
            };
            copyButton.Click += (s, e) => CopyToClipboard();
            this.Controls.Add(copyButton);

            // Save button
            saveButton = new Button
            {
                Text = "Save to File",
                Width = 120,
                Height = 30,
                Top = 510,
                Left = 140
            };
            saveButton.Click += (s, e) => SaveToFile();
            this.Controls.Add(saveButton);

            // Close button
            closeButton = new Button
            {
                Text = "Close",
                Width = 120,
                Height = 30,
                Top = 510,
                Left = 660
            };
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);
        }

        private void DisplayResponse(ApiResponse response)
        {
            if (response.Success)
            {
                responseTextBox.Text = response.GeneratedCode;
                tokenLabel.Text = $"Tokens: {response.InputTokens} input + {response.OutputTokens} output = {response.InputTokens + response.OutputTokens} total";
                timestampLabel.Text = $"Generated: {response.Timestamp:yyyy-MM-dd HH:mm:ss}";
                responseTextBox.Visible = true;
                errorLabel.Visible = false;
            }
            else
            {
                responseTextBox.Visible = false;
                errorLabel.Visible = true;
                errorLabel.Text = $"Error ({response.ErrorCode}): {response.Message}";
                errorLabel.Top = 50;
                copyButton.Enabled = false;
                saveButton.Enabled = false;
                tokenLabel.Text = "";
                timestampLabel.Text = "";
            }
        }

        private void CopyToClipboard()
        {
            try
            {
                Clipboard.SetText(responseTextBox.Text);
                MessageBox.Show("Copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveToFile()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "All Files (*.*)|*.*|C# Files (*.cs)|*.cs|Python Files (*.py)|*.py|JavaScript Files (*.js)|*.js|Text Files (*.txt)|*.txt";
                dialog.Title = "Save Response";
                dialog.FileName = $"claude_response_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        System.IO.File.WriteAllText(dialog.FileName, responseTextBox.Text);
                        MessageBox.Show($"Saved to {System.IO.Path.GetFileName(dialog.FileName)}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
