using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.UI
{
    /// <summary>
    /// Tabbed interface for detailed project configuration
    /// Provides 9 advanced configuration tabs with conditional visibility
    /// </summary>
    public class ConfigurationTabs : TabControl
    {
        public event EventHandler ConfigurationChanged;

        private ProjectConfiguration configuration;
        private Dictionary<string, TabPage> tabPages = new Dictionary<string, TabPage>();

        public ConfigurationTabs(ProjectConfiguration config)
        {
            this.configuration = config;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.Padding = new Point(0, 0);
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = SystemColors.Control;

            // Create all 9 tabs (some may be hidden based on project type)
            CreateTab("Project Details", 0);
            CreateTab("Architecture & Design", 1);
            CreateTab("Frontend Specifics", 2);
            CreateTab("Backend Specifics", 3);
            CreateTab("Database Design", 4);
            CreateTab("Testing & Quality", 5);
            CreateTab("Dependencies", 6);
            CreateTab("Timeline & Constraints", 7);
            CreateTab("Documentation", 8);

            UpdateTabVisibility();
        }

        private void CreateTab(string name, int index)
        {
            TabPage page = new TabPage
            {
                Text = name,
                Padding = new Padding(10),
                BackColor = SystemColors.Control
            };

            // Add placeholder content for each tab
            Label placeholder = new Label
            {
                Text = $"{name} configuration\n\n(To be implemented in Phase 3)",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = SystemColors.GrayText,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic)
            };
            page.Controls.Add(placeholder);

            tabPages[name] = page;
            this.TabPages.Add(page);
        }

        /// <summary>
        /// Update tab visibility based on project type
        /// Some tabs are conditional (Frontend/Backend only for certain project types)
        /// </summary>
        private void UpdateTabVisibility()
        {
            string projectType = configuration.ProjectType;

            // All projects have these tabs
            ShowTab("Project Details");
            ShowTab("Architecture & Design");
            ShowTab("Testing & Quality");
            ShowTab("Dependencies");
            ShowTab("Timeline & Constraints");
            ShowTab("Documentation");

            // Conditional tabs based on project type
            if (projectType == "Web Application" || projectType == "Desktop Application")
            {
                ShowTab("Frontend Specifics");
                HideTab("Backend Specifics");
                HideTab("Database Design");
            }
            else if (projectType == "Backend API")
            {
                HideTab("Frontend Specifics");
                ShowTab("Backend Specifics");
                ShowTab("Database Design");
            }
            else if (projectType == "CLI Tool")
            {
                HideTab("Frontend Specifics");
                ShowTab("Backend Specifics");
                HideTab("Database Design");
            }
            else
            {
                // Default: show both for unknown types
                ShowTab("Frontend Specifics");
                ShowTab("Backend Specifics");
                ShowTab("Database Design");
            }
        }

        private void ShowTab(string name)
        {
            if (tabPages.TryGetValue(name, out TabPage page))
            {
                if (!this.TabPages.Contains(page))
                    this.TabPages.Add(page);
            }
        }

        private void HideTab(string name)
        {
            if (tabPages.TryGetValue(name, out TabPage page))
            {
                if (this.TabPages.Contains(page))
                    this.TabPages.Remove(page);
            }
        }

        public void SetConfiguration(ProjectConfiguration config)
        {
            this.configuration = config;
            UpdateTabVisibility();
            OnConfigurationChanged();
        }

        protected void OnConfigurationChanged()
        {
            ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
