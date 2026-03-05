using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UIScreens
{
    /// <summary>
    /// Screen 5: Performance & Scalability
    /// Configure performance targets and scalability requirements
    /// </summary>
    public class Screen5_PerformanceScalability : IWizardScreen
    {
        private ProjectConfiguration config;
        private Panel screenPanel;
        private TextBox expectedUsersTextBox;
        private CheckBox realtimeDataCheckBox;
        private ComboBox cachingStrategyComboBox;
        private CheckBox cdnCheckBox;
        private ComboBox dbOptimizationComboBox;
        private Label validationLabel;

        private static readonly string[] CachingStrategies =
        {
            "None", "In-Memory Cache", "Redis", "Memcached",
            "HTTP Cache Headers", "CDN Cache", "Database Query Cache"
        };

        private static readonly string[] DBOptimizationLevels =
        {
            "Standard indexes", "Advanced indexing", "Query optimization",
            "Partitioning", "Replication", "Read replicas"
        };

        public Screen5_PerformanceScalability(ProjectConfiguration configuration)
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
            const int labelWidth = 160;

            // Expected Users
            Label usersLabel = CreateLabel("Expected Users:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(usersLabel);

            expectedUsersTextBox = new TextBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(150, controlHeight),
                Text = config.ExpectedUsers
            };
            expectedUsersTextBox.TextChanged += (s, e) => config.ExpectedUsers = expectedUsersTextBox.Text;
            screenPanel.Controls.Add(expectedUsersTextBox);
            yPos += controlHeight + spacing;

            // Real-time Data Needs
            realtimeDataCheckBox = new CheckBox
            {
                Text = "Real-time Data Updates Required",
                Location = new Point(10, yPos),
                Size = new Size(300, checkBoxHeight),
                AutoSize = false,
                Checked = config.RealtimeDataNeeds,
                Font = new Font("Segoe UI", 9F)
            };
            realtimeDataCheckBox.CheckedChanged += (s, e) => config.RealtimeDataNeeds = realtimeDataCheckBox.Checked;
            screenPanel.Controls.Add(realtimeDataCheckBox);
            yPos += checkBoxHeight + spacing;

            // Caching Strategy
            Label cachingLabel = CreateLabel("Caching Strategy:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(cachingLabel);

            cachingStrategyComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(150, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var strategy in CachingStrategies)
                cachingStrategyComboBox.Items.Add(strategy);

            cachingStrategyComboBox.SelectedItem = config.CachingStrategy;
            cachingStrategyComboBox.SelectedIndexChanged += (s, e) =>
                config.CachingStrategy = cachingStrategyComboBox.SelectedItem?.ToString() ?? "None";
            screenPanel.Controls.Add(cachingStrategyComboBox);
            yPos += controlHeight + spacing;

            // CDN Usage
            cdnCheckBox = new CheckBox
            {
                Text = "Use Content Delivery Network (CDN)",
                Location = new Point(10, yPos),
                Size = new Size(300, checkBoxHeight),
                AutoSize = false,
                Checked = config.CDNUsage,
                Font = new Font("Segoe UI", 9F)
            };
            cdnCheckBox.CheckedChanged += (s, e) => config.CDNUsage = cdnCheckBox.Checked;
            screenPanel.Controls.Add(cdnCheckBox);
            yPos += checkBoxHeight + spacing;

            // Database Optimization
            Label dbOptLabel = CreateLabel("DB Optimization:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(dbOptLabel);

            dbOptimizationComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(150, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var level in DBOptimizationLevels)
                dbOptimizationComboBox.Items.Add(level);

            dbOptimizationComboBox.SelectedItem = config.DatabaseOptimization;
            dbOptimizationComboBox.SelectedIndexChanged += (s, e) =>
                config.DatabaseOptimization = dbOptimizationComboBox.SelectedItem?.ToString() ?? "Standard indexes";
            screenPanel.Controls.Add(dbOptimizationComboBox);
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
            if (string.IsNullOrWhiteSpace(expectedUsersTextBox.Text))
            {
                validationLabel.Text = "Please enter expected number of users";
                return false;
            }

            if (!int.TryParse(expectedUsersTextBox.Text, out _))
            {
                validationLabel.Text = "Expected users must be a valid number";
                return false;
            }

            validationLabel.Text = "";
            return true;
        }

        public string GetValidationError() => validationLabel.Text;

        public void OnLoad()
        {
            expectedUsersTextBox.Focus();
        }

        public void OnUnload()
        {
        }
    }
}
