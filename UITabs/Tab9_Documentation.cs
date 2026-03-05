using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UITabs
{
    /// <summary>
    /// Tab 9: Documentation & Comments
    /// Business logic notes, comments, special requirements
    /// </summary>
    public class Tab9_Documentation : IConfigurationTab
    {
        private ProjectConfiguration config;
        private Panel tabPanel;
        private TextBox businessLogicTextBox;
        private TextBox specialRequirementsTextBox;
        private TextBox technicalNotesTextBox;
        private CheckBox readmeCheckBox;
        private CheckBox apiDocsCheckBox;
        private CheckBox architectureDocCheckBox;
        private Label validationLabel;

        public Tab9_Documentation(ProjectConfiguration configuration)
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
            const int spacing = 20;

            // Business Logic
            Label businessLabel = new Label
            {
                Text = "Business Logic & Workflows:",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(businessLabel);

            businessLogicTextBox = new TextBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(440, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "Describe key business rules, workflows, and logic the system must implement"
            };
            tabPanel.Controls.Add(businessLogicTextBox);
            yPos += 120;

            // Special Requirements
            Label specialLabel = new Label
            {
                Text = "Special Requirements:",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(specialLabel);

            specialRequirementsTextBox = new TextBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(440, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "List compliance requirements, SLAs, uptime guarantees, data residency, etc."
            };
            tabPanel.Controls.Add(specialRequirementsTextBox);
            yPos += 120;

            // Technical Notes
            Label technicalLabel = new Label
            {
                Text = "Additional Technical Notes:",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(technicalLabel);

            technicalNotesTextBox = new TextBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(440, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "Any other technical details, integrations, or special considerations"
            };
            tabPanel.Controls.Add(technicalNotesTextBox);
            yPos += 120;

            // Documentation Checkboxes
            Label docsLabel = new Label
            {
                Text = "Documentation to Generate:",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(docsLabel);
            yPos += 25;

            readmeCheckBox = new CheckBox
            {
                Text = "README with project overview and setup instructions",
                Location = new Point(10, yPos),
                Size = new Size(400, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(readmeCheckBox);
            yPos += 25;

            apiDocsCheckBox = new CheckBox
            {
                Text = "API Documentation (Swagger/OpenAPI)",
                Location = new Point(10, yPos),
                Size = new Size(400, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(apiDocsCheckBox);
            yPos += 25;

            architectureDocCheckBox = new CheckBox
            {
                Text = "Architecture & Design Documentation",
                Location = new Point(10, yPos),
                Size = new Size(400, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(architectureDocCheckBox);
            yPos += 40;

            // Info label
            Label infoLabel = new Label
            {
                Text = "This information will be included in your project specification for Claude",
                Location = new Point(10, yPos),
                Size = new Size(420, 30),
                ForeColor = SystemColors.GrayText,
                AutoSize = true,
                MaximumSize = new Size(420, 0),
                Font = new Font("Segoe UI", 8F)
            };
            tabPanel.Controls.Add(infoLabel);
            yPos += 50;

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

            config.AdvancedConfig["BusinessLogic"] = businessLogicTextBox.Text;
            config.AdvancedConfig["SpecialRequirements"] = specialRequirementsTextBox.Text;
            config.AdvancedConfig["TechnicalNotes"] = technicalNotesTextBox.Text;
            config.AdvancedConfig["GenerateREADME"] = readmeCheckBox.Checked;
            config.AdvancedConfig["GenerateAPIDocumentation"] = apiDocsCheckBox.Checked;
            config.AdvancedConfig["GenerateArchitectureDoc"] = architectureDocCheckBox.Checked;
        }
    }
}
