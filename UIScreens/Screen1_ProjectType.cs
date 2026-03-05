using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UIScreens
{
    /// <summary>
    /// Screen 1: Project Type & Scope
    /// Collects basic project information
    /// </summary>
    public class Screen1_ProjectType : IWizardScreen
    {
        private ProjectConfiguration config;
        private Panel screenPanel;
        private TextBox projectNameTextBox;
        private ComboBox projectTypeComboBox;
        private ComboBox complexityComboBox;
        private TextBox descriptionTextBox;
        private TextBox targetAudienceTextBox;
        private Label validationLabel;

        public Screen1_ProjectType(ProjectConfiguration configuration)
        {
            this.config = configuration;
            InitializeScreen();
        }

        private void InitializeScreen()
        {
            screenPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            int yPos = 10;
            const int controlHeight = 25;
            const int spacing = 10;
            const int labelWidth = 120;
            const int inputWidth = 200;

            // Project Name
            Label nameLabel = CreateLabel("Project Name:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(nameLabel);

            projectNameTextBox = new TextBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(inputWidth, controlHeight),
                Text = config.ProjectName
            };
            projectNameTextBox.TextChanged += (s, e) => config.ProjectName = projectNameTextBox.Text;
            screenPanel.Controls.Add(projectNameTextBox);
            yPos += controlHeight + spacing;

            // Project Type
            Label typeLabel = CreateLabel("Project Type:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(typeLabel);

            projectTypeComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(inputWidth, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Web App", "Desktop App", "Backend API", "CLI Tool", "Mobile App", "Game" }
            };
            if (!string.IsNullOrEmpty(config.ProjectType))
                projectTypeComboBox.SelectedItem = config.ProjectType;
            projectTypeComboBox.SelectedIndexChanged += (s, e) => config.ProjectType = projectTypeComboBox.SelectedItem?.ToString() ?? "";
            screenPanel.Controls.Add(projectTypeComboBox);
            yPos += controlHeight + spacing;

            // Complexity Level
            Label complexityLabel = CreateLabel("Complexity:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(complexityLabel);

            complexityComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(inputWidth, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Simple", "Medium", "Advanced", "Enterprise" }
            };
            complexityComboBox.SelectedItem = config.ComplexityLevel ?? "Medium";
            complexityComboBox.SelectedIndexChanged += (s, e) => config.ComplexityLevel = complexityComboBox.SelectedItem?.ToString() ?? "Medium";
            screenPanel.Controls.Add(complexityComboBox);
            yPos += controlHeight + spacing;

            // Target Audience
            Label audienceLabel = CreateLabel("Target Audience:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(audienceLabel);

            targetAudienceTextBox = new TextBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(inputWidth, controlHeight),
                Text = config.TargetAudience
            };
            targetAudienceTextBox.TextChanged += (s, e) => config.TargetAudience = targetAudienceTextBox.Text;
            screenPanel.Controls.Add(targetAudienceTextBox);
            yPos += controlHeight + spacing;

            // Description
            Label descLabel = CreateLabel("Description:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(descLabel);

            descriptionTextBox = new TextBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(330, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Text = config.ProjectDescription
            };
            descriptionTextBox.TextChanged += (s, e) => config.ProjectDescription = descriptionTextBox.Text;
            screenPanel.Controls.Add(descriptionTextBox);
            yPos += 140;

            // Validation label
            validationLabel = new Label
            {
                Location = new Point(10, yPos),
                Size = new Size(330, 40),
                ForeColor = Color.Red,
                AutoSize = true,
                MaximumSize = new Size(330, 0)
            };
            screenPanel.Controls.Add(validationLabel);
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

        public Control GetScreenControl() => screenPanel;

        public bool ValidateScreen()
        {
            if (string.IsNullOrWhiteSpace(projectNameTextBox.Text))
            {
                validationLabel.Text = "Project name is required";
                return false;
            }

            if (projectTypeComboBox.SelectedItem == null)
            {
                validationLabel.Text = "Please select a project type";
                return false;
            }

            validationLabel.Text = "";
            return true;
        }

        public string GetValidationError() => validationLabel.Text;

        public void OnLoad()
        {
            projectNameTextBox.Focus();
        }

        public void OnUnload()
        {
        }
    }
}
