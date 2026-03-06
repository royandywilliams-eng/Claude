using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UIScreens
{
    /// <summary>
    /// Screen 3: Key Features
    /// Select which features your project needs
    /// </summary>
    public class Screen3_KeyFeatures : IWizardScreen
    {
        private ProjectConfiguration config;
        private Panel screenPanel;
        private List<CheckBox> featureCheckBoxes;

        private static readonly string[] Features =
        {
            "Authentication",
            "Authorization",
            "Database Integration",
            "REST API",
            "Real-time Updates",
            "File Upload/Download",
            "Payment Integration",
            "Email/Notifications",
            "Admin Dashboard",
            "Analytics/Logging",
            "Search Functionality",
            "User Management"
        };

        public Screen3_KeyFeatures(ProjectConfiguration configuration)
        {
            this.config = configuration;
            featureCheckBoxes = new List<CheckBox>();
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

            // Title
            Label titleLabel = new Label
            {
                Text = "Select the features your project needs:",
                Location = new Point(10, 10),
                Size = new Size(300, 25),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            screenPanel.Controls.Add(titleLabel);

            int yPos = 40;
            const int controlHeight = 20;
            const int spacing = 8;

            foreach (var feature in Features)
            {
                CheckBox checkBox = new CheckBox
                {
                    Text = feature,
                    Location = new Point(10, yPos),
                    Size = new Size(300, controlHeight),
                    AutoSize = false,
                    Checked = config.Features.ContainsKey(feature) && config.Features[feature],
                    Font = new Font("Segoe UI", 9F)
                };

                string featureName = feature;
                checkBox.CheckedChanged += (s, e) =>
                {
                    if (!config.Features.ContainsKey(featureName))
                        config.Features[featureName] = false;
                    config.Features[featureName] = checkBox.Checked;
                };

                featureCheckBoxes.Add(checkBox);
                screenPanel.Controls.Add(checkBox);
                yPos += controlHeight + spacing;
            }

            // Info label
            Label infoLabel = new Label
            {
                Text = "You can select multiple features",
                Location = new Point(10, yPos + 10),
                Size = new Size(300, 30),
                ForeColor = SystemColors.GrayText,
                AutoSize = true,
                Font = new Font("Segoe UI", 8F)
            };
            screenPanel.Controls.Add(infoLabel);
        }

        public Control GetScreenControl() => screenPanel;

        public bool ValidateScreen()
        {
            // No validation needed - zero features is valid
            return true;
        }

        public string GetValidationError() => "";

        public void OnLoad()
        {
        }

        public void OnUnload()
        {
            // Save selected features
            config.Features.Clear();
            foreach (var feature in Features)
                config.Features[feature] = false;

            foreach (var checkBox in featureCheckBoxes)
                config.Features[checkBox.Text] = checkBox.Checked;
        }
    }
}
