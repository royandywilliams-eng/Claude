using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;
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

            // Tools menu
            ToolStripMenuItem toolsMenu = new ToolStripMenuItem("&Tools");
            toolsMenu.DropDownItems.Add("&Settings", null, OnSettings);
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

            // Update status bar
            var validationErrors = projectConfig.Validate();
            if (validationErrors.Count == 0)
            {
                validationLabel.Text = "✓ Valid";
                validationLabel.ForeColor = Color.Green;
                statusLabel.Text = "Configuration is valid";
            }
            else
            {
                validationLabel.Text = $"⚠ {validationErrors.Count} issues";
                validationLabel.ForeColor = Color.Orange;
                statusLabel.Text = "Configuration has validation errors";
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
                    // TODO: Implement loading from JSON file
                    MessageBox.Show("Configuration loading not yet implemented.", "Info");
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
                    // TODO: Implement saving to JSON file
                    MessageBox.Show("Configuration saving not yet implemented.", "Info");
                }
            }
        }

        private void OnValidateConfiguration(object sender, EventArgs e)
        {
            var errors = projectConfig.Validate();
            if (errors.Count == 0)
            {
                MessageBox.Show("Configuration is valid! ✓", "Validation Result",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string errorMessage = "Configuration has the following issues:\n\n" +
                    string.Join("\n", errors);
                MessageBox.Show(errorMessage, "Validation Result",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void OnSettings(object sender, EventArgs e)
        {
            MessageBox.Show("Settings dialog not yet implemented.", "Coming Soon");
            // TODO: Implement settings dialog
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
