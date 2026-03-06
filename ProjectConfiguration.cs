using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSpecGUI.Core
{
    /// <summary>
    /// Core data model representing a complete project configuration
    /// </summary>
    public class ProjectConfiguration
    {
        // === WIZARD DATA ===
        public string ProjectName { get; set; } = "";
        public string ProjectDescription { get; set; } = "";
        public string ProjectType { get; set; } = ""; // Web App, Desktop App, Backend API, CLI Tool
        public string ComplexityLevel { get; set; } = "Medium"; // Simple, Medium, Advanced, Enterprise
        public string TargetAudience { get; set; } = "";

        // === TECHNOLOGY STACK ===
        public string PrimaryLanguage { get; set; } = "JavaScript";
        public List<string> Frameworks { get; set; } = new List<string>();
        public string Database { get; set; } = "None";
        public List<string> Libraries { get; set; } = new List<string>();

        // === KEY FEATURES ===
        public Dictionary<string, bool> Features { get; set; } = new Dictionary<string, bool>
        {
            { "Authentication", false },
            { "Authorization", false },
            { "Database Integration", false },
            { "REST API", false },
            { "Real-time Updates", false },
            { "File Upload/Download", false },
            { "Payment Integration", false },
            { "Email/Notifications", false },
            { "Admin Dashboard", false },
            { "Analytics/Logging", false },
            { "Search Functionality", false },
            { "User Management", false }
        };

        // === UI/UX REQUIREMENTS ===
        public string DesignFramework { get; set; } = "Bootstrap";
        public bool ResponsiveDesign { get; set; } = true;
        public bool DarkModeSupport { get; set; } = false;
        public string AccessibilityRequirements { get; set; } = "WCAG 2.1 AA";
        public bool MobileCompatibility { get; set; } = true;

        // === PERFORMANCE & SCALABILITY ===
        public string ExpectedUsers { get; set; } = "1000";
        public bool RealtimeDataNeeds { get; set; } = false;
        public string CachingStrategy { get; set; } = "None";
        public bool CDNUsage { get; set; } = false;
        public string DatabaseOptimization { get; set; } = "Standard indexes";

        // === DEPLOYMENT & DEVOPS ===
        public string HostingPlatform { get; set; } = "AWS";
        public string CIPipeline { get; set; } = "GitHub Actions";
        public bool UseDocker { get; set; } = false;
        public bool UseKubernetes { get; set; } = false;
        public string MonitoringTools { get; set; } = "CloudWatch";

        // === ADVANCED CONFIGURATION (from tabs) ===
        public Dictionary<string, object> AdvancedConfig { get; set; } = new Dictionary<string, object>();

        // === PROJECT METADATA ===
        public string Author { get; set; } = "";
        public string Version { get; set; } = "1.0.0";
        public string License { get; set; } = "MIT";
        public string RepositoryUrl { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Generate a markdown specification from the configuration
        /// </summary>
        public string GenerateMarkdownSpecification()
        {
            var sb = new StringBuilder();

            sb.AppendLine("# Project Specification");
            sb.AppendLine();
            sb.AppendLine($"**Project Name:** {ProjectName}");
            sb.AppendLine($"**Type:** {ProjectType}");
            sb.AppendLine($"**Complexity:** {ComplexityLevel}");
            sb.AppendLine($"**Created:** {CreatedDate:yyyy-MM-dd}");
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(ProjectDescription))
            {
                sb.AppendLine("## Description");
                sb.AppendLine(ProjectDescription);
                sb.AppendLine();
            }

            sb.AppendLine("## Technology Stack");
            sb.AppendLine($"- **Language:** {PrimaryLanguage}");
            if (Frameworks.Count > 0)
                sb.AppendLine($"- **Frameworks:** {string.Join(", ", Frameworks)}");
            sb.AppendLine($"- **Database:** {Database}");
            if (Libraries.Count > 0)
                sb.AppendLine($"- **Libraries:** {string.Join(", ", Libraries)}");
            sb.AppendLine();

            var selectedFeatures = Features.Where(f => f.Value).Select(f => f.Key).ToList();
            if (selectedFeatures.Count > 0)
            {
                sb.AppendLine("## Key Features");
                foreach (var feature in selectedFeatures)
                    sb.AppendLine($"- {feature}");
                sb.AppendLine();
            }

            sb.AppendLine("## UI/UX Requirements");
            sb.AppendLine($"- **Design Framework:** {DesignFramework}");
            sb.AppendLine($"- **Responsive Design:** {(ResponsiveDesign ? "Yes" : "No")}");
            sb.AppendLine($"- **Dark Mode:** {(DarkModeSupport ? "Yes" : "No")}");
            sb.AppendLine($"- **Accessibility:** {AccessibilityRequirements}");
            sb.AppendLine($"- **Mobile Compatible:** {(MobileCompatibility ? "Yes" : "No")}");
            sb.AppendLine();

            sb.AppendLine("## Performance & Scalability");
            sb.AppendLine($"- **Expected Users:** {ExpectedUsers}");
            sb.AppendLine($"- **Real-time Data:** {(RealtimeDataNeeds ? "Yes" : "No")}");
            sb.AppendLine($"- **Caching Strategy:** {CachingStrategy}");
            sb.AppendLine($"- **CDN Usage:** {(CDNUsage ? "Yes" : "No")}");
            sb.AppendLine();

            sb.AppendLine("## Deployment & DevOps");
            sb.AppendLine($"- **Hosting:** {HostingPlatform}");
            sb.AppendLine($"- **CI/CD Pipeline:** {CIPipeline}");
            sb.AppendLine($"- **Docker:** {(UseDocker ? "Yes" : "No")}");
            sb.AppendLine($"- **Kubernetes:** {(UseKubernetes ? "Yes" : "No")}");
            sb.AppendLine($"- **Monitoring:** {MonitoringTools}");
            sb.AppendLine();

            sb.AppendLine("## Project Metadata");
            sb.AppendLine($"- **Author:** {Author}");
            sb.AppendLine($"- **Version:** {Version}");
            sb.AppendLine($"- **License:** {License}");
            if (!string.IsNullOrWhiteSpace(RepositoryUrl))
                sb.AppendLine($"- **Repository:** {RepositoryUrl}");
            sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        /// Generate a prompt for Claude based on this configuration
        /// </summary>
        public string GenerateClaudePrompt()
        {
            var spec = GenerateMarkdownSpecification();
            return $@"I want you to build a new project based on this specification:

{spec}

Please provide a comprehensive plan for implementing this project, including:
1. Detailed file structure
2. Technology setup and configuration
3. Implementation phases
4. Key components and their interactions
5. Testing strategy
6. Deployment approach

Then, generate the initial code structure and core files needed to get started.";
        }

        /// <summary>
        /// Validate the configuration for completeness
        /// </summary>
        public List<string> Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(ProjectName))
                errors.Add("Project name is required");

            if (string.IsNullOrWhiteSpace(ProjectType))
                errors.Add("Project type must be selected");

            if (string.IsNullOrWhiteSpace(PrimaryLanguage))
                errors.Add("Primary language must be selected");

            if (Frameworks.Count == 0)
                errors.Add("At least one framework should be selected");

            return errors;
        }
    }
}
