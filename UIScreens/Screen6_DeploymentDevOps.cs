using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UIScreens
{
    /// <summary>
    /// Screen 6: Deployment & DevOps
    /// Configure hosting platform, CI/CD pipeline, Docker, and monitoring
    /// </summary>
    public class Screen6_DeploymentDevOps : IWizardScreen
    {
        private ProjectConfiguration config;
        private Panel screenPanel;
        private ComboBox hostingComboBox;
        private ComboBox cicdComboBox;
        private CheckBox dockerCheckBox;
        private CheckBox kubernetesCheckBox;
        private ComboBox monitoringComboBox;
        private Label validationLabel;

        private static readonly string[] HostingPlatforms =
        {
            "AWS", "Azure", "Google Cloud", "Heroku", "DigitalOcean",
            "Linode", "Vercel", "Netlify", "Firebase", "Self-Hosted"
        };

        private static readonly string[] CIPipelines =
        {
            "GitHub Actions", "GitLab CI", "Jenkins", "CircleCI",
            "Travis CI", "Azure DevOps", "AWS CodePipeline", "None"
        };

        private static readonly string[] MonitoringTools =
        {
            "CloudWatch", "Datadog", "New Relic", "Grafana",
            "Prometheus", "ELK Stack", "Splunk", "Sentry"
        };

        public Screen6_DeploymentDevOps(ProjectConfiguration configuration)
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

            // Hosting Platform
            Label hostingLabel = CreateLabel("Hosting Platform:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(hostingLabel);

            hostingComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(180, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var platform in HostingPlatforms)
                hostingComboBox.Items.Add(platform);

            hostingComboBox.SelectedItem = config.HostingPlatform;
            hostingComboBox.SelectedIndexChanged += (s, e) =>
                config.HostingPlatform = hostingComboBox.SelectedItem?.ToString() ?? "AWS";
            screenPanel.Controls.Add(hostingComboBox);
            yPos += controlHeight + spacing;

            // CI/CD Pipeline
            Label cicdLabel = CreateLabel("CI/CD Pipeline:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(cicdLabel);

            cicdComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(180, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var pipeline in CIPipelines)
                cicdComboBox.Items.Add(pipeline);

            cicdComboBox.SelectedItem = config.CIPipeline;
            cicdComboBox.SelectedIndexChanged += (s, e) =>
                config.CIPipeline = cicdComboBox.SelectedItem?.ToString() ?? "GitHub Actions";
            screenPanel.Controls.Add(cicdComboBox);
            yPos += controlHeight + spacing;

            // Docker
            dockerCheckBox = new CheckBox
            {
                Text = "Use Docker",
                Location = new Point(10, yPos),
                Size = new Size(300, checkBoxHeight),
                AutoSize = false,
                Checked = config.UseDocker,
                Font = new Font("Segoe UI", 9F)
            };
            dockerCheckBox.CheckedChanged += (s, e) => config.UseDocker = dockerCheckBox.Checked;
            screenPanel.Controls.Add(dockerCheckBox);
            yPos += checkBoxHeight + spacing;

            // Kubernetes
            kubernetesCheckBox = new CheckBox
            {
                Text = "Use Kubernetes",
                Location = new Point(10, yPos),
                Size = new Size(300, checkBoxHeight),
                AutoSize = false,
                Checked = config.UseKubernetes,
                Font = new Font("Segoe UI", 9F)
            };
            kubernetesCheckBox.CheckedChanged += (s, e) => config.UseKubernetes = kubernetesCheckBox.Checked;
            screenPanel.Controls.Add(kubernetesCheckBox);
            yPos += checkBoxHeight + spacing;

            // Monitoring Tools
            Label monitoringLabel = CreateLabel("Monitoring:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(monitoringLabel);

            monitoringComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(180, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var tool in MonitoringTools)
                monitoringComboBox.Items.Add(tool);

            monitoringComboBox.SelectedItem = config.MonitoringTools;
            monitoringComboBox.SelectedIndexChanged += (s, e) =>
                config.MonitoringTools = monitoringComboBox.SelectedItem?.ToString() ?? "CloudWatch";
            screenPanel.Controls.Add(monitoringComboBox);
            yPos += controlHeight + spacing;

            // Info
            Label infoLabel = new Label
            {
                Text = "This is the final step. You can review your configuration in the preview panel below.",
                Location = new Point(10, yPos),
                Size = new Size(330, 40),
                ForeColor = SystemColors.GrayText,
                AutoSize = true,
                MaximumSize = new Size(330, 0),
                Font = new Font("Segoe UI", 8F)
            };
            screenPanel.Controls.Add(infoLabel);
            yPos += 50;

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
