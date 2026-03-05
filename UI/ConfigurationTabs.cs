using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ProjectSpecGUI.Core;
using ProjectSpecGUI.UITabs;

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
        private Dictionary<string, IConfigurationTab> tabImplementations = new Dictionary<string, IConfigurationTab>();

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

            // Initialize all 9 tab implementations
            InitializeTabImplementations();

            // Create all 9 tabs (some may be hidden based on project type)
            CreateTab("Project Details", new Tab1_ProjectDetails(configuration));
            CreateTab("Architecture & Design", new Tab2_ArchitectureDesign(configuration));
            CreateTab("Frontend Specifics", new Tab3_FrontendSpecifics(configuration));
            CreateTab("Backend Specifics", new Tab4_BackendSpecifics(configuration));
            CreateTab("Database Design", new Tab5_DatabaseDesign(configuration));
            CreateTab("Testing & Quality", new Tab6_TestingQuality(configuration));
            CreateTab("Dependencies", new Tab7_DependenciesIntegration(configuration));
            CreateTab("Timeline & Constraints", new Tab8_TimelineConstraints(configuration));
            CreateTab("Documentation", new Tab9_Documentation(configuration));

            UpdateTabVisibility();
        }

        private void InitializeTabImplementations()
        {
            tabImplementations["Project Details"] = new Tab1_ProjectDetails(configuration);
            tabImplementations["Architecture & Design"] = new Tab2_ArchitectureDesign(configuration);
            tabImplementations["Frontend Specifics"] = new Tab3_FrontendSpecifics(configuration);
            tabImplementations["Backend Specifics"] = new Tab4_BackendSpecifics(configuration);
            tabImplementations["Database Design"] = new Tab5_DatabaseDesign(configuration);
            tabImplementations["Testing & Quality"] = new Tab6_TestingQuality(configuration);
            tabImplementations["Dependencies"] = new Tab7_DependenciesIntegration(configuration);
            tabImplementations["Timeline & Constraints"] = new Tab8_TimelineConstraints(configuration);
            tabImplementations["Documentation"] = new Tab9_Documentation(configuration);
        }

        private void CreateTab(string name, IConfigurationTab tabImpl)
        {
            TabPage page = new TabPage
            {
                Text = name,
                Padding = new Padding(0),
                BackColor = SystemColors.Control
            };

            // Add the actual tab content
            Control tabControl = tabImpl.GetTabControl();
            tabControl.Dock = DockStyle.Fill;
            page.Controls.Add(tabControl);

            tabPages[name] = page;
            tabImplementations[name] = tabImpl;
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
            if (projectType == "Web App" || projectType == "Desktop App")
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
            else if (projectType == "Mobile App")
            {
                ShowTab("Frontend Specifics");
                HideTab("Backend Specifics");
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
            // Call OnUnload for all tabs to save their state
            foreach (var tabImpl in tabImplementations.Values)
            {
                tabImpl.OnUnload();
            }
            ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
