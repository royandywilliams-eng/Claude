using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UITabs
{
    /// <summary>
    /// Tab 4: Backend Specifics
    /// API versioning, rate limiting, caching, logging
    /// </summary>
    public class Tab4_BackendSpecifics : IConfigurationTab
    {
        private ProjectConfiguration config;
        private Panel tabPanel;
        private ComboBox apiVersioningComboBox;
        private CheckBox rateLimitingCheckBox;
        private ComboBox loggingFrameworkComboBox;
        private ComboBox monitoringComboBox;
        private CheckBox metricsCheckBox;
        private TextBox notesTextBox;
        private Label validationLabel;

        private static readonly string[] APIVersioning =
        {
            "URL Path (/v1/)", "Query Parameter (?version=1)", "Header (Accept-Version)",
            "Media Type (application/vnd.api+json)", "No Versioning"
        };

        private static readonly string[] LoggingFrameworks =
        {
            "Winston", "Bunyan", "Pino", "Log4j", "SLF4J",
            "NLog", "Serilog", "Structured Logging", "ELK Stack", "Papertrail"
        };

        private static readonly string[] MonitoringTools =
        {
            "Prometheus", "Grafana", "New Relic", "Datadog", "AppDynamics",
            "Elastic APM", "Jaeger", "Zipkin", "Cloud-native", "Custom"
        };

        public Tab4_BackendSpecifics(ProjectConfiguration configuration)
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
            const int labelWidth = 160;

            // API Versioning
            Label versioningLabel = CreateLabel("API Versioning:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(versioningLabel);

            apiVersioningComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var versioning in APIVersioning)
                apiVersioningComboBox.Items.Add(versioning);
            apiVersioningComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(apiVersioningComboBox);
            yPos += controlHeight + spacing;

            // Rate Limiting
            rateLimitingCheckBox = new CheckBox
            {
                Text = "Implement Rate Limiting",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(rateLimitingCheckBox);
            yPos += controlHeight + spacing;

            // Logging Framework
            Label loggingLabel = CreateLabel("Logging Framework:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(loggingLabel);

            loggingFrameworkComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var logging in LoggingFrameworks)
                loggingFrameworkComboBox.Items.Add(logging);
            loggingFrameworkComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(loggingFrameworkComboBox);
            yPos += controlHeight + spacing;

            // Monitoring
            Label monitoringLabel = CreateLabel("Monitoring Tool:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(monitoringLabel);

            monitoringComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var monitoring in MonitoringTools)
                monitoringComboBox.Items.Add(monitoring);
            monitoringComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(monitoringComboBox);
            yPos += controlHeight + spacing;

            // Metrics Collection
            metricsCheckBox = new CheckBox
            {
                Text = "Collect Detailed Metrics",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(metricsCheckBox);
            yPos += controlHeight + spacing;

            // Backend Notes
            Label notesLabel = new Label
            {
                Text = "Backend Notes:",
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

            config.AdvancedConfig["APIVersioning"] = apiVersioningComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["RateLimiting"] = rateLimitingCheckBox.Checked;
            config.AdvancedConfig["LoggingFramework"] = loggingFrameworkComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["MonitoringTool"] = monitoringComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["DetailedMetrics"] = metricsCheckBox.Checked;
            config.AdvancedConfig["BackendNotes"] = notesTextBox.Text;
        }
    }
}
