using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UITabs
{
    /// <summary>
    /// Tab 5: Database Design
    /// Schema notes, relationships, indexes, migrations
    /// </summary>
    public class Tab5_DatabaseDesign : IConfigurationTab
    {
        private ProjectConfiguration config;
        private Panel tabPanel;
        private ComboBox normalizationComboBox;
        private CheckBox migrationsCheckBox;
        private CheckBox backupCheckBox;
        private TextBox schemaNotesTextBox;
        private TextBox relationshipsTextBox;
        private Label validationLabel;

        private static readonly string[] NormalizationLevels =
        {
            "1NF (First Normal Form)", "2NF (Second Normal Form)",
            "3NF (Third Normal Form)", "BCNF (Boyce-Codd Normal Form)",
            "Denormalized (for performance)", "NoSQL (Document-based)", "Graph-based"
        };

        public Tab5_DatabaseDesign(ProjectConfiguration configuration)
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

            // Normalization
            Label normLabel = CreateLabel("Normalization:", 10, yPos, labelWidth);
            tabPanel.Controls.Add(normLabel);

            normalizationComboBox = new ComboBox
            {
                Location = new Point(labelWidth + 20, yPos),
                Size = new Size(280, controlHeight),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var norm in NormalizationLevels)
                normalizationComboBox.Items.Add(norm);
            normalizationComboBox.SelectedIndex = 2;
            tabPanel.Controls.Add(normalizationComboBox);
            yPos += controlHeight + spacing;

            // Database Migrations
            migrationsCheckBox = new CheckBox
            {
                Text = "Use Database Migrations (Flyway/Liquibase/Alembic)",
                Location = new Point(10, yPos),
                Size = new Size(350, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(migrationsCheckBox);
            yPos += controlHeight + spacing;

            // Automated Backups
            backupCheckBox = new CheckBox
            {
                Text = "Enable Automated Backups",
                Location = new Point(10, yPos),
                Size = new Size(350, 20),
                AutoSize = false,
                Checked = true
            };
            tabPanel.Controls.Add(backupCheckBox);
            yPos += controlHeight + spacing;

            // Schema Notes
            Label schemaNoteLabel = new Label
            {
                Text = "Schema Overview:",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(schemaNoteLabel);

            schemaNotesTextBox = new TextBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(420, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "Document main tables, fields, and structure"
            };
            tabPanel.Controls.Add(schemaNotesTextBox);
            yPos += 120;

            // Relationships Notes
            Label relLabel = new Label
            {
                Text = "Key Relationships:",
                Location = new Point(10, yPos),
                Size = new Size(300, 20),
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            tabPanel.Controls.Add(relLabel);

            relationshipsTextBox = new TextBox
            {
                Location = new Point(10, yPos + 25),
                Size = new Size(420, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "Document table relationships and foreign keys"
            };
            tabPanel.Controls.Add(relationshipsTextBox);
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

            config.AdvancedConfig["NormalizationLevel"] = normalizationComboBox.SelectedItem?.ToString() ?? "";
            config.AdvancedConfig["DatabaseMigrations"] = migrationsCheckBox.Checked;
            config.AdvancedConfig["AutomatedBackups"] = backupCheckBox.Checked;
            config.AdvancedConfig["SchemaOverview"] = schemaNotesTextBox.Text;
            config.AdvancedConfig["KeyRelationships"] = relationshipsTextBox.Text;
        }
    }
}
