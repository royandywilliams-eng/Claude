using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UITabs
{
    /// <summary>
    /// Tab 7: Dependencies & Integration
    /// External APIs, payment processors, email services
    /// </summary>
    public class Tab7_DependenciesIntegration : IConfigurationTab
    {
        private ProjectConfiguration config;
        private Panel tabPanel;
        private CheckBox paymentCheckBox;
        private ComboBox paymentProviderComboBox;
        private CheckBox emailCheckBox;
        private ComboBox emailServiceComboBox;
        private CheckBox analyticsCheckBox;
        private ComboBox analyticsComboBox;
        private CheckBox storageCheckBox;
        private ComboBox storageComboBox;
        private TextBox customIntegrationsTextBox;
        private Label validationLabel;

        private static readonly string[] PaymentProviders =
        {
            "Stripe", "PayPal", "Square", "Braintree", "Authorize.net",
            "2Checkout", "Razorpay", "Skrill", "Google Pay", "Apple Pay"
        };

        private static readonly string[] EmailServices =
        {
            "SendGrid", "Mailgun", "Amazon SES", "Postmark", "SparkPost",
            "Braze", "Klaviyo", "Mailchimp", "Gmail API", "Custom SMTP"
        };

        private static readonly string[] AnalyticsServices =
        {
            "Google Analytics", "Mixpanel", "Amplitude", "Segment", "Hotjar",
            "Sentry (Error Tracking)", "LogRocket", "Heap", "Custom", "None"
        };

        private static readonly string[] StorageServices =
        {
            "AWS S3", "Google Cloud Storage", "Azure Blob Storage", "Cloudinary",
            "Firebase Storage", "DigitalOcean Spaces", "MinIO", "Local Storage"
        };

        public Tab7_DependenciesIntegration(ProjectConfiguration configuration)
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
            const int labelWidth = 140;

            // Payment Integration
            paymentCheckBox = new CheckBox
            {
                Text = "Payment Integration Required",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false
            };
            paymentCheckBox.CheckedChanged += (s, e) => paymentProviderComboBox.Enabled = paymentCheckBox.Checked;
            tabPanel.Controls.Add(paymentCheckBox);
            yPos += controlHeight + spacing;

            Label paymentLabel = CreateLabel("Payment Provider:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(paymentLabel);

            paymentProviderComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            foreach (var provider in PaymentProviders)
                paymentProviderComboBox.Items.Add(provider);
            paymentProviderComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(paymentProviderComboBox);
            yPos += controlHeight + spacing;

            // Email Service
            emailCheckBox = new CheckBox
            {
                Text = "Email Service Integration",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false,
                Checked = true
            };
            emailCheckBox.CheckedChanged += (s, e) => emailServiceComboBox.Enabled = emailCheckBox.Checked;
            tabPanel.Controls.Add(emailCheckBox);
            yPos += controlHeight + spacing;

            Label emailLabel = CreateLabel("Email Service:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(emailLabel);

            emailServiceComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var service in EmailServices)
                emailServiceComboBox.Items.Add(service);
            emailServiceComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(emailServiceComboBox);
            yPos += controlHeight + spacing;

            // Analytics
            analyticsCheckBox = new CheckBox
            {
                Text = "Analytics & Tracking",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false,
                Checked = true
            };
            analyticsCheckBox.CheckedChanged += (s, e) => analyticsComboBox.Enabled = analyticsCheckBox.Checked;
            tabPanel.Controls.Add(analyticsCheckBox);
            yPos += controlHeight + spacing;

            Label analyticsLabel = CreateLabel("Analytics Service:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(analyticsLabel);

            analyticsComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var service in AnalyticsServices)
                analyticsComboBox.Items.Add(service);
            analyticsComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(analyticsComboBox);
            yPos += controlHeight + spacing;

            // Storage Service
            storageCheckBox = new CheckBox
            {
                Text = "Cloud Storage Required",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false
            };
            storageCheckBox.CheckedChanged += (s, e) => storageComboBox.Enabled = storageCheckBox.Checked;
            tabPanel.Controls.Add(storageCheckBox);
            yPos += controlHeight + spacing;

            Label storageLabel = CreateLabel("Storage Service:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(storageLabel);

            storageComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            foreach (var service in StorageServices)
                storageComboBox.Items.Add(service);
            storageComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(storageComboBox);
            yPos += controlHeight + spacing;

            // Custom Integrations
            Label customLabel = new Label
            {
                Text = "Custom Integrations:",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(customLabel);

            customIntegrationsTextBox = new TextBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(420, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "List any other external APIs or integrations needed"
            };
            tabPanel.Controls.Add(customIntegrationsTextBox);
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

            config.AdvancedConfig["PaymentIntegration"] = paymentCheckBox.Checked;
            config.AdvancedConfig["PaymentProvider"] = paymentProviderComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["EmailService"] = emailCheckBox.Checked;
            config.AdvancedConfig["EmailServiceProvider"] = emailServiceComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["Analytics"] = analyticsCheckBox.Checked;
            config.AdvancedConfig["AnalyticsService"] = analyticsComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["CloudStorage"] = storageCheckBox.Checked;
            config.AdvancedConfig["StorageService"] = storageComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["CustomIntegrations"] = customIntegrationsTextBox.Text;
        }
    }
}
