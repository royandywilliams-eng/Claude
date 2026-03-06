using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UI
{
    /// <summary>
    /// Real-time specification preview panel
    /// Shows generated specification in multiple formats (Markdown, JSON, Claude Prompt)
    /// </summary>
    public class PreviewPanel : Panel
    {
        public event EventHandler ConfigurationChanged;

        private ProjectConfiguration configuration;
        private TabControl viewModeTabControl;
        private RichTextBox previewTextBox;
        private Button copyButton;
        private Button exportButton;
        private Label statusLabel;

        public PreviewPanel(ProjectConfiguration config)
        {
            this.configuration = config;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.BackColor = SystemColors.Control;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Padding = new Padding(10);
            this.Dock = DockStyle.Fill;

            // Header with title
            Label headerLabel = new Label
            {
                Text = "📋 Real-time Specification Preview",
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(10, 10)
            };
            this.Controls.Add(headerLabel);

            // Tab control for different view modes
            viewModeTabControl = new TabControl
            {
                Location = new Point(10, 35),
                Size = new Size(this.Width - 30, this.Height - 100),
                Dock = DockStyle.Top,
                Height = this.Height - 100
            };

            // Markdown tab
            TabPage markdownTab = new TabPage
            {
                Text = "📝 Markdown Preview",
                Padding = new Padding(5)
            };

            RichTextBox markdownBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            markdownTab.Controls.Add(markdownBox);
            viewModeTabControl.TabPages.Add(markdownTab);

            // JSON tab
            TabPage jsonTab = new TabPage
            {
                Text = "{ } JSON",
                Padding = new Padding(5)
            };

            RichTextBox jsonBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            jsonTab.Controls.Add(jsonBox);
            viewModeTabControl.TabPages.Add(jsonTab);

            // Claude Prompt tab
            TabPage promptTab = new TabPage
            {
                Text = "🤖 Claude Prompt",
                Padding = new Padding(5)
            };

            RichTextBox promptBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            promptTab.Controls.Add(promptBox);
            viewModeTabControl.TabPages.Add(promptTab);

            this.Controls.Add(viewModeTabControl);

            // Action buttons
            Panel buttonPanel = new Panel
            {
                Location = new Point(10, this.Height - 55),
                Size = new Size(this.Width - 30, 45),
                Dock = DockStyle.Bottom,
                BackColor = SystemColors.Control
            };

            copyButton = new Button
            {
                Text = "📋 Copy to Clipboard",
                Location = new Point(0, 10),
                Size = new Size(140, 30),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat
            };
            copyButton.Click += CopyButton_Click;
            buttonPanel.Controls.Add(copyButton);

            exportButton = new Button
            {
                Text = "💾 Export to File",
                Location = new Point(150, 10),
                Size = new Size(120, 30),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            exportButton.Click += ExportButton_Click;
            buttonPanel.Controls.Add(exportButton);

            // Status label
            statusLabel = new Label
            {
                Text = "Ready",
                AutoSize = true,
                ForeColor = SystemColors.GrayText,
                Location = new Point(280, 15),
                Font = new Font("Segoe UI", 9F)
            };
            buttonPanel.Controls.Add(statusLabel);

            this.Controls.Add(buttonPanel);

            // Set store references to update content dynamically
            this.Tag = new TextBoxReferences
            {
                MarkdownBox = markdownBox,
                JsonBox = jsonBox,
                PromptBox = promptBox
            };

            UpdatePreview();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            try
            {
                TabPage currentTab = viewModeTabControl.SelectedTab;
                RichTextBox currentBox = (RichTextBox)currentTab.Controls[0];
                if (!string.IsNullOrWhiteSpace(currentBox.Text))
                {
                    Clipboard.SetText(currentBox.Text);
                    statusLabel.Text = "✓ Copied to clipboard!";
                    statusLabel.ForeColor = Color.Green;
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Interval = 3000;
                    timer.Tick += (s, args) =>
                    {
                        statusLabel.Text = "Ready";
                        statusLabel.ForeColor = SystemColors.GrayText;
                        timer.Stop();
                    };
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error copying: " + ex.Message;
                statusLabel.ForeColor = Color.Red;
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|Markdown Files (*.md)|*.md|JSON Files (*.json)|*.json",
                    FileName = $"project-spec-{DateTime.Now:yyyy-MM-dd}.txt",
                    DefaultExt = ".txt"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    TabPage currentTab = viewModeTabControl.SelectedTab;
                    RichTextBox currentBox = (RichTextBox)currentTab.Controls[0];
                    System.IO.File.WriteAllText(saveDialog.FileName, currentBox.Text);
                    statusLabel.Text = $"✓ Exported to {System.IO.Path.GetFileName(saveDialog.FileName)}";
                    statusLabel.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Error exporting: " + ex.Message;
                statusLabel.ForeColor = Color.Red;
            }
        }

        public void UpdatePreview()
        {
            if (this.Tag is TextBoxReferences refs)
            {
                try
                {
                    // Generate and display markdown specification
                    string markdownSpec = configuration.GenerateMarkdownSpecification();
                    refs.MarkdownBox.Text = markdownSpec;

                    // Generate and display JSON (structured format)
                    string jsonSpec = GenerateJsonSpecification();
                    refs.JsonBox.Text = jsonSpec;

                    // Generate and display Claude prompt
                    string claudePrompt = configuration.GenerateClaudePrompt();
                    refs.PromptBox.Text = claudePrompt;

                    statusLabel.Text = $"✓ Preview updated ({markdownSpec.Length} chars)";
                    statusLabel.ForeColor = SystemColors.GrayText;
                }
                catch (Exception ex)
                {
                    statusLabel.Text = "Error updating preview: " + ex.Message;
                    statusLabel.ForeColor = Color.Red;
                }
            }
        }

        private string GenerateJsonSpecification()
        {
            // Use the ConfigurationSerializer for consistent JSON generation
            var serializer = new ConfigurationSerializer();
            return serializer.Serialize(configuration);
        }

        public void SetConfiguration(ProjectConfiguration config)
        {
            this.configuration = config;
            UpdatePreview();
        }

        public void ShowJsonPreview()
        {
            // Switch to JSON tab
            if (viewModeTabControl.TabPages.Count > 1)
            {
                viewModeTabControl.SelectedIndex = 1;
            }
        }

        protected void OnConfigurationChanged()
        {
            ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        // Helper class to store references to text boxes
        private class TextBoxReferences
        {
            public RichTextBox MarkdownBox { get; set; }
            public RichTextBox JsonBox { get; set; }
            public RichTextBox PromptBox { get; set; }
        }
    }
}
