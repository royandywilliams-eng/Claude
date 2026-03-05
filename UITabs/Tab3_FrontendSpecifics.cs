using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UITabs
{
    /// <summary>
    /// Tab 3: Frontend Specifics
    /// State management, routing, testing frameworks
    /// </summary>
    public class Tab3_FrontendSpecifics : IConfigurationTab
    {
        private ProjectConfiguration config;
        private Panel tabPanel;
        private ComboBox stateManagementComboBox;
        private ComboBox routingComboBox;
        private ComboBox testingFrameworkComboBox;
        private CheckBox cssPreprocessorCheckBox;
        private CheckBox buildToolCheckBox;
        private TextBox notesTextBox;
        private Label validationLabel;

        private static readonly string[] StateManagement =
        {
            "Redux", "MobX", "Vuex", "Pinia", "Context API",
            "Recoil", "Zustand", "Jotai", "TanStack Query", "None"
        };

        private static readonly string[] RoutingLibraries =
        {
            "React Router", "Vue Router", "Angular Router", "Next.js",
            "Nuxt", "Remix", "SvelteKit", "Astro", "Qwik", "Custom"
        };

        private static readonly string[] TestingFrameworks =
        {
            "Jest", "Vitest", "Mocha", "Jasmine", "Karma",
            "Playwright", "Cypress", "Testing Library", "Puppeteer"
        };

        public Tab3_FrontendSpecifics(ProjectConfiguration configuration)
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

            // State Management
            Label stateLabel = CreateLabel("State Management:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(stateLabel);

            stateManagementComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(200, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var sm in StateManagement)
                stateManagementComboBox.Items.Add(sm);
            stateManagementComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(stateManagementComboBox);
            yPos += controlHeight + spacing;

            // Routing
            Label routingLabel = CreateLabel("Routing Library:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(routingLabel);

            routingComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(200, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var routing in RoutingLibraries)
                routingComboBox.Items.Add(routing);
            routingComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(routingComboBox);
            yPos += controlHeight + spacing;

            // Testing Framework
            Label testingLabel = CreateLabel("Testing Framework:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(testingLabel);

            testingFrameworkComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(200, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var testing in TestingFrameworks)
                testingFrameworkComboBox.Items.Add(testing);
            testingFrameworkComboBox.SelectedIndex = 0;
            tabPanel.Controls.Add(testingFrameworkComboBox);
            yPos += controlHeight + spacing;

            // CSS Preprocessor
            cssPreprocessorCheckBox = new CheckBox
            {
                Text = "Use CSS Preprocessor (Sass/Less)",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false
            };
            tabPanel.Controls.Add(cssPreprocessorCheckBox);
            yPos += controlHeight + spacing;

            // Build Tool
            buildToolCheckBox = new CheckBox
            {
                Text = "Include Build Tool (Webpack/Vite/Parcel)",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(buildToolCheckBox);
            yPos += controlHeight + spacing;

            // Frontend Notes
            Label notesLabel = new Label
            {
                Text = "Frontend Notes:",
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

            config.AdvancedConfig["StateManagement"] = stateManagementComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["RoutingLibrary"] = routingComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["TestingFramework"] = testingFrameworkComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["CSSPreprocessor"] = cssPreprocessorCheckBox.Checked;
            config.AdvancedConfig["BuildTool"] = buildToolCheckBox.Checked;
            config.AdvancedConfig["FrontendNotes"] = notesTextBox.Text;
        }
    }
}
