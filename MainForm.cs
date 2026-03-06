using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectSpecGUI.Core;
using ProjectSpecGUI.Integration;
using ProjectSpecGUI.UI;

namespace ProjectSpecGUI
{
    /// <summary>
    /// Main application window with hybrid UI layout
    /// Split into 3 panels: Wizard (left), Config Tabs (right), Preview (bottom)
    /// </summary>
    public partial class MainForm : Form
    {
        private ProjectConfiguration projectConfig;
        private WizardPanel wizardPanel;
        private ConfigurationTabs configTabs;
        private PreviewPanel previewPanel;
        private StatusStrip statusBar;
        private ToolStripStatusLabel statusLabel;
        private ToolStripStatusLabel validationLabel;
        private ToolStripStatusLabel progressLabel;

        public MainForm()
        {
            InitializeComponent();
            InitializeUI();
            LoadDefaultConfiguration();
        }

        private void InitializeComponent()
        {
            // Main form properties
            this.Text = "ProjectSpec GUI - Windows 11 Project Configuration Tool";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Icon = SystemIcons.Application;
            this.Font = new Font("Segoe UI", 9F);

            // Enable dark mode support
            this.ForeColor = SystemColors.ControlText;
            this.BackColor = SystemColors.Control;

            // Create menu bar
            CreateMenuBar();

            // Create status bar
            CreateStatusBar();
        }

        private void InitializeUI()
        {
            // Initialize data model
            projectConfig = new ProjectConfiguration();

            // Create split container for wizard and tabs
            SplitContainer mainSplitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 300, // 25% for wizard
                SplitterWidth = 2
            };

            // Left Panel: Wizard
            wizardPanel = new WizardPanel(projectConfig);
            wizardPanel.Dock = DockStyle.Fill;
            mainSplitContainer.Panel1.Controls.Add(wizardPanel);
            mainSplitContainer.Panel1.BackColor = SystemColors.Control;

            // Right Panel: Config Tabs
            configTabs = new ConfigurationTabs(projectConfig);
            configTabs.Dock = DockStyle.Fill;
            mainSplitContainer.Panel2.Controls.Add(configTabs);
            mainSplitContainer.Panel2.BackColor = SystemColors.Control;

            // Create vertical splitter for preview
            SplitContainer previewSplitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 600, // 75% for main content
                SplitterWidth = 2
            };

            previewSplitContainer.Panel1.Controls.Add(mainSplitContainer);
            previewSplitContainer.Panel1.BackColor = SystemColors.Control;

            // Bottom Panel: Preview
            previewPanel = new PreviewPanel(projectConfig);
            previewPanel.Dock = DockStyle.Fill;
            previewSplitContainer.Panel2.Controls.Add(previewPanel);
            previewSplitContainer.Panel2.BackColor = SystemColors.Control;

            // Add to form
            this.Controls.Add(previewSplitContainer);

            // Hook up events to update preview in real-time
            wizardPanel.ConfigurationChanged += OnConfigurationChanged;
            configTabs.ConfigurationChanged += OnConfigurationChanged;
        }

        private void CreateMenuBar()
        {
            MenuStrip menuStrip = new MenuStrip();

            // File menu
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("&File");
            fileMenu.DropDownItems.Add("&New Configuration", null, OnNewConfiguration);
            fileMenu.DropDownItems.Add("&Open Configuration", null, OnOpenConfiguration);
            fileMenu.DropDownItems.Add("&Save Configuration", null, OnSaveConfiguration);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add("E&xit", null, OnExit);
            menuStrip.Items.Add(fileMenu);

            // Edit menu
            ToolStripMenuItem editMenu = new ToolStripMenuItem("&Edit");
            editMenu.DropDownItems.Add("&Validate Configuration", null, OnValidateConfiguration);
            editMenu.DropDownItems.Add("&Reset to Defaults", null, OnResetDefaults);
            menuStrip.Items.Add(editMenu);

            // Templates menu
            ToolStripMenuItem templatesMenu = new ToolStripMenuItem("&Templates");
            templatesMenu.DropDownItems.Add("&Blog CMS", null, OnLoadTemplate);
            templatesMenu.DropDownItems.Add("&E-commerce Store", null, OnLoadTemplate);
            templatesMenu.DropDownItems.Add("&API Backend", null, OnLoadTemplate);
            templatesMenu.DropDownItems.Add("&Desktop App", null, OnLoadTemplate);
            menuStrip.Items.Add(templatesMenu);

            // Claude menu
            ToolStripMenuItem claudeMenu = new ToolStripMenuItem("&Claude");
            claudeMenu.DropDownItems.Add("&Send to Claude", null, OnSendToClaude);
            claudeMenu.DropDownItems.Add("&View History", null, OnViewHistory);
            claudeMenu.DropDownItems.Add(new ToolStripSeparator());
            claudeMenu.DropDownItems.Add("&Settings", null, OnClaudeSettings);
            menuStrip.Items.Add(claudeMenu);

            // Tools menu
            ToolStripMenuItem toolsMenu = new ToolStripMenuItem("&Tools");
            toolsMenu.DropDownItems.Add("&Preview As JSON", null, OnPreviewJSON);
            toolsMenu.DropDownItems.Add("&Copy Prompt to Clipboard", null, OnCopyPrompt);
            menuStrip.Items.Add(toolsMenu);

            // Help menu
            ToolStripMenuItem helpMenu = new ToolStripMenuItem("&Help");
            helpMenu.DropDownItems.Add("&About", null, OnAbout);
            helpMenu.DropDownItems.Add("&Documentation", null, OnDocumentation);
            menuStrip.Items.Add(helpMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void CreateStatusBar()
        {
            statusBar = new StatusStrip();

            statusLabel = new ToolStripStatusLabel("Ready");
            statusBar.Items.Add(statusLabel);

            statusBar.Items.Add(new ToolStripSeparator());

            validationLabel = new ToolStripStatusLabel("✓ Valid");
            validationLabel.ForeColor = Color.Green;
            statusBar.Items.Add(validationLabel);

            statusBar.Items.Add(new ToolStripSeparator());

            progressLabel = new ToolStripStatusLabel("Step 1 of 6");
            statusBar.Items.Add(progressLabel);

            this.Controls.Add(statusBar);
        }

        private void LoadDefaultConfiguration()
        {
            projectConfig.ProjectName = "My New Project";
            projectConfig.ProjectType = AppConstants.PROJECT_TYPES[0];
            projectConfig.PrimaryLanguage = AppConstants.LANGUAGES[0];
            OnConfigurationChanged(null, EventArgs.Empty);
        }

        // Event handler for configuration changes
        private void OnConfigurationChanged(object? sender, EventArgs e)
        {
            // Update preview panel in real-time
            if (previewPanel != null)
            {
                previewPanel.UpdatePreview();
            }

            // Update status bar with comprehensive validation
            var validator = new ConfigurationValidator();
            var validationResult = validator.Validate(projectConfig);

            if (validationResult.IsValid)
            {
                validationLabel.Text = "✓ Valid";
                validationLabel.ForeColor = Color.Green;
                if (validationResult.Warnings.Count > 0)
                {
                    statusLabel.Text = $"Configuration is valid ({validationResult.Warnings.Count} warnings)";
                }
                else
                {
                    statusLabel.Text = "Configuration is valid";
                }
            }
            else
            {
                int issueCount = validationResult.Errors.Count + validationResult.Warnings.Count;
                validationLabel.Text = $"⚠ {issueCount} issues";
                validationLabel.ForeColor = Color.Orange;
                statusLabel.Text = $"Configuration has {validationResult.Errors.Count} error(s), {validationResult.Warnings.Count} warning(s)";
            }
        }

        // Menu event handlers
        private void OnNewConfiguration(object sender, EventArgs e)
        {
            if (MessageBox.Show("Create a new configuration? Any unsaved changes will be lost.",
                "New Configuration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                projectConfig = new ProjectConfiguration();
                wizardPanel.SetConfiguration(projectConfig);
                configTabs.SetConfiguration(projectConfig);
                LoadDefaultConfiguration();
            }
        }

        private void OnOpenConfiguration(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                dialog.Title = "Open Project Configuration";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    statusLabel.Text = "Opening configuration...";
                    try
                    {
                        string json = System.IO.File.ReadAllText(dialog.FileName);
                        var serializer = new ConfigurationSerializer();
                        projectConfig = serializer.Deserialize(json);

                        // Update all UI components with loaded configuration
                        wizardPanel.SetConfiguration(projectConfig);
                        configTabs.SetConfiguration(projectConfig);
                        previewPanel.SetConfiguration(projectConfig);

                        statusLabel.Text = $"✓ Configuration loaded from {System.IO.Path.GetFileName(dialog.FileName)}";
                        statusLabel.ForeColor = System.Drawing.Color.Green;
                        MessageBox.Show("Configuration loaded successfully!", "Success");
                    }
                    catch (Exception ex)
                    {
                        statusLabel.Text = "Error loading configuration";
                        statusLabel.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show($"Failed to load configuration: {ex.Message}", "Error");
                    }
                }
            }
        }

        private void OnSaveConfiguration(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                dialog.Title = "Save Project Configuration";
                dialog.FileName = $"{projectConfig.ProjectName.Replace(" ", "_")}_config.json";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    statusLabel.Text = "Saving configuration...";
                    try
                    {
                        var serializer = new ConfigurationSerializer();
                        string json = serializer.Serialize(projectConfig);
                        System.IO.File.WriteAllText(dialog.FileName, json);

                        statusLabel.Text = $"✓ Configuration saved to {System.IO.Path.GetFileName(dialog.FileName)}";
                        statusLabel.ForeColor = System.Drawing.Color.Green;
                        MessageBox.Show("Configuration saved successfully!", "Success");
                    }
                    catch (Exception ex)
                    {
                        statusLabel.Text = "Error saving configuration";
                        statusLabel.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show($"Failed to save configuration: {ex.Message}", "Error");
                    }
                }
            }
        }

        private void OnValidateConfiguration(object sender, EventArgs e)
        {
            var validator = new ConfigurationValidator();
            var result = validator.Validate(projectConfig);

            if (result.IsValid && result.Warnings.Count == 0)
            {
                MessageBox.Show("Configuration is valid! ✓", "Validation Result",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string message = "";
                if (result.Errors.Count > 0)
                {
                    message += "Errors:\n" + string.Join("\n", result.Errors.Select(e => "  • " + e));
                }
                if (result.Warnings.Count > 0)
                {
                    if (message.Length > 0) message += "\n\n";
                    message += "Warnings:\n" + string.Join("\n", result.Warnings.Select(w => "  • " + w));
                }

                MessageBoxIcon icon = result.IsValid ? MessageBoxIcon.Warning : MessageBoxIcon.Error;
                MessageBox.Show(message, "Validation Result",
                    MessageBoxButtons.OK, icon);
            }
        }

        private void OnResetDefaults(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reset configuration to defaults?", "Reset Defaults",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                projectConfig = new ProjectConfiguration();
                wizardPanel.SetConfiguration(projectConfig);
                configTabs.SetConfiguration(projectConfig);
                LoadDefaultConfiguration();
            }
        }

        private void OnLoadTemplate(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string templateName = item?.Text.Replace("&", "").Trim();

            statusLabel.Text = $"Loading template: {templateName}";
            MessageBox.Show($"Loading template '{templateName}' not yet implemented.", "Info");
            // TODO: Implement template loading
        }

        private void OnClaudeSettings(object sender, EventArgs e)
        {
            using (SettingsDialog dialog = new SettingsDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    statusLabel.Text = "✓ Settings saved";
                    statusLabel.ForeColor = Color.Green;
                }
            }
        }

        private async void OnSendToClaude(object sender, EventArgs e)
        {
            // Load settings
            var settings = ClaudeSettings.Load();

            // Check if API key is configured
            if (!settings.IsConfigured())
            {
                MessageBox.Show("Please configure your Claude API key in Claude → Settings.", "API Key Not Configured", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                OnClaudeSettings(null, null);
                return;
            }

            // Show progress dialog
            var progressForm = new Form
            {
                Text = "Sending to Claude...",
                Width = 300,
                Height = 120,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ControlBox = false,
                TopMost = true
            };

            Label progressLabel = new Label
            {
                Text = "Sending configuration to Claude...",
                AutoSize = true,
                Top = 20,
                Left = 20
            };
            progressForm.Controls.Add(progressLabel);

            progressForm.Show(this);
            this.Cursor = Cursors.WaitCursor;
            statusLabel.Text = "Sending to Claude...";

            try
            {
                // Send configuration to Claude
                var apiClient = new ClaudeAPIClient(settings);
                var response = await apiClient.SendConfigurationAsync(projectConfig);

                progressForm.Close();
                this.Cursor = Cursors.Default;

                if (response.Success)
                {
                    statusLabel.Text = $"✓ Sent to Claude ({response.InputTokens + response.OutputTokens} tokens)";
                    statusLabel.ForeColor = Color.Green;

                    // Display response
                    using (ResponseWindow responseWindow = new ResponseWindow(response))
                    {
                        responseWindow.ShowDialog(this);
                    }

                    // Save to history if enabled
                    if (settings.SaveHistory)
                    {
                        var history = ApiHistory.Load();
                        history.AddEntry(response.ToHistoryEntry(projectConfig));
                    }
                }
                else
                {
                    statusLabel.Text = $"✗ Error: {response.Message}";
                    statusLabel.ForeColor = Color.Red;
                    MessageBox.Show($"Failed to send to Claude:\n\n{response.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                progressForm.Close();
                this.Cursor = Cursors.Default;
                statusLabel.Text = "Error sending to Claude";
                statusLabel.ForeColor = Color.Red;
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnViewHistory(object sender, EventArgs e)
        {
            var history = ApiHistory.Load();
            if (history.Count == 0)
            {
                MessageBox.Show("No history available. Send a configuration to Claude first.", "No History", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string historyText = "Recent API Calls:\n\n";
            foreach (var entry in history.GetHistory().Take(10))
            {
                historyText += $"• {entry.ProjectName}\n  {entry.SentAt:yyyy-MM-dd HH:mm} - {(entry.Success ? "Success" : "Failed")}\n";
            }

            MessageBox.Show(historyText, "API History", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnPreviewJSON(object sender, EventArgs e)
        {
            previewPanel?.ShowJsonPreview();
        }

        private void OnCopyPrompt(object sender, EventArgs e)
        {
            try
            {
                string prompt = projectConfig.GenerateClaudePrompt();
                Clipboard.SetText(prompt);
                statusLabel.Text = "Claude prompt copied to clipboard!";
                MessageBox.Show("Claude prompt copied to clipboard!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying to clipboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnAbout(object sender, EventArgs e)
        {
            MessageBox.Show(
                "ProjectSpec GUI v1.0\n\n" +
                "Windows 11 Project Configuration Tool\n" +
                "Configure projects visually and send to Claude for code generation\n\n" +
                "© 2026 Claude AI\n" +
                "Built with C# WinForms (.NET 6+)",
                "About ProjectSpec GUI",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void OnDocumentation(object sender, EventArgs e)
        {
            MessageBox.Show("Opening documentation...\n\nSee IMPLEMENTATION_GUIDE.md in project folder",
                "Documentation");
            // TODO: Open documentation file or URL
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (MessageBox.Show("Close ProjectSpec GUI?", "Confirm Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }
    }
}
