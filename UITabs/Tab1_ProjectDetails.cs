using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UITabs
{
    /// <summary>
    /// Tab 1: Project Details
    /// Version, author, license, repository URL
    /// </summary>
    public class Tab1_ProjectDetails : IConfigurationTab
    {
        private ProjectConfiguration config;
        private Panel tabPanel;
        private TextBox versionTextBox;
        private TextBox authorTextBox;
        private ComboBox licenseComboBox;
        private TextBox repoUrlTextBox;
        private Label validationLabel;

        private static readonly string[] Licenses =
        {
            "MIT", "Apache 2.0", "GPL v3", "BSD 3-Clause", "ISC",
            "MPL 2.0", "LGPL v3", "Unlicense", "Custom", "Proprietary"
        };

        public Tab1_ProjectDetails(ProjectConfiguration configuration)
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
            const int labelWidth = 120;

            // Version
            Label versionLabel = CreateLabel("Version:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(versionLabel);

            versionTextBox = new TextBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, controlHeight),
                Text = config.Version
            };
            versionTextBox.TextChanged += (s, e) => config.Version = versionTextBox.Text;
            tabPanel.Controls.Add(versionTextBox);
            yPos += controlHeight + spacing;

            // Author
            Label authorLabel = CreateLabel("Author:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(authorLabel);

            authorTextBox = new TextBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, controlHeight),
                Text = config.Author
            };
            authorTextBox.TextChanged += (s, e) => config.Author = authorTextBox.Text;
            tabPanel.Controls.Add(authorTextBox);
            yPos += controlHeight + spacing;

            // License
            Label licenseLabel = CreateLabel("License:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(licenseLabel);

            licenseComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDown
            };
            foreach (var license in Licenses)
                licenseComboBox.Items.Add(license);

            if (licenseComboBox.Items.Contains(config.License))
                licenseComboBox.SelectedItem = config.License;
            else
                licenseComboBox.SelectedIndex = 0;

            licenseComboBox.SelectedIndexChanged += (s, e) =>
                config.License = licenseComboBox.SelectedItem?.ToString() ?? "MIT";
            tabPanel.Controls.Add(licenseComboBox);
            yPos += controlHeight + spacing;

            // Repository URL
            Label repoLabel = CreateLabel("Repository URL:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(repoLabel);

            repoUrlTextBox = new TextBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(350, controlHeight),
                Text = config.RepositoryUrl,
                PlaceholderText = "https://github.com/user/repo"
            };
            repoUrlTextBox.TextChanged += (s, e) => config.RepositoryUrl = repoUrlTextBox.Text;
            tabPanel.Controls.Add(repoUrlTextBox);
            yPos += controlHeight + spacing;

            // Info label
            Label infoLabel = new Label
            {
                Text = "These details will be included in your project specification",
                Location = new Point(10, yPos),
                Size = new Size(400, 30),
                ForeColor = SystemColors.GrayText,
                AutoSize = true,
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
            if (!string.IsNullOrWhiteSpace(repoUrlTextBox.Text) && !IsValidUrl(repoUrlTextBox.Text))
            {
                validationLabel.Text = "Repository URL must be a valid URL";
                return false;
            }

            validationLabel.Text = "";
            return true;
        }

        private bool IsValidUrl(string url)
        {
            return url.StartsWith("http://") || url.StartsWith("https://") || url.Contains("github.com") || url.Contains("gitlab.com");
        }

        public string GetValidationError() => validationLabel.Text;

        public void OnLoad()
        {
            versionTextBox.Focus();
        }

        public void OnUnload()
        {
        }
    }
}
