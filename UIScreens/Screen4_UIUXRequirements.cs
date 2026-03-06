using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UIScreens
{
    /// <summary>
    /// Screen 4: UI/UX Requirements
    /// Configure design framework, responsive design, dark mode, and accessibility
    /// </summary>
    public class Screen4_UIUXRequirements : IWizardScreen
    {
        private ProjectConfiguration config;
        private Panel screenPanel;
        private ComboBox designFrameworkComboBox;
        private CheckBox responsiveCheckBox;
        private CheckBox darkModeCheckBox;
        private CheckBox mobileCompatibilityCheckBox;
        private ComboBox accessibilityComboBox;
        private Label validationLabel;

        private static readonly string[] DesignFrameworks =
        {
            "Bootstrap", "Tailwind CSS", "Material Design", "Foundation",
            "Ant Design", "Chakra UI", "Material-UI", "Semantic UI",
            "Bulma", "Custom CSS"
        };

        private static readonly string[] AccessibilityLevels =
        {
            "WCAG 2.1 A", "WCAG 2.1 AA", "WCAG 2.1 AAA", "Section 508", "None"
        };

        public Screen4_UIUXRequirements(ProjectConfiguration configuration)
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
            const int checkBoxHeight = 20;
            const int spacing = 10;
            const int labelWidth = 140;

            // Design Framework
            Label frameworkLabel = CreateLabel("Design Framework:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(frameworkLabel);

            designFrameworkComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(180, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var fw in DesignFrameworks)
                designFrameworkComboBox.Items.Add(fw);

            designFrameworkComboBox.SelectedItem = config.DesignFramework;
            designFrameworkComboBox.SelectedIndexChanged += (s, e) =>
                config.DesignFramework = designFrameworkComboBox.SelectedItem?.ToString() ?? "Bootstrap";
            screenPanel.Controls.Add(designFrameworkComboBox);
            yPos += controlHeight + spacing;

            // Responsive Design
            responsiveCheckBox = new CheckBox
            {
                Text = "Responsive Design Required",
                Location = new Point(10, yPos),
                Size = new Size(300, checkBoxHeight),
                AutoSize = false,
                Checked = config.ResponsiveDesign,
                Font = new Font("Segoe UI", 9F)
            };
            responsiveCheckBox.CheckedChanged += (s, e) => config.ResponsiveDesign = responsiveCheckBox.Checked;
            screenPanel.Controls.Add(responsiveCheckBox);
            yPos += checkBoxHeight + spacing;

            // Dark Mode Support
            darkModeCheckBox = new CheckBox
            {
                Text = "Dark Mode Support",
                Location = new Point(10, yPos),
                Size = new Size(300, checkBoxHeight),
                AutoSize = false,
                Checked = config.DarkModeSupport,
                Font = new Font("Segoe UI", 9F)
            };
            darkModeCheckBox.CheckedChanged += (s, e) => config.DarkModeSupport = darkModeCheckBox.Checked;
            screenPanel.Controls.Add(darkModeCheckBox);
            yPos += checkBoxHeight + spacing;

            // Mobile Compatibility
            mobileCompatibilityCheckBox = new CheckBox
            {
                Text = "Mobile Compatibility Required",
                Location = new Point(10, yPos),
                Size = new Size(300, checkBoxHeight),
                AutoSize = false,
                Checked = config.MobileCompatibility,
                Font = new Font("Segoe UI", 9F)
            };
            mobileCompatibilityCheckBox.CheckedChanged += (s, e) => config.MobileCompatibility = mobileCompatibilityCheckBox.Checked;
            screenPanel.Controls.Add(mobileCompatibilityCheckBox);
            yPos += checkBoxHeight + spacing;

            // Accessibility Requirements
            Label accessibilityLabel = CreateLabel("Accessibility:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(accessibilityLabel);

            accessibilityComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(180, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var level in AccessibilityLevels)
                accessibilityComboBox.Items.Add(level);

            accessibilityComboBox.SelectedItem = config.AccessibilityRequirements;
            accessibilityComboBox.SelectedIndexChanged += (s, e) =>
                config.AccessibilityRequirements = accessibilityComboBox.SelectedItem?.ToString() ?? "WCAG 2.1 AA";
            screenPanel.Controls.Add(accessibilityComboBox);
            yPos += controlHeight + spacing;

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
            validationLabel.Text = "";
            return true;
        }

        public string GetValidationError() => validationLabel.Text;

        public void OnLoad()
        {
        }

        public void OnUnload()
        {
        }
    }
}
