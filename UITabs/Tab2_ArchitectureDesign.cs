using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UITabs
{
    /// <summary>
    /// Tab 2: Architecture & Design
    /// Design patterns, API design, authentication approach
    /// </summary>
    public class Tab2_ArchitectureDesign : IConfigurationTab
    {
        private ProjectConfiguration config;
        private Panel tabPanel;
        private ListBox patternsListBox;
        private ComboBox apiDesignComboBox;
        private ComboBox authMethodComboBox;
        private TextBox notesTextBox;
        private Label validationLabel;

        private static readonly string[] DesignPatterns =
        {
            "MVC", "MVVM", "MVP", "Repository", "Factory", "Singleton",
            "Observer", "Adapter", "Strategy", "State", "Decorator",
            "Facade", "Command", "Chain of Responsibility"
        };

        private static readonly string[] APIDesignStyles =
        {
            "RESTful", "GraphQL", "gRPC", "SOAP", "Event-driven", "Hybrid (REST + GraphQL)"
        };

        private static readonly string[] AuthMethods =
        {
            "JWT Tokens", "Session-based", "OAuth 2.0", "SAML", "API Keys",
            "mTLS", "Basic Auth", "Custom"
        };

        public Tab2_ArchitectureDesign(ProjectConfiguration configuration)
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
            const int labelWidth = 140;

            // Design Patterns
            Label patternsLabel = new Label
            {
                Text = "Design Patterns:",
                Location = new Point(10, yPos),
                Size = new Size(labelWidth, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(patternsLabel);

            patternsListBox = new ListBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(350, 100),
                SelectionMode = SelectionMode.MultiSimple
            };
            foreach (var pattern in DesignPatterns)
                patternsListBox.Items.Add(pattern);

            tabPanel.Controls.Add(patternsListBox);
            yPos += 140;

            // API Design
            Label apiLabel = CreateLabel("API Design Style:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(apiLabel);

            apiDesignComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var style in APIDesignStyles)
                apiDesignComboBox.Items.Add(style);

            apiDesignComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(apiDesignComboBox);
            yPos += 45;

            // Authentication Method
            Label authLabel = CreateLabel("Auth Method:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(authLabel);

            authMethodComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(250, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var method in AuthMethods)
                authMethodComboBox.Items.Add(method);

            authMethodComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(authMethodComboBox);
            yPos += 45;

            // Architecture Notes
            Label notesLabel = new Label
            {
                Text = "Architecture Notes:",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(notesLabel);

            notesTextBox = new TextBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(400, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            tabPanel.Controls.Add(notesTextBox);
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
            validationLabel.Text = "";
            return true;
        }

        public string GetValidationError() => validationLabel.Text;

        public void OnLoad()
        {
        }

        public void OnUnload()
        {
            // Save selected patterns
            if (config.AdvancedConfig == null)
                config.AdvancedConfig = new System.Collections.Generic.Dictionary<string, object>();

            var patterns = new System.Collections.Generic.List<string>();
            foreach (var item in patternsListBox.SelectedItems)
                patterns.Add(item.ToString());

            config.AdvancedConfig["DesignPatterns"] = patterns;
            config.AdvancedConfig["APIDesignStyle"] = apiDesignComboBox.SelectedItem?.ToString() ?? "RESTful";
            config.AdvancedConfig["AuthenticationMethod"] = authMethodComboBox.SelectedItem?.ToString() ?? "JWT Tokens";
            config.AdvancedConfig["ArchitectureNotes"] = notesTextBox.Text;
        }
    }
}
