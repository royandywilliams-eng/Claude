using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UITabs
{
    /// <summary>
    /// Tab 6: Testing & Quality
    /// Test coverage, security scanning, code quality
    /// </summary>
    public class Tab6_TestingQuality : IConfigurationTab
    {
        private ProjectConfiguration config;
        private Panel tabPanel;
        private NumericUpDown coverageTargetUpDown;
        private CheckBox unitTestsCheckBox;
        private CheckBox integrationTestsCheckBox;
        private CheckBox e2eTestsCheckBox;
        private CheckBox securityScanCheckBox;
        private CheckBox codeQualityCheckBox;
        private CheckBox dependencyCheckCheckBox;
        private TextBox notesTextBox;
        private Label validationLabel;

        public Tab6_TestingQuality(ProjectConfiguration configuration)
        {
            this.config = configuration;
            InitializeTab();
        }

        private void InitializeTab()
        {
            tabPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(15)
            };

            int yPos = 10;
            const int controlHeight = 25;
            const int spacing = 15;
            const int labelWidth = 160;

            // Code Coverage Target
            Label coverageLabel = CreateLabel("Code Coverage Target %:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(coverageLabel);

            coverageTargetUpDown = new NumericUpDown
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(80, controlHeight),
                Minimum = 0,
                Maximum = 100,
                Value = 80
            };
            tabPanel.Controls.Add(coverageTargetUpDown);
            yPos += controlHeight + spacing;

            // Unit Tests
            unitTestsCheckBox = new CheckBox
            {
                Text = "Unit Tests Required",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(unitTestsCheckBox);
            yPos += controlHeight + spacing;

            // Integration Tests
            integrationTestsCheckBox = new CheckBox
            {
                Text = "Integration Tests Required",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(integrationTestsCheckBox);
            yPos += controlHeight + spacing;

            // E2E Tests
            e2eTestsCheckBox = new CheckBox
            {
                Text = "End-to-End (E2E) Tests Required",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false
            };
            tabPanel.Controls.Add(e2eTestsCheckBox);
            yPos += controlHeight + spacing;

            // Security Scanning
            securityScanCheckBox = new CheckBox
            {
                Text = "Enable Security Vulnerability Scanning",
                Location = new Point(10, yPos),
                Size = new Size(350, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(securityScanCheckBox);
            yPos += controlHeight + spacing;

            // Code Quality
            codeQualityCheckBox = new CheckBox
            {
                Text = "Code Quality Analysis (SonarQube/Linting)",
                Location = new Point(10, yPos),
                Size = new Size(350, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(codeQualityCheckBox);
            yPos += controlHeight + spacing;

            // Dependency Check
            dependencyCheckCheckBox = new CheckBox
            {
                Text = "Dependency Vulnerability Checking",
                Location = new Point(10, yPos),
                Size = new Size(350, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(dependencyCheckCheckBox);
            yPos += controlHeight + spacing;

            // Testing Notes
            Label notesLabel = new Label
            {
                Text = "Testing Notes:",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(notesLabel);

            notesTextBox = new TextBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(400, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            tabPanel.Controls.Add(notesTextBox);
            yPos += 120;

            // Validation label
            validationLabel = new Label
            {
                Location = new Point(10, yPos),
                Size = new Size(400, 40),
                ForeColor = Color.Red,
                AutoSize = true,
                MaximumSize = new Size(400, 0)
            };
            tabPanel.Controls.Add(validationLabel);
        }

        private Label CreateLabel(string text, int x, int y, int width)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9F)
            };
        }

        public Control GetTabControl() => tabPanel;

        public bool ValidateTab()
        {
            validationLabel.Text = "";
            return true;
        }

        public string GetValidationError() => validationLabel.Text;

        public void OnLoad()
        {
        }

        public void OnUnload()
        {
            if (config.AdvancedConfig == null)
                config.AdvancedConfig = new System.Collections.Generic.Dictionary<string, object>();

            config.AdvancedConfig["CodeCoverageTarget"] = (int)coverageTargetUpDown.Value;
            config.AdvancedConfig["UnitTests"] = unitTestsCheckBox.Checked;
            config.AdvancedConfig["IntegrationTests"] = integrationTestsCheckBox.Checked;
            config.AdvancedConfig["E2ETests"] = e2eTestsCheckBox.Checked;
            config.AdvancedConfig["SecurityScanning"] = securityScanCheckBox.Checked;
            config.AdvancedConfig["CodeQualityAnalysis"] = codeQualityCheckBox.Checked;
            config.AdvancedConfig["DependencyVulnerabilityCheck"] = dependencyCheckCheckBox.Checked;
            config.AdvancedConfig["TestingNotes"] = notesTextBox.Text;
        }
    }
}
