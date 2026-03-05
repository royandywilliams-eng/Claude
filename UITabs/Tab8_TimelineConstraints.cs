using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UITabs
{
    /// <summary>
    /// Tab 8: Timeline & Constraints
    /// Deadline, budget, team size, maintenance plan
    /// </summary>
    public class Tab8_TimelineConstraints : IConfigurationTab
    {
        private ProjectConfiguration config;
        private Panel tabPanel;
        private DateTimePicker deadlinePicke;
        private TextBox estimatedHoursTextBox;
        private NumericUpDown teamSizeUpDown;
        private ComboBox budgetRangeComboBox;
        private CheckBox maintenanceCheckBox;
        private TextBox constraintsTextBox;
        private Label validationLabel;

        private static readonly string[] BudgetRanges =
        {
            "Under $5,000", "$5,000 - $10,000", "$10,000 - $25,000",
            "$25,000 - $50,000", "$50,000 - $100,000", "Over $100,000", "TBD"
        };

        public Tab8_TimelineConstraints(ProjectConfiguration configuration)
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
            const int spacing = 20;
            const int labelWidth = 140;

            // Target Deadline
            Label deadlineLabel = CreateLabel("Target Deadline:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(deadlineLabel);

            deadlinePicke = new DateTimePicker
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(200, controlHeight),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(3)
            };
            tabPanel.Controls.Add(deadlinePicke);
            yPos += controlHeight + spacing;

            // Estimated Hours
            Label hoursLabel = CreateLabel("Estimated Hours:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(hoursLabel);

            estimatedHoursTextBox = new TextBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(150, controlHeight),
                Text = "160",
                PlaceholderText = "e.g., 160 for 1 developer month"
            };
            tabPanel.Controls.Add(estimatedHoursTextBox);
            yPos += controlHeight + spacing;

            // Team Size
            Label teamLabel = CreateLabel("Team Size:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(teamLabel);

            teamSizeUpDown = new NumericUpDown
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(80, controlHeight),
                Minimum = 1,
                Maximum = 50,
                Value = 1
            };
            tabPanel.Controls.Add(teamSizeUpDown);
            yPos += controlHeight + spacing;

            // Budget
            Label budgetLabel = CreateLabel("Budget Range:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(budgetLabel);

            budgetRangeComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(200, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var range in BudgetRanges)
                budgetRangeComboBox.Items.Add(range);
            budgetRangeComboBox.SelectedIndex = 6; // TBD
            tabPanel.Controls.Add(budgetRangeComboBox);
            yPos += controlHeight + spacing;

            // Maintenance
            maintenanceCheckBox = new CheckBox
            {
                Text = "Post-Launch Maintenance/Support Required",
                Location = new Point(10, yPos),
                Size = new Size(350, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(maintenanceCheckBox);
            yPos += controlHeight + spacing;

            // Constraints
            Label constraintsLabel = new Label
            {
                Text = "Constraints & Requirements:",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(constraintsLabel);

            constraintsTextBox = new TextBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(420, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "List any constraints (browser support, dependencies, legal requirements, etc.)"
            };
            tabPanel.Controls.Add(constraintsTextBox);
            yPos += 140;

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
            if (string.IsNullOrWhiteSpace(estimatedHoursTextBox.Text))
            {
                validationLabel.Text = "Please enter estimated hours";
                return false;
            }

            if (!int.TryParse(estimatedHoursTextBox.Text, out int hours) || hours <= 0)
            {
                validationLabel.Text = "Estimated hours must be a positive number";
                return false;
            }

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

            config.AdvancedConfig["TargetDeadline"] = deadlinePicke.Value;
            config.AdvancedConfig["EstimatedHours"] = estimatedHoursTextBox.Text;
            config.AdvancedConfig["TeamSize"] = (int)teamSizeUpDown.Value;
            config.AdvancedConfig["BudgetRange"] = budgetRangeComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["PostLaunchMaintenance"] = maintenanceCheckBox.Checked;
            config.AdvancedConfig["Constraints"] = constraintsTextBox.Text;
        }
    }
}
