using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UIScreens
{
    /// <summary>
    /// Screen 2: Technology Stack
    /// Select programming language, frameworks, and database
    /// </summary>
    public class Screen2_TechStack : IWizardScreen
    {
        private ProjectConfiguration config;
        private Panel screenPanel;
        private ComboBox languageComboBox;
        private ListBox frameworkListBox;
        private ComboBox databaseComboBox;
        private Label validationLabel;

        private static readonly string[] Languages =
        {
            "JavaScript", "Python", "C#", "Java", "Go", "Rust",
            "PHP", "Ruby", "TypeScript", "Swift", "Kotlin"
        };

        private static readonly string[] Databases =
        {
            "None", "PostgreSQL", "MySQL", "MongoDB", "SQLServer",
            "Firebase", "Redis", "DynamoDB", "Cassandra", "Oracle",
            "SQLite", "MariaDB"
        };

        public Screen2_TechStack(ProjectConfiguration configuration)
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
            const int spacing = 10;
            const int labelWidth = 100;

            // Primary Language
            Label langLabel = CreateLabel("Language:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(langLabel);

            languageComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(200, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var lang in Languages)
                languageComboBox.Items.Add(lang);

            if (!string.IsNullOrEmpty(config.PrimaryLanguage))
                languageComboBox.SelectedItem = config.PrimaryLanguage;
            else
                languageComboBox.SelectedIndex = 0;

            languageComboBox.SelectedIndexChanged += (s, e) => config.PrimaryLanguage = languageComboBox.SelectedItem?.ToString() ?? "";
            screenPanel.Controls.Add(languageComboBox);
            yPos += controlHeight + spacing;

            // Frameworks
            Label frameworkLabel = new Label
            {
                Text = "Frameworks:",
                Location = new Point(10, yPos),
                Size = new Size(labelWidth, 20),
                TextAlign = ContentAlignment.TopLeft
            };
            screenPanel.Controls.Add(frameworkLabel);

            frameworkListBox = new ListBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(330, 120),
                SelectionMode = SelectionMode.MultiSimple
            };

            // Populate frameworks based on selected language
            PopulateFrameworks();
            languageComboBox.SelectedIndexChanged += (s, e) => PopulateFrameworks();

            screenPanel.Controls.Add(frameworkListBox);
            yPos += 160;

            // Database
            Label dbLabel = CreateLabel("Database:", 10, yPos, labelWidth);
            screenPanel.Controls.Add(dbLabel);

            databaseComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(200, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var db in Databases)
                databaseComboBox.Items.Add(db);

            databaseComboBox.SelectedItem = config.Database;
            databaseComboBox.SelectedIndexChanged += (s, e) => config.Database = databaseComboBox.SelectedItem?.ToString() ?? "";
            screenPanel.Controls.Add(databaseComboBox);
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

        private void PopulateFrameworks()
        {
            frameworkListBox.Items.Clear();
            config.Frameworks.Clear();

            string[] frameworks = languageComboBox.SelectedItem?.ToString() switch
            {
                "JavaScript" => new[] { "React", "Vue", "Angular", "Next.js", "Nuxt", "Express", "Fastify" },
                "TypeScript" => new[] { "React", "Vue", "Angular", "Next.js", "NestJS", "Express", "Deno" },
                "Python" => new[] { "Django", "Flask", "FastAPI", "Pyramid", "Tornado", "Bottle" },
                "C#" => new[] { "ASP.NET Core", ".NET 6+", "Entity Framework", "Blazor", "WPF", "WinForms" },
                "Java" => new[] { "Spring Boot", "Hibernate", "Quarkus", "Dropwizard", "Play Framework" },
                "Go" => new[] { "Gin", "Echo", "Chi", "Buffalo", "Gorilla Mux" },
                "Rust" => new[] { "Actix", "Tokio", "Rocket", "Warp", "Axum" },
                "PHP" => new[] { "Laravel", "Symfony", "CodeIgniter", "Yii", "Slim" },
                "Ruby" => new[] { "Rails", "Sinatra", "Hanami", "Sequel", "ActiveRecord" },
                "Swift" => new[] { "Vapor", "Perfect", "Kitura" },
                "Kotlin" => new[] { "Spring Boot", "Ktor", "Quarkus", "Android" },
                _ => new[] { "None" }
            };

            foreach (var fw in frameworks)
                frameworkListBox.Items.Add(fw);
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
            if (languageComboBox.SelectedItem == null)
            {
                validationLabel.Text = "Please select a programming language";
                return false;
            }

            validationLabel.Text = "";
            return true;
        }

        public string GetValidationError() => validationLabel.Text;

        public void OnLoad()
        {
            languageComboBox.Focus();
        }

        public void OnUnload()
        {
            // Save selected frameworks
            config.Frameworks.Clear();
            foreach (var item in frameworkListBox.SelectedItems)
                config.Frameworks.Add(item.ToString() ?? "");
        }
    }
}
